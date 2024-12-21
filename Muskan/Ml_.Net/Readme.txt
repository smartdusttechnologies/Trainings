📖 Project Overview
This project integrates an ASP.NET Core MVC application with a Python Flask API to predict job titles from resumes uploaded in PDF format. The ASP.NET Core application sends input data to a Flask server running a machine learning model, receives predictions, and displays the results seamlessly to the user.


## 🔍 Key Features

- **ASP.NET Core MVC Integration** 

- **HTTP POST Requests**    

- **JSON Serialization & Deserialization**    

- **User Interaction**  
  
Pair of Projects :

Python(endpoint)                                                    Asp.net 

Endpoint(For Ml multiple pdf upload)                                Ml_Models_In_Vs
Deep_Learning_Resume_Endpoint(For DL multiple pdf upload)           Ml_Models_In_Vs
Model_with_zip_file_endpoint(For Dl Zip file upload)                Ml_Models_In_VS_Zip_file
 

⚡ Step 1: Clone the Repository
Clone the repository to your local machine:
   git clone https://github.com/smartdusttechnologies/Trainings.git

   
   📘 For ASP.NET Core Application
   Install .NET SDK:
   Ensure you have the .NET SDK installed on your system. Download it from the official site:
   👉 Download .NET SDK ( https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
 The version .net is 6.0 
   Open the Project:
   Open your project in either Visual Studio or Visual Studio Code.

   Restore NuGet Packages:
   Navigate to the project root folder and run:
   --> dotnet restore
   Configure Flask Service:
   Before running your ASP.NET project, ensure that the Flask server is running on:
   🌐 http://127.0.0.1:5000
 
 3. For Python(Endpoint)
 This project requires Python 3.10.6. If you have a different version, you can download the required version from:
https://www.python.org/downloads/release/python-3106/ 
  A. Make virtual environment 
 # Create a virtual environment
    python -m venv venv  
    If you encounter version issues, use:
    py -3.10 -m venv venv  (Make sure you already install python3.10.6)


 # Activate the virtual environment
    # Windows
    venv\Scripts\activate

    # macOS/Linux
    source venv/bin/activate
 B. Install the required packages: 

   pip install -r requirements.txt 
If you are using python3.10.6
   py -3.10 -m pip install -r requirements.txt

 To run the machine learning model
py -3.10 -m spacy download en_core_web_sm  

   

📚 Usage
✅ 1. Start the Flask Server
   python main.py 
✅ 2. Run the ASP.NET Core Project
   dotnet run
✅ 3. After this in Resume page you can upload the resume and find the job recommendations.


Train the Model 
1. Install Jupyter Notebook:
To train or re-train the model, you can use Jupyter Notebook. First, install it:

pip install notebook

2. Start Jupyter Notebook:
Open the terminal/command prompt and run the command to start the notebook:
jupyter notebook

3. Setup Environment for Model Training:
Install additional dependencies for the training environment:
!pip install pandas numpy nltk tensorflow scikit-learn matplotlib seaborn PyPDF2 joblib gensim
 
4. Open the Notebook:
Open the Deep Learning Resume Matcher.ipynb or Resume Matcher.ipynb file in Jupyter to start model training.


