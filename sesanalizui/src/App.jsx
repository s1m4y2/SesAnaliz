import React, { useState } from "react";
import { Container, Typography, Button, Box, TextField, CircularProgress } from "@mui/material";
import axios from "axios";

function App() {
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState("");
    const [recording, setRecording] = useState(false);
    const [mediaRecorder, setMediaRecorder] = useState(null);

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        if (!file) return alert("Lütfen bir ses dosyası seçin.");

        setLoading(true);
        const formData = new FormData();
        formData.append("file", file);

        try {
            const response = await axios.post("http://localhost:5169/api/analysis/process-audio", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });
            setResult(JSON.stringify(response.data, null, 2));
        } catch (error) {
            console.error("Hata:", error);
            setResult("Analiz sırasında hata oluştu.");
        }
        setLoading(false);
    };

    const startRecording = () => {
        navigator.mediaDevices.getUserMedia({ audio: true })
            .then(stream => {
                const options = { mimeType: "audio/webm;codecs=opus" };
                const recorder = new MediaRecorder(stream, options);
                setMediaRecorder(recorder);

                let localAudioChunks = []; // 🔥 Burada local değişken tutuyoruz!

                recorder.ondataavailable = (event) => {
                    console.log("🎤 Yeni ses parçası geldi:", event.data);
                    localAudioChunks.push(event.data); // ✅ Doğrudan diziye ekle
                };

                recorder.start();
                setRecording(true);

                recorder.onstop = async () => {
                    console.log("📌 Kaydedilen parçalar:", localAudioChunks);

                    if (localAudioChunks.length === 0) {
                        console.error("❌ Ses kaydı alınamadı!");
                        setResult("Ses kaydı alınamadı!");
                        return;
                    }

                    const audioBlob = new Blob(localAudioChunks, { type: "audio/webm" });
                    console.log("✅ Blob oluşturuldu:", audioBlob);

                    await handleLiveAudioUpload(audioBlob);
                };
            })
            .catch(error => console.error("❌ Mikrofona erişim hatası:", error));
    };




    const stopRecording = () => {
        if (mediaRecorder) {
            mediaRecorder.stop();
            setRecording(false);
        }
    };

    const handleLiveAudioUpload = async (audioBlob) => {
        setLoading(true);

        // 📌 Blob'u File nesnesine çeviriyoruz
        const audioFile = new File([audioBlob], "live_audio.wav", { type: "audio/wav" });

        const formData = new FormData();
        formData.append("file", audioFile);

        // ✅ Backend'e gitmeden önce kontrol et
        console.log("Gönderilen FormData içeriği:");
        for (let pair of formData.entries()) {
            console.log(pair[0], pair[1]);
        }

        try {
            const response = await axios.post("http://localhost:5169/api/analysis/process-live-audio", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });

            setResult(JSON.stringify(response.data, null, 2));
        } catch (error) {
            console.error("Hata:", error);
            console.error("Hata Detayları:", error.response?.data); // Hata detaylarını logla
            setResult("Anlık analiz sırasında hata oluştu.");
        }
        setLoading(false);
    };



    return (
        <Container maxWidth="sm" sx={{ height: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <Box
                display="flex"
                flexDirection="column"
                justifyContent="center"
                alignItems="center"
                textAlign="center"
                sx={{ width: '100%', maxWidth: '600px' }} // maxWidth ile kutuyu sınırlıyoruz
            >
                <Typography variant="h4" gutterBottom>Ses Analiz Uygulaması 🎤</Typography>

                <input type="file" accept="audio/*" onChange={handleFileChange} style={{ marginBottom: "20px", fontSize: "1rem" }} />

                <Button variant="contained" color="primary" onClick={handleUpload} sx={{ mt: 2, width: "100%", maxWidth: "200px", height: "50px" }}>
                    Yükle ve Analiz Et
                </Button>

                <Typography variant="h6" sx={{ mt: 4 }}>Anlık Ses Kaydı</Typography>

                <Button variant="contained" color={recording ? "secondary" : "success"} onClick={recording ? stopRecording : startRecording} sx={{ mt: 2 }}>
                    {recording ? "Kaydı Durdur" : "Kaydı Başlat"}
                </Button>

                {loading && <CircularProgress sx={{ mt: 2 }} />}

                {result && (
                    <Box mt={4}>
                        <Typography variant="h6">Sonuç:</Typography>
                        <TextField
                            fullWidth
                            multiline
                            rows={4}
                            value={result}
                            disabled
                            sx={{ backgroundColor: 'white' }} // Arka planı beyaz yapıyoruz
                        />
                    </Box>

                )}
            </Box>
        </Container>


    );
}

export default App;
