import pyodbc
import pandas as pd
import torch
import os
import json
from transformers import T5Tokenizer, T5ForConditionalGeneration, TrainingArguments, Trainer
from datasets import Dataset
from sklearn.model_selection import train_test_split
from torch.utils.data import DataLoader

# 🔹 Load Database Configuration from appsettings.json
def load_db_config():
    try:
        # ✅ Get the correct absolute path for appsettings.json
        script_dir = os.path.dirname(os.path.abspath(__file__))  
        config_path = os.path.join(script_dir, "..", "..", "appsettings.json")  

        if not os.path.exists(config_path):
            raise FileNotFoundError(f"Config file not found: {config_path}")

        with open(config_path, "r") as file:
            config = json.load(file)
            db_config = config.get("DatabaseSettings", None)

            if not db_config:
                raise ValueError("DatabaseSettings section is missing in appsettings.json")

            return db_config
    except Exception as e:
        print(json.dumps({"error": f"Error loading database config: {str(e)}"}))
        return None

def main():
    # GPU Enable
    device = "cuda" if torch.cuda.is_available() else "cpu"
    print(f"Using device: {device}")

    # Hugging Face Auth Warning Suppress
    os.environ["HF_HUB_DISABLE_SYMLINKS_WARNING"] = "0"

    # Load database config
    db_settings = load_db_config()

    # Database Connection
    DB_CONNECTION = (f"DRIVER={{{db_settings['Driver']}}};"
                     f"SERVER={db_settings['Server']};"
                     f"DATABASE={db_settings['Database']};"
                     f"UID={db_settings['User']};"
                     f"PWD={db_settings['Password']};"
                     "Connection Timeout=30;")
    try:
        conn = pyodbc.connect(DB_CONNECTION)
        cursor = conn.cursor()
        print("✅ Database Connection Successful!")
    except Exception as e:
        print(f"❌ Database Connection Failed: {e}")
        exit()

    # Fetch table names
    cursor.execute("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';")
    tables = [row[0] for row in cursor.fetchall()]

    data = []
    for table in tables:
        cursor.execute("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=?", table)
        columns = [col[0] for col in cursor.fetchall()]
        if not columns:
            continue
        queries = [
            (f"Get all records from {table}", f"SELECT * FROM {table};"),
            (f"Find top 10 records from {table}", f"SELECT TOP 10 * FROM {table};"),
            (f"Count total records in {table}", f"SELECT COUNT(*) FROM {table};"),
            (f"Find {columns[0]} from {table} where {columns[0]} > 50", f"SELECT {columns[0]} FROM {table} WHERE {columns[0]} > 50;"),
            (f"Find unique values in {columns[0]} from {table}", f"SELECT DISTINCT {columns[0]} FROM {table};"),
            (f"Find average of {columns[0]} from {table}", f"SELECT AVG({columns[0]}) FROM {table};"),
            (f"Find minimum of {columns[0]} from {table}", f"SELECT MIN({columns[0]}) FROM {table};"),
            (f"Find maximum of {columns[0]} from {table}", f"SELECT MAX({columns[0]}) FROM {table};"),
            (f"Find sum of {columns[0]} from {table}", f"SELECT SUM({columns[0]}) FROM {table};"),
            (f"Find count of {columns[0]} from {table}", f"SELECT COUNT({columns[0]}) FROM {table};"),
            (f"Find count of distinct {columns[0]} from {table}", f"SELECT COUNT(DISTINCT {columns[0]}) FROM {table};"),
            (f"Find count of null values in {columns[0]} from {table}", f"SELECT COUNT(*) - COUNT({columns[0]}) FROM {table};"),
            (f"Find count of non-null values in {columns[0]} from {table}", f"SELECT COUNT({columns[0]}) FROM {table};"),
            (f"Find count of records where {columns[0]} is null from {table}", f"SELECT COUNT(*) FROM {table} WHERE {columns[0]} IS NULL;"),
            (f"Find count of records where {columns[0]} is not null from {table}", f"SELECT COUNT(*) FROM {table} WHERE {columns[0]} IS NOT NULL;"),
            (f"Find count of records where {columns[0]} is between 10 and 20 from {table}", f"SELECT COUNT(*) FROM {table} WHERE {columns[0]} BETWEEN 10 AND 20;"),
            (f"Find count of records where {columns[0]} is not between 10 and 20 from {table}", f"SELECT COUNT(*) FROM {table} WHERE {columns[0]} NOT BETWEEN 10 AND 20;"),
            (f"Get all table names", f"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';")
        ]
        data.extend(queries)

    cursor.close()
    conn.close()

    # Save data to CSV
    csv_filename = "data.csv"
    df = pd.DataFrame(data, columns=["nl_query", "sql_query"])
    df.to_csv(csv_filename, index=False, encoding='utf-8')
    print(f"✅ Data saved to {csv_filename}")

    # Train-test split
    train_data, test_data = train_test_split(df, test_size=0.1, random_state=42)

    # Load Tokenizer & Model
    model_name = "t5-small"  # Smaller model for faster training
    tokenizer = T5Tokenizer.from_pretrained(model_name)
    model = T5ForConditionalGeneration.from_pretrained(model_name).to(device)

    # Preprocessing Function
    def preprocess_function(examples):
        inputs = [f"translate English to SQL: {query}" for query in examples["nl_query"]]
        targets = [query for query in examples["sql_query"]]

        model_inputs = tokenizer(inputs, max_length=128, truncation=True, padding="max_length", return_tensors="pt")
        labels = tokenizer(targets, max_length=128, truncation=True, padding="max_length")["input_ids"]
        labels = [[(l if l != tokenizer.pad_token_id else -100) for l in label] for label in labels]

        model_inputs["labels"] = labels
        return model_inputs

    # Convert DataFrame to Dataset
    train_dataset = Dataset.from_pandas(train_data)
    test_dataset = Dataset.from_pandas(test_data)

    # Apply Preprocessing
    train_dataset = train_dataset.map(preprocess_function, batched=True, remove_columns=["nl_query", "sql_query"])
    test_dataset = test_dataset.map(preprocess_function, batched=True, remove_columns=["nl_query", "sql_query"])

    # Training Configuration (Optimized for Speed)
    training_args = TrainingArguments(
        output_dir="./sql_model",
        evaluation_strategy="epoch",
        learning_rate=3e-4,  # Increased for faster convergence
        per_device_train_batch_size=8,  # Increased batch size
        per_device_eval_batch_size=8,
        gradient_accumulation_steps=2,  # Helps manage memory
        num_train_epochs=5,  # More epochs to improve accuracy
        weight_decay=0.01,
        logging_dir="./logs",
        save_total_limit=1,
        save_strategy="epoch",
        fp16=torch.cuda.is_available(),  # Use mixed precision if GPU available
        dataloader_num_workers=0,  # Set to 0 to avoid multiprocessing issues on Windows
        report_to="none"
    )

    # Trainer Setup
    trainer = Trainer(
        model=model,
        args=training_args,
        train_dataset=train_dataset,
        eval_dataset=test_dataset
    )

    # Start Training
    print("🚀 Training Started...")
    trainer.train()
    print("✅ Training Completed Successfully!")

    # Accuracy Calculation
    def compute_accuracy(model, test_dataset, tokenizer):
        dataloader = DataLoader(test_dataset.with_format("torch"), batch_size=8)
        correct, total = 0, 0
        model.eval()
        with torch.no_grad():
            for batch in dataloader:
                batch = {key: value.to(device) for key, value in batch.items() if key in ["input_ids", "attention_mask", "labels"]}
                outputs = model.generate(input_ids=batch["input_ids"], attention_mask=batch["attention_mask"], num_beams=5)

                predictions = [tokenizer.decode(output, skip_special_tokens=True) for output in outputs]
                actual_labels = batch["labels"].cpu().numpy()
                actuals = [tokenizer.decode([l for l in label if l != -100], skip_special_tokens=True) for label in actual_labels]

                correct += sum(1 if pred.strip().lower() == act.strip().lower() else 0 for pred, act in zip(predictions, actuals))
                total += len(actuals)

        accuracy = correct / total if total > 0 else 0
        print(f"🎯 Model Accuracy: {accuracy * 100:.2f}%")
        return accuracy

    # Compute Accuracy
    accuracy = compute_accuracy(model, test_dataset, tokenizer)

    # Save Model & Tokenizer
    model.save_pretrained("./sql_model")
    tokenizer.save_pretrained("./sql_model")

    print("✅ Model Training & Saving Completed Successfully!")

if __name__ == '__main__':
    main()