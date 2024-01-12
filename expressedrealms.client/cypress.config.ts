import { defineConfig } from 'cypress';

export default defineConfig({
  projectId: "3wpvob",

  e2e: {
    baseUrl: "https://localhost:5173",
  },

  component: {
    devServer: {
      framework: "vue",
      bundler: "vite",
    },
  },
});
