from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.decomposition import NMF
import sys

def extract_topics(text):
    vectorizer = TfidfVectorizer(stop_words='english')
    X = vectorizer.fit_transform([text])

    nmf = NMF(n_components=1, random_state=42)
    nmf.fit(X)
    words = vectorizer.get_feature_names_out()
    
    topics = [words[i] for i in nmf.components_[0].argsort()[:-5:-1]]
    return topics

if __name__ == "__main__":
    input_text = sys.argv[1]
    topics = extract_topics(input_text)
    print(topics)
