import pyodbc
import sys
import json
from datetime import datetime  # Import datetime for conversion

# 🔹 MSSQL Connection Settings
server = '147.79.67.111'
database = 'TestingAndCalibration'
username = 'sa'
password = 'Prem@9123188'
driver = '{ODBC Driver 17 for SQL Server}'

try:
    conn = pyodbc.connect(
        f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password}'
    )
    cursor = conn.cursor()
except Exception as e:
    print(json.dumps({"error": str(e)}))
    exit()

# 🔹 Function to Convert SQL Query Results to JSON
def execute_sql_query(sql_query):
    try:
        cursor.execute(sql_query)
        columns = [column[0] for column in cursor.description]
        rows = cursor.fetchall()

        # 🔹 Convert datetime objects to string format
        def serialize_value(value):
            if isinstance(value, datetime):  # Convert datetime to string
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
    sql_query = sys.argv[1]
    print(execute_sql_query(sql_query))
