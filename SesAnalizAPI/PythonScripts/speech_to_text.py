import speech_recognition as sr
import sys

def recognize_speech(file_path):
    recognizer = sr.Recognizer()
    with sr.AudioFile(file_path) as source:
        audio = recognizer.record(source)
    
    try:
        text = recognizer.recognize_google(audio, language="tr-TR")  # Google STT
        return text
    except sr.UnknownValueError:
        return "Ses anlaşılamadı."
    except sr.RequestError:
        return "Google API'ye ulaşılamadı."

if __name__ == "__main__":
    file_path = sys.argv[1]
    result = recognize_speech(file_path)
    print(result)
