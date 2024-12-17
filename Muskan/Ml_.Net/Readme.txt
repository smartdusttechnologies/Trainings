📖 Project Overview
This project integrates an ASP.NET Core MVC application with a Python Flask API to predict job titles from resumes uploaded in PDF format. The ASP.NET Core application sends input data to a Flask server running a machine learning model, receives predictions, and displays the results seamlessly to the user.


## 🔍 Key Features

- **ASP.NET Core MVC Integration** 

- **HTTP POST Requests**    

- **JSON Serialization & Deserialization**    

- **User Interaction**  
  


⚡ Step 1: Clone the Repository
Clone the repository to your local machine:
   git clone https://github.com/smartdusttechnologies/Trainings.git

   
   📘 For ASP.NET Core Application
   Install .NET SDK:
   Ensure you have the .NET SDK installed on your system. Download it from the official site:
   👉 Download .NET SDK ( https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

   Open the Project:
   Open your project in either Visual Studio or Visual Studio Code.

   Restore NuGet Packages:
   Navigate to the project root folder and run:
   --> dotnet restore
   Configure Flask Service:
   Before running your ASP.NET project, ensure that the Flask server is running on:
   🌐 http://127.0.0.1:5000
 
 3. For Python(Endpoint)
  
  A. Make virtual environment 
 # Create a virtual environment
    python -m venv venv

 # Activate the virtual environment
    # Windows
    venv\Scripts\activate

    # macOS/Linux
    source venv/bin/activate
 B. Install the required packages: 

   pip install -r requirements.txt


📚 Usage
✅ 1. Start the Flask Server
   python main.py 
✅ 2. Run the ASP.NET Core Project
   dotnet run
✅ 3. After this in Resume page you can upload the resume and find the job recommendations.

