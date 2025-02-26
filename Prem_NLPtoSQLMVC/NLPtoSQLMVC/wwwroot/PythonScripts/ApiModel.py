import os
import google.generativeai as genai
import re
import sys

# 🔹 Configure Google Gemini API
API_KEY = "AIzaSyDCR4RB13np-rWIF58wZOTgFRkPU5zTQME"
genai.configure(api_key=API_KEY)

def convert_nlp_to_sql(nlp_query):
    """
    Converts a natural language query to a valid Microsoft SQL Server (MSSQL) query
    using the Google Gemini API.
    """
    model = genai.GenerativeModel("gemini-1.5-pro")

    prompt = f"""
    Convert the following natural language query into a valid Microsoft SQL Server (MSSQL) query.
    - Only return the SQL query itself. Do NOT include "mssql", explanations, or any other text.
    - Use square brackets [ ] for table and column names when necessary.
    - Do NOT use backticks (`).
    - Do NOT use semicolons (;).
    - Generate a valid query that works directly in MSSQL.

    NLP Query: {nlp_query}
    """

    response = model.generate_content(prompt)

    if response and hasattr(response, "text"):
        sql_query = response.text.strip()

        # 🔹 Remove unwanted text
        sql_query = re.sub(r"(?i)^mssql\s*", "", sql_query)  # Remove "mssql" prefix if present
        sql_query = sql_query.replace("`", "")  # Remove accidental backticks
        
        return sql_query
    else:
        return "❌ Failed to generate SQL query."

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("❌ Error: Please provide an NLP query.")
        sys.exit(1)

    nlp_query = sys.argv[1]
    sql_query = convert_nlp_to_sql(nlp_query)
    print(sql_query)
