from transformers import pipeline
import sys

def extract_topic_using_bert(text):
    # Zero-shot classification pipeline'ını kullanarak metni daha doğru analiz etme
    topic_pipeline = pipeline("zero-shot-classification")
    
    # Konu etiketleri, eğitim, sınav, üniversite gibi etiketler daha uygun olabilir
    candidate_labels = ["eğitim", "üniversite", "sınav", "proje", "ders", "aile", "ev", "yurtta yaşam"]
    
    # Konu tahmini
    result = topic_pipeline(text, candidate_labels)
    
    # En yüksek skoru alan konuyu döndür
    return result['labels'][0]

if __name__ == "__main__":
    input_text = sys.argv[1]
    topic = extract_topic_using_bert(input_text)
    print(f"Konular: {topic}")
