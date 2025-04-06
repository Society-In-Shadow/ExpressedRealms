import { defineConfig } from '@playwright/experimental-ct-vue';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
    testDir: './playwright', // Directory containing your tests
    use: {
        ctPort: 3100, // Port for the component testing server
        ctViteConfig: './playwrite.vite.config.ts'
    },
    projects: [
        {
            name: 'chromium',
            use: { browserName: 'chromium' },
        },
        {
            name: 'firefox',
            use: { browserName: 'firefox' },
        }
    ],
});
