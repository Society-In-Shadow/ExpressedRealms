import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import fs from 'node:fs'
import { fileURLToPath, URL } from 'node:url'

export default defineConfig(() => {
  // Needs to be above 1024, as any below that will be protected / require administrative rights
  // This is particularly important for Cypress testing
  const port = process.env.VITE_PORT || 3000
  const httpsKey = process.env.VITE_HTTPS_KEY
  const httpsCert = process.env.VITE_HTTPS_CERT

  const serverConfig: Record<string, any> = {
    port,
    proxy: {
      '/api': {
        target: process.env.VITE_API_SERVER_LOCATION,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/api/, ''),
        secure: false,
      },
    },
  }

  if (httpsKey && httpsCert) {
    serverConfig.https = {
      key: fs.readFileSync(httpsKey),
      cert: fs.readFileSync(httpsCert),
    }
  }

  return {
    plugins: [vue()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)), // ✅ works in ESM + Cypress
      },
    },
    server: serverConfig,
  }
})
