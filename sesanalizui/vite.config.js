import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 3000, // Sabit port numarasýný belirleyin
        proxy: {
            '/api': {
                target: 'http://localhost:5169', // Backend'inizin portu
                changeOrigin: true,
                secure: false,
                rewrite: (path) => path.replace(/^\/api/, ''), // API prefix'i çýkarýyoruz
            },
        },
    },
});
