import sys
import json
import pyodbc
from datetime import datetime

# 🔹 Load Database Configuration from JSON File
try:
    with open("appsettings.json", "r") as config_file:
        config = json.load(config_file)
        db_settings = config["DatabaseSettings"]
except Exception as e:
    print(json.dumps({"error": f"Error loading config: {str(e)}"}))
    sys.exit(1)

# 🔹 Establish Database Connection
try:
    conn = pyodbc.connect(
        f"DRIVER={db_settings['Driver']};"
        f"SERVER={db_settings['Server']};"
        f"DATABASE={db_settings['Database']};"
        f"UID={db_settings['User']};"
        f"PWD={db_settings['Password']}"
    )
    cursor = conn.cursor()
except Exception as e:
    print(json.dumps({"error": f"Database Connection Error: {str(e)}"}))
    sys.exit(1)

# 🔹 Function to Convert SQL Query Results to JSON
def execute_sql_query(sql_query):
    try:
        cursor.execute(sql_query)
        columns = [column[0] for column in cursor.description]
        rows = cursor.fetchall()

        # 🔹 Convert datetime objects to string format
        def serialize_value(value):
            if isinstance(value, datetime):
                return value.strftime("%Y-%m-%d %H:%M:%S")
            return value

        # 🔹 Process query results into structured JSON
        data = [[serialize_value(cell) for cell in row] for row in rows]
        
        response = {
            "sql_query": sql_query,
            "result": {
                "columns": columns,
                "data": data
            }
        }
        return json.dumps(response)
    except Exception as e:
        return json.dumps({"error": str(e)})

# 🔹 Main Execution
if __name__ == "__main__":
    if len(sys.argv) < 2:
        print(json.dumps({"error": "Usage: python execute_query.py <sql_query>"}))
        sys.exit(1)

    sql_query = sys.argv[1]
    print(execute_sql_query(sql_query))
