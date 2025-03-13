import speech_recognition as sr
import librosa
import soundfile as sf
import os
import sys
import json
from transformers import pipeline

# FFmpeg yollarını tanımla
FFMPEG_PATH = r"C:\ffempeg\ffmpeg-2025-03-10-git-87e5da9067-full_build\ffmpeg-2025-03-10-git-87e5da9067-full_build\bin\ffmpeg.exe"
FFPROBE_PATH = r"C:\ffempeg\ffmpeg-2025-03-10-git-87e5da9067-full_build\ffmpeg-2025-03-10-git-87e5da9067-full_build\bin\ffprobe.exe"

def convert_audio(input_path, output_path, target_sr=16000):
    """Ses dosyasını 16kHz, mono, 16-bit PCM WAV formatına çevirir."""
    y, sr = librosa.load(input_path, sr=None)  # Orijinal sample rate ile yükle
    y = librosa.resample(y, orig_sr=sr, target_sr=target_sr)  # 16kHz'e çevir

    # Mono'ya çevir
    if len(y.shape) > 1:
        y = librosa.to_mono(y)

    # 16-bit PCM WAV olarak kaydet
    sf.write(output_path, y, target_sr, subtype='PCM_16')

def convert_to_wav(file_path):
    """FFmpeg ile ses dosyasını WAV formatına dönüştürür."""
    wav_path = file_path.rsplit(".", 1)[0] + ".wav"
    command = f'"{FFMPEG_PATH}" -i "{file_path}" -ar 16000 -ac 1 -c:a pcm_s16le "{wav_path}"'
    os.system(command)
    return wav_path

def recognize_speech(file_path):
    """Ses dosyasını metne çevirir."""
    recognizer = sr.Recognizer()

    # WAV değilse dönüştür
    if not file_path.endswith(".wav"):
        file_path = convert_to_wav(file_path)

    with sr.AudioFile(file_path) as source:
        audio = recognizer.record(source)

    try:
        text = recognizer.recognize_google(audio, language="tr-TR")
        return text
    except sr.UnknownValueError:
        return "Ses anlaşılamadı."
    except sr.RequestError:
        return "Google API'ye ulaşılamadı."

def extract_topic_using_bert(text):
    """BERT ile konu analizi yapar."""
    topic_pipeline = pipeline("zero-shot-classification", model="bert-base-uncased")
    candidate_labels = ["eğitim", "üniversite", "sınav", "proje", "ders", "aile", "ev", "yurtta yaşam"]
    result = topic_pipeline(text, candidate_labels)
    return result['labels'][0]

if __name__ == "__main__":
    file_path = sys.argv[1]

    try:
        # 1️⃣ Ses dosyasını modele uygun hale getir (16kHz, mono, PCM_16)
        converted_path = file_path.rsplit(".", 1)[0] + "_converted.wav"
        convert_audio(file_path, converted_path)

        # 2️⃣ Speech-to-text işlemi
        text = recognize_speech(converted_path)

        # 3️⃣ Metin yoksa boş JSON döndür
        if text in ["Ses anlaşılamadı.", "Google API'ye ulaşılamadı."]:
            output = {
                "text": text,
                "wordCount": 0,
                "topic": "Konu bulunamadı."
            }
        else:
            # 4️⃣ Konu analizi
            word_count = len(text.split())
            topic = extract_topic_using_bert(text)

            output = {
                "text": text,
                "wordCount": word_count,
                "topic": topic
            }

        # 5️⃣ JSON olarak sonucu yazdır
        print(json.dumps(output, ensure_ascii=False))

    except Exception as e:
        error_output = {"error": str(e)}
        print(json.dumps(error_output, ensure_ascii=False))
