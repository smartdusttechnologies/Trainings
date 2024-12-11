from flask import Flask, request, jsonify
import pickle
import pandas as pd
from sklearn.preprocessing import StandardScaler
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
import PyPDF2
import spacy
import os
import shutil
from sklearn.metrics.pairwise import cosine_similarity
from collections import Counter
from gensim.models import Word2Vec


# print("Flask:", flask.__version__)
# print("Flask-Cors:", flask_cors.__version__)
# print("scikit-learn:", sklearn.__version__)
# print("Pandas:", pandas.__version__)
# print("NLTK:", nltk.__version__)
app = Flask(__name__)
CORS(app)

word_vectorizer = joblib.load('word2vec_model.pkl')
svc_grid = joblib.load('svc_grid_word2vec.pkl')

# print(type(word_vectorizer))
# word_vectorizer = Word2Vec.load('word2vec_model.pkl') #word 2 Vect

# svc = joblib.load('svc_model_confidence.pkl')
# svc = joblib.load('svc_grid.pkl')
svc = joblib.load('svc_grid_word2vec.pkl')
# svc = joblib.load('svc_grid_tfid.pkl')

# word_vectorizer = joblib.load('tfidf_vectorizer.pkl') #tfidf_vectorizer
label = joblib.load('label_encoder.pkl')

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


def pdf(file):
    # with open(path,'rb') as file:        
        pdf_reader = PyPDF2.PdfReader(file)
        text = ""
        for page in pdf_reader.pages:
                text += page.extract_text()
        return text


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
def get_sentence_vector(sentence, word_vectorizer):
    words = sentence.split()
    word_vectors = []
    
    for word in words:
        if word in word_vectorizer.wv:
            word_vectors.append(word_vectorizer.wv[word])

    return np.mean(word_vectors, axis=0) if word_vectors else np.zeros(100)
def predict_category_for_resumes(resumes):
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_resumes = np.array([get_sentence_vector(resume,word_vectorizer) for resume in preprocessed_resumes])
    # input_resumes = word_vectorizer.transform(preprocessed_resumes)
    # name = extract_name(resume_text)
    predictions = svc.predict(input_resumes)
    # predicted_categories = label.inverse_transform(predictions)   
    predicted_categories = label.inverse_transform(predictions).tolist()  

    
    return predicted_categories 

def calculate_similarity(resume_text, target_role):
    preprocessed_resume = preprocess_text(resume_text)
    preprocessed_role = preprocess_text(target_role)
    
    # resume_vector = word_vectorizer.transform([preprocessed_resume])
    # role_vector = word_vectorizer.transform([preprocessed_role])
    resume_vector = get_sentence_vector(preprocessed_resume,word_vectorizer)
    role_vector = get_sentence_vector(preprocessed_role,word_vectorizer)
    similarity_score = cosine_similarity([resume_vector], [role_vector]).flatten()[0]


    # similarity_score = cosine_similarity(resume_vector, role_vector).flatten()[0]
    return similarity_score

def get_confidence_score(resume_text):   
    preprocessed_resumes = preprocess_text(resume_text) 
    resume_vector = get_sentence_vector(resume_text, word_vectorizer)
    probabilities = svc.predict_proba([resume_vector])
    max_prob = probabilities.max(axis=1)[0]  
    return float(max_prob)


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

# Route for Resume Matcher 

@app.route('/predict-resume', methods=['POST'])
def predict_categories():
    if 'resumes' not in request.files:
        return jsonify({"error": "Invalid input, please provide 'resumes' field."}), 400

    resumes_files = request.files.getlist('resumes')
    prediction_results = []

    for resume_file in resumes_files:        
        text = pdf(resume_file)
        name = extract_name(text)
        confidence_score = get_confidence_score(text)

        predicted_category = predict_category_for_resumes([text])[0]

        prediction_results.append({
            "resume_name": resume_file.filename,
            "predicted_category": predicted_category,
            "name": name,
            "confidence_score": confidence_score  
        })

    return jsonify({"predicted_categories": prediction_results})

@app.route('/resume-similarity', methods=['POST'])
def predict_similarity():
    if 'resumes' not in request.files:
        return jsonify({"error": "No resume uploaded"}), 400
    resume_files = request.files.getlist('resumes')

    target_role = request.form.get('target_role')
    if not target_role:
        return jsonify({"error": "Target role is missing"}), 400

    # similarity_results = []
    # for resume_file in resume_files:
    #     resume_text = pdf(resume_file)
    #     similarity_score = calculate_similarity(resume_text, target_role)
    #     name = extract_name(resume_text)

    #     similarity_results.append({
    #         "resume_name": resume_file.filename,
    #         "name": name,
    #         "similarity_score": similarity_score
    #     })
    similarity_results = []
    for resume_file in resume_files:
        resume_text = pdf(resume_file)
        resume_vector = get_sentence_vector(resume_text,word_vectorizer)
        target_role_vector = get_sentence_vector(target_role,word_vectorizer)

        similarity_score = cosine_similarity([resume_vector], [target_role_vector]).flatten()[0]

        name = extract_name(resume_text)

        similarity_results.append({
            "resume_name": resume_file.filename,
            "name": name,
            "similarity_score": similarity_score
        })

    similarity_results = sorted(similarity_results, key=lambda x: x['similarity_score'], reverse=True)
        
   
    top_5_results = similarity_results[:5]

    return jsonify({"similarity_score": top_5_results})
if __name__ == "__main__":
    app.run(debug=True)
