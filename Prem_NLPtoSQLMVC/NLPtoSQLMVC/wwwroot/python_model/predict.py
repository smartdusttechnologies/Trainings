import os
import torch
import pyodbc
import pandas as pd
from transformers import T5Tokenizer, T5ForConditionalGeneration

# Define model path using os.path.join for cross-platform support
MODEL_DIR = os.path.join("wwwroot", "python_model")
TOKENIZER_PATH = os.path.join(MODEL_DIR, "sql_tokenizer")
MODEL_PATH = os.path.join(MODEL_DIR, "sql_model")

# Load Model & Tokenizer for Prediction
try:
    tokenizer = T5Tokenizer.from_pretrained(TOKENIZER_PATH)
    model = T5ForConditionalGeneration.from_pretrained(MODEL_PATH)
    print("Model and tokenizer loaded successfully.")
except Exception as e:
    print(f"Error loading model/tokenizer: {e}")
    exit()

# Database Connection
DB_CONNECTION = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=147.79.67.111;"
    "DATABASE=TestingAndCalibration;"
    "UID=sa;"
    "PWD=Prem@9123188;"
)

try:
    conn = pyodbc.connect(DB_CONNECTION)
    cursor = conn.cursor()
    print("Database connection successful.")
except Exception as e:
    print(f"Error connecting to database: {e}")
    exit()

# Function to Predict SQL Query
def predict_sql(natural_language_query):
    input_text = f"translate English to SQL: {natural_language_query}"
    input_ids = tokenizer(input_text, return_tensors="pt").input_ids

    with torch.no_grad():
        output_ids = model.generate(input_ids)

    predicted_sql = tokenizer.decode(output_ids[0], skip_special_tokens=True)
    return predicted_sql

# Function to Execute SQL Query & Fetch Data
def execute_sql_query(sql_query):
    try:
        cursor.execute(sql_query)
        if cursor.description is None:
            return "Query executed successfully, but no records found."
        
        columns = [column[0] for column in cursor.description]  # Get column names
        rows = cursor.fetchall()  # Fetch all results
        
        if not rows:
            return "No records found."

        # Convert to DataFrame
        df = pd.DataFrame.from_records(rows, columns=columns)
        return df
    except Exception as e:
        return f"Error executing query: {e}"

# Main Function to Handle User Interaction
def main():
    while True:
        user_query = input("\nEnter an English query (or 'exit' to quit): ")
        if user_query.lower() == "exit":
            print("Exiting program.")
            break

        sql_query = predict_sql(user_query)
        print(f"\nPredicted SQL Query: {sql_query}")

        # Fetch data from database
        result = execute_sql_query(sql_query)
        print("\nQuery Results:")
        print(result)

# Run the main function
if __name__ == "__main__":
    main()

# Close the database connection when script ends
cursor.close()
conn.close()
