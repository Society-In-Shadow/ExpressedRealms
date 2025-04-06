// test/test-setup.ts
if (!global.ResizeObserver) {
    global.ResizeObserver = class ResizeObserver {
        observe() {
            // Mock the observe method (no-op)
        }
        unobserve() {
            // Mock the unobserve method (no-op)
        }
        disconnect() {
            // Mock the disconnect method (no-op)
        }
    };
}

import { config } from '@vue/test-utils';
import PrimeVue from 'primevue/config';
import router from './src/router/index';
import piniaPluginPersistedState from "pinia-plugin-persistedstate"
import { createPinia } from 'pinia'

const pinia = createPinia();
pinia.use(piniaPluginPersistedState);

// Add plugins globally
config.global.plugins = [PrimeVue, router, pinia];
