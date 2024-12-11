
import requests
# Define the API endpoint
url = "http://127.0.0.1:5000/predict"

# Define the input data
data = {
    "sepal_length": 5.4,
    "sepal_width": 3.9,
    "petal_length": 1.7,
    "petal_width": 0.4
}

# Send a POST request to the API
response = requests.post(url, json=data)

# Print the response
print(response.json())
