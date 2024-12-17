from flask import Flask, request, jsonify
from nltk.sentiment.vader import SentimentIntensityAnalyzer
from flask_cors import CORS
import pickle


app = Flask(__name__)
CORS(app)


sid = SentimentIntensityAnalyzer()


@app.route('/analysis', methods=['POST'])
def sentiment_analysis():
  
        data = request.get_json()

      
        if "review" not in data:
            return jsonify({"error": "No review text provided"}), 400

        review_text = data["review"]

      
        sentiment_scores = sid.polarity_scores(review_text)

     
        return jsonify(sentiment_scores)
  

if __name__ == "__sentiment__":
    # Run Flask app with debug mode
    app.run(debug=True)
