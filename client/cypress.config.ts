import { defineConfig } from 'cypress';
import { UserConfig } from 'vite';

import ViteConfig from "./vite.config";

const viteConfig = ViteConfig as UserConfig;

if (viteConfig.server) {
  // @see https://github.com/cypress-io/cypress/issues/24564
  // @see https://chat.openai.com/share/f09deddb-821e-450d-b78d-444977b4aeab
  delete viteConfig.server.https;
}

export default defineConfig({
  projectId: "3wpvob",

  e2e: {
    baseUrl: "https://localhost:5173",
  },

  component: {
    devServer: {
      framework: "vue",
      bundler: "vite",
      viteConfig
    },
  },
});
