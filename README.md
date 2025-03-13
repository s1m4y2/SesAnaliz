# 🎙 Ses Analiz ve Konu Tanıma API'si  

Bu proje, ses dosyalarını analiz ederek konuşma metnine dönüştüren, kelime sayısını hesaplayan ve BERT modelini kullanarak konusunu tahmin eden bir **Ses Analiz API'sidir**.  

---

## 🚀 Proje Özellikleri  

✔️ **Canlı ses kaydı alıp analiz edebilme**  
✔️ **Ses dosyalarını işleyerek metin çıkarımı yapma**  
✔️ **Konuşma metninden konu tahmini**  
✔️ **Makine öğrenimi ve derin öğrenme modelleri ile analiz**  
✔️ **Web arayüzü ile kullanıcı dostu deneyim (React.js)**  

---

## 🛠 Kullanılan Teknolojiler  

| **Alan** | **Teknolojiler** |
|----------|----------------|
| **Backend API** | ASP.NET Core 9.0 |
| **Frontend UI** | React.js, Material UI, TypeScript |
| **Veri İşleme** | Python, SpeechRecognition, Librosa, Transformers (BERT) |
| **Model Eğitimi** | TensorFlow, Scikit-Learn, NMF, TfidfVectorizer |
| **Veritabanı** | MSSQL veya MySQL |
| **Ses İşleme** | FFmpeg, Google Speech-to-Text API |
| **Testler** | Selenium, NUnit, XUnit |

---

## 📦 Proje Dizini  

```bash
SesAnalizProjesi
├── 📁 Backend
│   ├── 📁 Controllers
│   │   ├── AnalysisController.cs   # API uç noktaları
│   │   ├── AudioController.cs
│   │   ├── UserController.cs
│   ├── 📁 PythonScripts
│   │   ├── analyze_audio.py         # Ses işleme betiği
│   │   ├── sentiment_analysis.py
│   │   ├── speech_to_text.py
│   │   ├── topic_modelling.py
│   │   ├── train_voice_model.py
│   ├── Program.cs                    # ASP.NET Core başlangıcı
│   ├── appsettings.json               # API yapılandırma dosyası
├── 📁 Frontend
│   ├── src/
│   │   ├── App.tsx                    # Ana React bileşeni
│   │   ├── styles.css                  # UI stilleri
```
---

## 📦 Kurulum ve Çalıştırma  

### 🔹 Backend (ASP.NET Core API)  
```sh
cd Backend
dotnet restore
dotnet run
```
### 🔹 Python Modüllerini Yükleyin
```sh
pip install -r requirements.txt
```
### 🔹 Frontend (React.js)
```sh
cd Frontend
npm install
npm start
```

## 🎯 API Kullanımı

### 🎤 Canlı Ses Kaydı İşleme
```sh
POST /api/analysis/process-live-audio
Content-Type: multipart/form-data
Body: { file: ses_dosyası.wav }
```
### ✅ Yanıt:
```sh
{
    "message": "Analiz tamamlandı!",
    "text": "Merhaba dünya",
    "wordCount": 2,
    "topic": "Genel"
}
```
### 📁 Yüklenen Ses Dosyası İşleme
```sh
POST /api/analysis/process-audio
Content-Type: multipart/form-data
Body: { file: ses_dosyası.mp3 }
```
### ✅ Yanıt:
```sh
{
    "message": "Analiz tamamlandı!",
    "text": "Python öğreniyorum",
    "wordCount": 2,
    "topic": "Eğitim"
}
```
 ## 🛠 Geliştirme ve Katkıda Bulunma

1️⃣ **Projeyi fork edin** 🍴  
2️⃣ **Yeni bir branch oluşturun** 🌱  
3️⃣ **Değişiklikleri yapıp commit atın** 💾  
4️⃣ **Pull request gönderin** 🚀 




⭐ **Projeyi beğendiyseniz yıldız bırakmayı unutmayın!**











