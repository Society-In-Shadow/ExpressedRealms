import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import { dirname } from 'node:path';
import fs from 'fs';

const __dirname = dirname(__filename);

export default defineConfig(({ mode }) => {
    
    const port = process.env.VITE_PORT || 443;
    const httpsKey = process.env.VITE_HTTPS_KEY;
    const httpsCert = process.env.VITE_HTTPS_CERT;

    const serverConfig = {
        port: port,
        proxy: {
            '/api': {
                target: process.env.VITE_API_BASE_URL,
                changeOrigin: true,
                rewrite: (path) => path.replace(/^\/api/, ''),
                secure: false
            }
        }
    };

    if (httpsKey && httpsCert) {
        serverConfig.https = {
            key: fs.readFileSync(httpsKey),
            cert: fs.readFileSync(httpsCert)
        };
    }

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': require('path').resolve(__dirname, 'src')
            }
        },
        server: serverConfig
    };
});