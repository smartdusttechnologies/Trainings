from flask import Flask, request, jsonify
import pickle
import pandas as pd
from nltk.sentiment.vader import SentimentIntensityAnalyzer
from sklearn.linear_model import Lasso,Ridge
from flask_cors import CORS
import numpy as np
import flask
import flask_cors
import sklearn
import pandas
import nltk
import joblib
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer
from tensorflow.keras.models import load_model
import PyPDF2
import spacy
import os
from sklearn.metrics.pairwise import cosine_similarity
from tensorflow.keras.preprocessing.sequence import pad_sequences
import zipfile
import tempfile

nltk.download('vader_lexicon', force=True)

nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')


# print("Flask:", flask.__version__)
# print("Flask-Cors:", flask_cors.__version__)
# print("scikit-learn:", sklearn.__version__)
# print("Pandas:", pandas.__version__)
# print("NLTK:", nltk.__version__)
app = Flask(__name__)
CORS(app)

svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')
# model = load_model('resume_model.h5')
# model = load_model('resume_model2.h5')
model = load_model('resume_model3.h5')

with open('resume_tokenizer.pkl', 'rb') as tokenizer_file:
    tokenizer = joblib.load(tokenizer_file)
max_length = 5000
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')
nlp = spacy.load('en_core_web_sm')

# Iris Flower Models
with open('irsi_data.pkl', 'rb') as file:
    model_flower = pickle.load(file)

with open('scaler.pkl', 'rb') as file:
    scaler_flower = pickle.load(file)
# Salary Models 
with open('salary_model_DTC.pkl', 'rb') as file:
    dt_model = pickle.load(file)

with open('lasso_model.pkl', 'rb') as file:
    lasso_model = pickle.load(file)

with open('ridge_model.pkl', 'rb') as file:
    ridge_model = pickle.load(file)
#  Sentiment Analysis
sid = SentimentIntensityAnalyzer()
#  Spam Detections 
model_text= joblib.load('text_classify.pkl')


def preprocess_text(text):
    stop_words = set(stopwords.words('english'))
    tokens = word_tokenize(text.lower())
    lemmatizer = WordNetLemmatizer()
    return " ".join(
        lemmatizer.lemmatize(word)
        for word in tokens
        if word.isalnum() and word not in stop_words
    )


# def pdf(file):
#     # with open(path,'rb') as file:        
#         pdf_reader = PyPDF2.PdfReader(file)
#         text = ""
#         for page in pdf_reader.pages:
#                 text += page.extract_text()
#         return text

def pdf(file):
    try:
        pdf_reader = PyPDF2.PdfReader(file)
        text = ""
        for page in pdf_reader.pages:
            text += page.extract_text() or ""
        return text
    except PyPDF2.errors.PdfReadError as e:
        print(f"Error reading PDF file: {file.name} - {e}")
        return ""  
def pdfs(pdf_files):
    resume_texts = []
    for pdf_file in pdf_files:        
        resume_text = pdf(pdf_file)
        resume_texts.append(resume_text) 
    return resume_texts  

def extract_name(resume_text):
    doc = nlp(resume_text)
    for ent in doc.ents:
        if ent.label_ == "PERSON":
            return ent.text
    return "Unknown"

def predict_category_for_resumes(resumes):   
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_sequences = tokenizer.texts_to_sequences(preprocessed_resumes)
    input_padded = pad_sequences(input_sequences, maxlen=max_length)
    predictions = model.predict(input_padded)
    predicted_classes = np.argmax(predictions, axis=1)
    predicted_categories = label.inverse_transform(predicted_classes)
    confidence_scores = np.max(predictions, axis=1) * 100 
    return predicted_categories, confidence_scores


def calculate_similarity(resume_text, target_role):
    preprocessed_resume = preprocess_text(resume_text)
    preprocessed_role = preprocess_text(target_role)
    
    resume_vector = word_vectorizer.transform([preprocessed_resume])
    role_vector = word_vectorizer.transform([preprocessed_role])
    
    similarity_score = cosine_similarity(resume_vector, role_vector).flatten()[0]
    return similarity_score

