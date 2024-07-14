import { defineConfig } from 'cypress';
import { UserConfig } from 'vite';

import ViteConfig from "./vite.config";

const viteConfig = ViteConfig as UserConfig;

export default defineConfig({
  projectId: "3wpvob",

  e2e: {
    baseUrl: "https://172.19.0.6",
  },

  component: {
    devServer: {
      framework: "vue",
      bundler: "vite",
      viteConfig
    }
  },
});
