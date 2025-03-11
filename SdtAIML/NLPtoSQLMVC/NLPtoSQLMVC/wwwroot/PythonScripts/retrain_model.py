import pandas as pd
import torch
import os
from transformers import T5Tokenizer, T5ForConditionalGeneration, TrainingArguments, Trainer
from datasets import Dataset
from sklearn.model_selection import train_test_split
from torch.utils.data import DataLoader

def retrain():
    device = "cuda" if torch.cuda.is_available() else "cpu"
    print(f"Using device: {device}")

    os.environ["HF_HUB_DISABLE_SYMLINKS_WARNING"] = "0"

    corrections_file = "FeedbackData/corrections.csv"
    if not os.path.exists(corrections_file):
        print("No corrections found. Retraining skipped.")
        return

    corrections_data = pd.read_csv(corrections_file, names=["nl_query", "sql_query"],header=None)
    if corrections_data.empty:
        print("No new corrections found. Retraining skipped.")
        return

    model_name = "t5-small"
    tokenizer = T5Tokenizer.from_pretrained("./sql_model")
    model = T5ForConditionalGeneration.from_pretrained("./sql_model").to(device)

    def preprocess_function(examples):
        inputs = [f"translate English to SQL: {query}" for query in examples["nl_query"]]
        targets = [query for query in examples["sql_query"]]

        model_inputs = tokenizer(inputs, max_length=128, truncation=True, padding="max_length", return_tensors="pt")
        labels = tokenizer(targets, max_length=128, truncation=True, padding="max_length")["input_ids"]
        labels = [[(l if l != tokenizer.pad_token_id else -100) for l in label] for label in labels]

        model_inputs["labels"] = labels
        return model_inputs

    correction_dataset = Dataset.from_pandas(corrections_data)
    correction_dataset = correction_dataset.map(preprocess_function, batched=True, remove_columns=["nl_query", "sql_query"])

    def compute_accuracy(model, dataset, tokenizer):
        dataloader = DataLoader(dataset.with_format("torch"), batch_size=8)
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
        return accuracy

    # Calculate accuracy before retraining
    initial_accuracy = compute_accuracy(model, correction_dataset, tokenizer)
    print(f"Initial accuracy on corrections: {initial_accuracy * 100:.2f}%")

    training_args = TrainingArguments(
        output_dir="./sql_model",
        learning_rate=5e-4,  # Higher learning rate for faster convergence
        per_device_train_batch_size=8,
        num_train_epochs=2,  # Only a few epochs for corrections
        logging_dir="./logs",
        report_to="none"
    )

    trainer = Trainer(
        model=model,
        args=training_args,
        train_dataset=correction_dataset,
    )
    print("🚀 Retraining Started...")
    trainer.train()
    print("✅ Retraining Completed Successfully!")

    # Calculate accuracy after retraining
    final_accuracy = compute_accuracy(model, correction_dataset, tokenizer)
    print(f"Final accuracy on corrections: {final_accuracy * 100:.2f}%")
    print(f"Accuracy improvement: {(final_accuracy - initial_accuracy) * 100:.2f}%")

    model.save_pretrained("./resql_model")
    tokenizer.save_pretrained("./resql_model")

    print("✅ Model Retraining & Saving Completed Successfully!")

if __name__ == "__main__":
    retrain()