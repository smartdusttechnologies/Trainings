import os
import sys
import json
import pyodbc
import torch
from transformers import T5Tokenizer, T5ForConditionalGeneration
from datetime import datetime
import logging

# 🔹 Suppress Warnings & Logs
os.environ["TF_CPP_MIN_LOG_LEVEL"] = "3"  
os.environ["TF_ENABLE_ONEDNN_OPTS"] = "0"  
sys.stderr = open(os.devnull, "w")  # Suppress stderr output
logging.disable(logging.CRITICAL)  # Disable logging
device = "cpu"

# 🔹 Load Model & Tokenizer
MODEL_PATH = "wwwroot/PythonScripts/sql_model"
try:
    tokenizer = T5Tokenizer.from_pretrained(MODEL_PATH)
    with torch.no_grad():
        model = T5ForConditionalGeneration.from_pretrained(MODEL_PATH).to(device)
except Exception as e:
    print(json.dumps({"error": f"Model Loading Error: {str(e)}"}))
    sys.exit(1)

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

# 🔹 Setup Database Connection
db_settings = load_db_config()

if db_settings:
    DB_CONNECTION = (
        f"DRIVER={{{db_settings['Driver']}}};"
        f"SERVER={db_settings['Server']};"
        f"DATABASE={db_settings['Database']};"
        f"UID={db_settings['User']};"
        f"PWD={db_settings['Password']};"
    )
else:
    print(json.dumps({"error": "Database settings could not be loaded."}))
    exit(1)

# 🔹 Convert Data for JSON Response
def convert_to_serializable(obj):
    if isinstance(obj, (int, float)):
        return str(obj)  
    elif isinstance(obj, datetime):
        return obj.isoformat()  
    elif isinstance(obj, bytes):  
        return obj.decode("utf-8", errors="ignore")  
    elif obj is None:  
        return "null"
    else:
        return str(obj)  

# 🔹 Execute SQL Query and Fetch Data
def fetch_data_from_db(sql_query):
    try:
        conn = pyodbc.connect(DB_CONNECTION, timeout=5)
        cursor = conn.cursor()
        cursor.execute(sql_query)
        columns = [desc[0] for desc in cursor.description]  
        rows = cursor.fetchall()  

        serializable_rows = [[convert_to_serializable(val) for val in row] for row in rows]

        cursor.close()
        conn.close()

        return {"columns": columns, "data": serializable_rows} if rows else {"columns": columns, "data": []}
    except pyodbc.Error as db_err:
        return {"error": f"Database Connection Error: {str(db_err)}"}
    except Exception as e:
        return {"error": f"SQL Execution Error: {str(e)}"}

# 🔹 Predict SQL Query from Natural Language
def predict_sql(nl_query):
    try:
        model_input = tokenizer(
            f"translate English to SQL: {nl_query}",
            return_tensors="pt",
            padding="max_length",
            truncation=True,
            max_length=128,
        ).to(device)

        with torch.no_grad():
            output = model.generate(
                input_ids=model_input["input_ids"],
                attention_mask=model_input["attention_mask"],
                num_beams=5,
            )

        return tokenizer.decode(output[0], skip_special_tokens=True)
    except Exception as e:
        return f"Prediction Error: {e}"

# 🔹 Process User Query
def process_user_query(user_query):
    predicted_sql = predict_sql(user_query)
    if "Error" in predicted_sql:
        return {"error": predicted_sql}

    result = fetch_data_from_db(predicted_sql)
    if "error" in result:
        return {"error": result["error"]}

    return {
        "sql_query": predicted_sql,
        "result": {
            "columns": result["columns"],
            "data": result["data"],
        },
    }

# 🔹 Main Execution
if __name__ == "__main__":
    try:
        if len(sys.argv) != 2:
            print(json.dumps({"error": "Usage: python predict.py <user_query>"}))
            sys.exit(1)

        user_query = sys.argv[1]
        result = process_user_query(user_query)
        print(json.dumps(result))
    except Exception as e:
        print(json.dumps({"error": f"Unexpected error: {str(e)}"}))
