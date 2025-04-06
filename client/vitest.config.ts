import { defineConfig } from 'vitest/config';
import vue from '@vitejs/plugin-vue'; // Import the Vue plugin
import path from 'path';

export default defineConfig({
    plugins: [vue()], // Add the Vue plugin
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'), // Define the "@" alias for the "src" folder
        },
    },
    test: {
        globals: true, // Enable global utilities like `vi`
        environment: 'jsdom', // Use JSDOM for DOM emulation
        setupFiles: './test-setup.ts', // Optional: Path to your global test setup file
    },
});
