import React, { useState } from "react";
import { Container, Typography, Button, Box, TextField, CircularProgress } from "@mui/material";
import axios from "axios";

function App() {
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState("");

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        if (!file) return alert("Lütfen bir ses dosyası seçin.");

        setLoading(true);
        const formData = new FormData();
        formData.append("file", file);

        try {
            const response = await axios.post("http://localhost:5000/api/analysis/process-audio", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });
            setResult(response.data);
        } catch (error) {
            console.error("Hata:", error);
            setResult("Analiz sırasında hata oluştu.");
        }
        setLoading(false);
    };

    return (
        <Container maxWidth="md">
            <Box textAlign="center" mt={5}>
                <Typography variant="h4" gutterBottom>
                    Ses Analiz Uygulaması 🎤
                </Typography>
                <input type="file" accept="audio/*" onChange={handleFileChange} />
                <Button variant="contained" color="primary" onClick={handleUpload} sx={{ mt: 2 }}>
                    Yükle ve Analiz Et
                </Button>
                {loading && <CircularProgress sx={{ mt: 2 }} />}
                {result && (
                    <Box mt={4}>
                        <Typography variant="h6">Sonuç:</Typography>
                        <TextField fullWidth multiline rows={4} value={result} disabled />
                    </Box>
                )}
            </Box>
        </Container>
    );
}

export default App;
