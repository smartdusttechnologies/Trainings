import streamlit as st
import pandas as pd
import matplotlib.pyplot as plt
import joblib
import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer
import PyPDF2
import spacy
import os
import shutil
from sklearn.metrics.pairwise import cosine_similarity
from collections import Counter

svc = joblib.load('svc_model.pkl')
word_vectorizer = joblib.load('tfidf_vectorizer.pkl')
label = joblib.load('label_encoder.pkl')


nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')
nlp = spacy.load('en_core_web_sm')



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

def predict_category_for_resumes(resumes):
    preprocessed_resumes = [preprocess_text(resume) for resume in resumes]
    input_resumes = word_vectorizer.transform(preprocessed_resumes)
    
    predictions = svc.predict(input_resumes)
    predicted_categories = label.inverse_transform(predictions)
    
    return predicted_categories  

def extract_name(resume_text):
    doc = nlp(resume_text)
    for ent in doc.ents:
        if ent.label_ == "PERSON":
            return ent.text
    return "Unknown"

def save_resumes_to_folders(df_results, upload_files):
    base_directory = "Resume"
    
    
    if not os.path.exists(base_directory):
        os.makedirs(base_directory)
    
    
    for idx, row in df_results.iterrows():
        category = row['Predicted Category']
        resume_name = row['Name']
        file_name = row['File']
        
       
        category_folder = os.path.join(base_directory, category)
        if not os.path.exists(category_folder):
            os.makedirs(category_folder)
        
       
        file = upload_files[idx]
        save_path = os.path.join(category_folder, file.name)
        
       
        with open(save_path, "wb") as f:
            f.write(file.getbuffer())
        
       
        print(f"Saved {file.name} to {category_folder}")


# pdf_files = ["Resume/10030015.pdf", "Resume/10344379.pdf", "Resume/10818478.pdf"]  

# resumes = pdfs(pdf_files)

# predicted_categories = predict_category_for_resumes(resumes)


# df_results = pd.DataFrame({
#     "Resume": resumes,
#     "Predicted Category": predicted_categories,
#     "File" : pdf_files
# })



st.title("Resume Classifier")
st.sidebar.header("Resumes")

upload_files = st.sidebar.file_uploader("Upload" , type=["pdf"], accept_multiple_files=True)
if upload_files:
    st.sidebar.text(f"{len(upload_files)} file(s) selected.")
    

    resumes = pdfs(upload_files)
    prediction = predict_category_for_resumes(resumes)
    names = [extract_name(resume) for resume in resumes]
   

    df_results = pd.DataFrame({
    "Name": names,
    "Predicted Category": prediction,
    "File": [file.name for file in upload_files] ,
    "Resume Text" : resumes
    })
    st.dataframe(df_results)

    save_resumes_to_folders(df_results, upload_files)
    categories = df_results["Predicted Category"].unique()
    selected_category = st.selectbox("Select a Category", categories)
    # Count frequencies of job titles
    job_title_counts = Counter(job_titles)
    top_5_titles = [title for title, _ in job_title_counts.most_common(5)]
    
    # Dropdown for selecting job titles
    selected_title = st.selectbox(
        "Select a Job Title", top_5_titles, help="Choose from the most frequent job titles"
    )

    if selected_title:
        # Filter resumes by selected job title
        filtered_resumes = df_results[df_results["Predicted Job Title"] == selected_title]
        st.subheader(f"Resumes for Job Title: {selected_title}")
        st.write(f"Total Resumes: {len(filtered_resumes)}")
        st.dataframe(filtered_resumes[["File Name", "Predicted Job Title"]])


# if selected_category:
#         filtered_resumes = df_results[df_results["Predicted Category"] == selected_category]
        
       
#         role_vector = word_vectorizer.transform([selected_category])
        
       
#         preprocessed_texts = [preprocess_text(text) for text in filtered_resumes["Resume Text"]]
#         tfidf_matrix = word_vectorizer.transform(preprocessed_texts)
        
       
#         similarity_scores = cosine_similarity(role_vector, tfidf_matrix).flatten()
        
        
#         filtered_resumes = filtered_resumes.assign(Similarity=similarity_scores)
#         top_resumes = filtered_resumes.sort_values(by="Similarity", ascending=False).head(5)
        
#         st.subheader(f"Top 5 Resumes Similar to Role: {selected_category}")
#         for idx, row in top_resumes.iterrows():
#             st.write(f"**Name**: {row['Name']}")
#             st.write(f"**File**: {row['File']}")
#             st.write(f"**Similarity Score with Role**: {row['Similarity']:.2f}")
#             st.write(f"**Resume Text Preview**: {row['Resume Text'][:500]}...")
#             st.write("---")    

    
    
# resume_text = "john smith phone 123 email linkedin github professional summary experienced project manager 7 year expertise leading team successfully deliver complex project time within budget adept stakeholder management risk mitigation process optimization strong background agile waterfall methodology proven track record managing multiple project simultaneously fostering collaborative productive team environment excellent communication skill passion driving organizational success strategic project execution technical skill project management tool jira trello asana microsoft project basecamp methodology agile scrum kanban waterfall lean prince2 collaboration tool slack microsoft team zoom google workspace software m office suite google sheet confluence visio risk management risk assessment risk mitigation contingency planning budget management cost estimation financial planning budget control stakeholder management client communication vendor coordination team leadership professional experience project manager tech innovator llc new york ny june 2020 present managed project lifecycles software development infrastructure project valued 10 million led team member including developer designer qa engineer ensuring alignment project goal timeline established managed project scope schedule resource ensuring project completed time within budget collaborated senior leadership define project objective deliverable coordinated regular project status meeting stakeholder client track progress resolve issue promptly implemented agile scrum methodology resulting 30 increase team efficiency created maintained detailed project documentation including risk management plan project charter review improved project delivery time 25 process improvement optimization initiative provided mentorship training junior project manager team member project coordinator global solution san francisco ca january 2017 may 2020 assisted planning execution complex project working closely project manager team lead managed project schedule tracked milestone updated stakeholder progress coordinated resource allocation ensured timely delivery project task developed maintained project report schedule key documentation leadership client monitored project risk including scope creep resource bottleneck escalated issue necessary led development project management knowledge base improving team collaboration efficiency project assistant nextgen technology remote march 2015 december 2016 supported senior project manager planning organizing executing various technical project assisted budgeting cost control activity ensure project stayed within financial limit facilitated communication internal team external stakeholder ensuring project requirement met managed project documentation tracked project progress organized meeting handled procurement activity including vendor management contract negotiation contributed development project timeline ensuring deadline met deliverable schedule education bachelor science business administration project management focus university california berkeley berkeley ca graduated may 2014 certification project management professional pmp project management institute pmi completed march 2017 certified scrummaster csm scrum alliance completed september 2018 agile certified practitioner project management institute pmi completed june 2020 prince2 foundation certification axelos completed december 2016 project cloud migration project role project manager technology aws microsoft azure managed migration major client infrastructure cloud resulting 40 reduction cost improved scalability coordinated across team ensure smooth migration zero downtime transition enterprise software development role project manager technology java rest apis led development software suite financial client including stakeholder management scope control timeline adherence delivered project 15 budget 20 ahead schedule additional information language english fluent spanish intermediate hobby playing chess volunteering local blogging project management best practice reference available upon request'"
# doc = nlp(resume_text) 
# for ent in doc.ents:
#         if ent.label_ == "PERSON":
#             print( ent.text)