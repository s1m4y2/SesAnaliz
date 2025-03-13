import os
import librosa
import numpy as np
import tensorflow as tf
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import accuracy_score, confusion_matrix
import matplotlib.pyplot as plt

# Ses verilerini yükleyen ve özellik çıkaran fonksiyon
def extract_features(file_path):
    y, sr = librosa.load(file_path, sr=None)
    # MFCC özelliklerini çıkarma
    mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=13)
    mfcc_mean = np.mean(mfcc, axis=1)  # MFCC'nin ortalama değerini alıyoruz
    return mfcc_mean

# Verilerinizi ve etiketlerinizi yükleyin
def load_data(data_dir):
    features = []
    labels = []
    for label in os.listdir(data_dir):
        label_dir = os.path.join(data_dir, label)
        if os.path.isdir(label_dir):
            for filename in os.listdir(label_dir):
                if filename.endswith(".wav"):  # ses dosyalarını sadece .wav uzantılı alıyoruz
                    file_path = os.path.join(label_dir, filename)
                    feature = extract_features(file_path)
                    features.append(feature)
                    labels.append(label)
    return np.array(features), np.array(labels)

# Veriyi yükle
data_dir = "path_to_your_audio_data"  # Ses verilerinin bulunduğu dizin
X, y = load_data(data_dir)

# Etiketleri sayısal değerlere dönüştürme
label_encoder = LabelEncoder()
y_encoded = label_encoder.fit_transform(y)

# Eğitim ve test verilerine ayırma
X_train, X_test, y_train, y_test = train_test_split(X, y_encoded, test_size=0.2, random_state=42)

# Modeli oluşturma (basit bir yapay sinir ağı)
model = tf.keras.Sequential([
    tf.keras.layers.Dense(128, activation='relu', input_shape=(X_train.shape[1],)),
    tf.keras.layers.Dropout(0.2),
    tf.keras.layers.Dense(64, activation='relu'),
    tf.keras.layers.Dense(len(np.unique(y_encoded)), activation='softmax')
])

# Modeli derleme
model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])

# Modeli eğitme
history = model.fit(X_train, y_train, epochs=10, batch_size=32, validation_data=(X_test, y_test))

# Modeli kaydetme
model.save("voice_recognition_model.h5")
print("Model başarıyla kaydedildi!")

# Test verisi üzerinde tahmin yapma
y_pred = model.predict(X_test)
y_pred_classes = np.argmax(y_pred, axis=1)

# Başarıyı değerlendirme
accuracy = accuracy_score(y_test, y_pred_classes)
print(f"Accuracy: {accuracy * 100:.2f}%")

# Karışıklık matrisi
cm = confusion_matrix(y_test, y_pred_classes)
print("Confusion Matrix:")
print(cm)

# Karışıklık matrisini görselleştirme
plt.figure(figsize=(8, 6))
plt.imshow(cm, interpolation='nearest', cmap=plt.cm.Blues)
plt.title('Confusion Matrix')
plt.colorbar()
tick_marks = np.arange(len(label_encoder.classes_))
plt.xticks(tick_marks, label_encoder.classes_, rotation=45)
plt.yticks(tick_marks, label_encoder.classes_)
plt.ylabel('True label')
plt.xlabel('Predicted label')
plt.tight_layout()
plt.show()

