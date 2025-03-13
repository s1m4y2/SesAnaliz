🎙 Ses Analiz ve Konu Tanıma API'si
Bu proje, ses dosyalarını analiz ederek konuşma metnine dönüştüren, kelime sayısını hesaplayan ve BERT modelini kullanarak konusunu tahmin eden bir Ses Analiz API'sidir.

🚀 Proje Özellikleri
🎤 Canlı ses kaydı alıp analiz edebilme
📁 Ses dosyalarını işleyerek metin çıkarımı yapma
📌 Konuşma metninden konu tahmini
📊 Makine öğrenimi ve derin öğrenme modelleri kullanarak veri analizi
🌐 Web arayüzü ile kullanıcı dostu deneyim (React.js)
🛠 Kullanılan Teknolojiler
Alan	Teknolojiler
Backend API	ASP.NET Core 9.0
Frontend UI	React.js, Material UI, TypeScript
Veri İşleme	Python, SpeechRecognition, Librosa, Transformers (BERT)
Model Eğitimi	TensorFlow, Scikit-Learn, NMF, TfidfVectorizer
Veri Tabanı	MSSQL veya MySQL
Ses İşleme	FFmpeg, Google Speech-to-Text API
Testler	Selenium, NUnit, XUnit
📂 Proje Yapısı
📦 SesAnalizProjesi
├── 📁 Backend
│   ├── 📁 Controllers
│   │   ├── AnalysisController.cs  # API uç noktaları
│   │   ├── AudioController.cs  
│   │   ├── UserController.cs  
│   ├── 📁 PythonScripts
│   │   ├── analyze_audio.py       # Ses işleme betiği
│   │   ├── sentiment_analysis.py      
│   │   ├── speech_to_text.py       
│   │   ├── topic_modelling.py   
│   │   ├── train_voice_model.py       
│   ├── Program.cs                 # ASP.NET Core uygulama başlangıcı
│   ├── appsettings.json           # API yapılandırma dosyası
│
├── 📁 Frontend
│   ├── src/
│   │   ├── App.tsx                # Ana React bileşeni
│   │   ├── styles.css              # UI stilleri
│
└── README.md
📦 Kurulum ve Çalıştırma
1️⃣ Backend (ASP.NET Core API)
cd Backend
dotnet restore
dotnet run
2️⃣ Python Modüllerini Yükleyin
pip install -r requirements.txt
3️⃣ Frontend (React.js)
cd Frontend
npm install
npm start
🎯 API Kullanımı
🎤 Canlı Ses Kaydı İşleme
POST /api/analysis/process-live-audio
Content-Type: multipart/form-data
Body: { file: ses_dosyası.wav }
✅ Yanıt:
{
  "message": "Analiz tamamlandı!",
  "text": "Merhaba dünya",
  "wordCount": 2,
  "topic": "Genel"
}
📁 Yüklenen Ses Dosyası İşleme
POST /api/analysis/process-audio
Content-Type: multipart/form-data
Body: { file: ses_dosyası.mp3 }
✅ Yanıt:
{
  "message": "Analiz tamamlandı!",
  "text": "Python öğreniyorum",
  "wordCount": 2,
  "topic": "Eğitim"
}
🛠 Geliştirme ve Katkıda Bulunma
Eğer projeye katkıda bulunmak isterseniz:

Fork edin 🍴
Yeni bir branch oluşturun 🌱
Değişiklikleri yapıp commit atın 💾
Pull request gönderin 🚀