def get_confidence_score(resume_text):   
    preprocessed_resumes = preprocess_text(resume_text) 
    input_sequence = tokenizer.texts_to_sequences([preprocessed_resumes])
    input_padded = pad_sequences(input_sequence, maxlen=max_length)
    predictions = model.predict(input_padded)
    confidence_score = np.max(predictions, axis=1)[0] 
    return float(confidence_score)


# Route for prediction of Iris Flower
@app.route('/flowers', methods=['POST'])
def predict():
    data = request.get_json()
    
  
    inp = pd.DataFrame(data, index=[0])
    
   
    scaled_dt = scaler_flower.transform(inp)
    
   
    prediction = model_flower.predict(scaled_dt)
    
    return jsonify({'species': prediction.tolist()})

# Route for sentiment analysis
@app.route('/sentiments', methods=['POST'])
def sentiment_analysis():
    data = request.get_json()
    
   
    review_text = data.get("review")
    if not review_text:
        return jsonify({"error": "No review text provided"}), 400
    
    
    sentiment_scores = sid.polarity_scores(review_text)
    
    return jsonify(sentiment_scores)

# Route for Salary Prediction
@app.route('/salary', methods=['POST'])
def salary_prediction():
     
    data = request.get_json()

   
    if 'YearsExperience' not in data or 'Age' not in data:
        return jsonify({"error": "Missing required fields: 'YearsExperience' and 'Age'"}), 400

   
    years_experience = data['YearsExperience']
    age = data['Age']

    
    input_data = np.array([[years_experience, age]])

   
    # dt_prediction = dt_model.predict(input_data)[0]
    lasso_prediction = lasso_model.predict(input_data)[0]
    # ridge_prediction = ridge_model.predict(input_data)[0]

 
    # response = {
    #     "DecisionTreePrediction": dt_prediction,
    #     "LassoPrediction": lasso_prediction,
    #     "RidgePrediction": ridge_prediction
    # }

    return jsonify({'Salary': lasso_prediction.tolist()})
    # return jsonify(response)


#  Route for Spam Detections 
@app.route('/spam-detection', methods=['POST'])
def text(): 
        
        data = request.get_json()
        if not data or 'message' not in data:
            return jsonify({"error": "Invalid input, please provide a 'message' field."}), 400
        
       
        message = data['message']        
       
        prediction = model_text.predict([message])[0]        
       
        return jsonify({"message": message, "prediction": prediction})

# Route for movies review 

@app.route('/predict-resume', methods=['POST'])
def predict_categories():
    with tempfile.TemporaryDirectory() as temp_dir:
        zip_file = request.files['resumes']
        zip_path = os.path.join(temp_dir, zip_file.filename)
        zip_file.save(zip_path)

       
        try:
            with zipfile.ZipFile(zip_path, 'r') as zip_ref:
                zip_ref.extractall(temp_dir)
        except zipfile.BadZipFile:
            return jsonify({"error": "File is not a zip file"}), 400

        prediction_results = []
        for resume_filename in os.listdir(temp_dir):
            resume_path = os.path.join(temp_dir, resume_filename)
            with open(resume_path, 'rb') as resume_file:
                text = pdf(resume_file) 
                name = extract_name(text)  
                predicted_category, confidence_score = predict_category_for_resumes([text])
                prediction_results.append({
                    "resume_name": resume_filename,
                    "predicted_category": str(predicted_category[0]),
                    "name": name,
                    "confidence_score": float(confidence_score[0])
                })

    # return jsonify(prediction_results)
        return jsonify({"predicted_categories": prediction_results})

@app.route('/resume-similarity', methods=['POST'])
def predict_similarity():
    if 'resumes' not in request.files:
        return jsonify({"error": "No resume uploaded"}), 400
    resume_files = request.files.getlist('resumes')

    target_role = request.form.get('target_role')
    if not target_role:
        return jsonify({"error": "Target role is missing"}), 400

    similarity_results = []
    for resume_file in resume_files:
        resume_text = pdf(resume_file)
        similarity_score = calculate_similarity(resume_text, target_role)
        name = extract_name(resume_text)

        similarity_results.append({
            "resume_name": resume_file.filename,
            "name": name,
            "similarity_score": float(similarity_score)
        })

    return jsonify({"similarity_score": similarity_results})
if __name__ == "__main__":
    app.run(debug=True)
