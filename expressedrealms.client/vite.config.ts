import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import { dirname } from 'node:path';

// @see https://chat.openai.com/share/f09deddb-821e-450d-b78d-444977b4aeab

const __dirname = dirname(__filename);

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': require('path').resolve(__dirname, 'src')
        }
    },
    server: {
        proxy: {
            '/api': {
                target: 'https://expressed-realms-web-api:5001/',
                changeOrigin: true,
                rewrite: (path) => path.replace(/^\/api/, ''),
                secure: false
            },
        },
        port: 5173,
        https: {
            key: require('path').resolve(__dirname, `/https/key.pem`),
            cert: require('path').resolve(__dirname, `/https/cert.pem`)
        }
    }
})
