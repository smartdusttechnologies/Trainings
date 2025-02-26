import pyodbc
import pandas as pd
import torch
from transformers import T5Tokenizer, T5ForConditionalGeneration, TrainingArguments, Trainer
from datasets import Dataset
from sklearn.model_selection import train_test_split
from torch.utils.data import DataLoader
import numpy as np
import pickle
import os
import warnings

# Suppress Future Warnings
warnings.simplefilter(action="ignore", category=FutureWarning)

# Define Base Directories
BASE_DIR = os.path.abspath(os.path.dirname(__file__))  
WWWROOT_DIR = os.path.join(BASE_DIR, "../wwwroot/python_model")  

os.makedirs(os.path.join(WWWROOT_DIR, "dataset"), exist_ok=True)
os.makedirs(os.path.join(WWWROOT_DIR, "sql_model"), exist_ok=True)

# Database Connection
DB_CONNECTION = 'DRIVER={ODBC Driver 17 for SQL Server};SERVER=147.79.67.111;DATABASE=TestingAndCalibration;UID=sa;PWD=Prem@9123188;'
conn = pyodbc.connect(DB_CONNECTION)
cursor = conn.cursor()

# Fetch all table names
cursor.execute("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';")
tables = [row[0] for row in cursor.fetchall()]

# Generate Training Data
data = []
for table in tables:
    cursor.execute("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=?", table)
    columns = [col[0] for col in cursor.fetchall()]
    
    if not columns:
        continue

    queries = [
        (f"Get all records from {table}", f"SELECT * FROM {table};"),
        (f"Show all {', '.join(columns)} from {table}", f"SELECT {', '.join(columns)} FROM {table};"),
        (f"Find {columns[0]} in {table} where {columns[0]} is greater than 50", f"SELECT {columns[0]} FROM {table} WHERE {columns[0]} > 50;")
    ]
    
    data.extend(queries)

df = pd.DataFrame(data, columns=["nl_query", "sql_query"])
df.to_csv(os.path.join(WWWROOT_DIR, "dataset/dataset.csv"), index=False)

# Split Data
train_data, test_data = train_test_split(df, test_size=0.2, random_state=42)
train_data.to_csv(os.path.join(WWWROOT_DIR, "dataset/train_data.csv"), index=False)
test_data.to_csv(os.path.join(WWWROOT_DIR, "dataset/test_data.csv"), index=False)

# Load Tokenizer & Model
model_name = "t5-small"
tokenizer = T5Tokenizer.from_pretrained(model_name, legacy=False)
model = T5ForConditionalGeneration.from_pretrained(model_name)

# Data Preprocessing
def preprocess_function(examples):
    inputs = [f"translate English to SQL: {query}" for query in examples["nl_query"]]
    targets = [query for query in examples["sql_query"]]

    model_inputs = tokenizer(inputs, max_length=256, truncation=True, padding="max_length", return_tensors="pt")
    
    with tokenizer.as_target_tokenizer():
        labels = tokenizer(targets, max_length=256, truncation=True, padding="max_length").input_ids

    model_inputs["labels"] = [[(l if l != tokenizer.pad_token_id else -100) for l in label] for label in labels]
    
    return model_inputs

train_dataset = Dataset.from_pandas(train_data).map(preprocess_function, batched=True)
test_dataset = Dataset.from_pandas(test_data).map(preprocess_function, batched=True)

# Training Configuration
training_args = TrainingArguments(
    output_dir=WWWROOT_DIR,
    evaluation_strategy="epoch",
    learning_rate=3e-5,
    per_device_train_batch_size=4,
    per_device_eval_batch_size=4,
    num_train_epochs=5,
    weight_decay=0.01,
    save_total_limit=1,
    logging_dir=os.path.join(WWWROOT_DIR, "logs"),
    report_to="none"
)

trainer = Trainer(
    model=model,
    args=training_args,
    train_dataset=train_dataset,
    eval_dataset=test_dataset
)

# Train the Model
trainer.train()

# Evaluate Model Accuracy
def compute_accuracy(model, test_dataset):
    dataloader = DataLoader(test_dataset.remove_columns(["nl_query", "sql_query"]), batch_size=4)
    correct, total = 0, 0

    model.eval()
    with torch.no_grad():
        for batch in dataloader:
            batch = {key: torch.tensor(batch[key]).to(model.device) if isinstance(batch[key], list) else batch[key].to(model.device) for key in ["input_ids", "attention_mask"]}
            outputs = model.generate(**batch)
            predictions = tokenizer.batch_decode(outputs, skip_special_tokens=True)
            actual = tokenizer.batch_decode(batch["labels"], skip_special_tokens=True)

            correct += sum(1 for p, a in zip(predictions, actual) if p.strip().lower() == a.strip().lower())
            total += len(actual)

    return correct / total if total > 0 else 0

accuracy = compute_accuracy(model, test_dataset)
print(f"Model Accuracy: {accuracy:.2%}")

# Save Model & Tokenizer
model.save_pretrained(WWWROOT_DIR)
tokenizer.save_pretrained(WWWROOT_DIR)

# Save Model as .pkl File
pkl_model_path = os.path.join(WWWROOT_DIR, "sql_model.pkl")
with open(pkl_model_path, "wb") as f:
    pickle.dump(model, f)

print(f"✅ Training completed. Model and tokenizer saved in {WWWROOT_DIR}")
print(f"✅ Model saved as .pkl at: {pkl_model_path}")
