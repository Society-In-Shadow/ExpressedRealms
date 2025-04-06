// test/test-setup.ts
import {vi} from "vitest";

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
import { routes } from './src/router/index';
import piniaPluginPersistedState from "pinia-plugin-persistedstate"
import { createPinia } from 'pinia'
import { createRouter, createWebHistory } from 'vue-router'; // Vue Router

const pinia = createPinia();
pinia.use(piniaPluginPersistedState);

const testRouter = createRouter({
    history: createWebHistory(),
    routes
});

// Add plugins globally
config.global.plugins = [PrimeVue, testRouter, pinia];

import { vi } from 'vitest';
beforeAll(() => {
    window.matchMedia = vi.fn().mockImplementation((query) => ({
        matches: false, // Default mock value (can be adjusted per test)
        media: query,
        onchange: null,
        addListener: vi.fn(), // Legacy method
        removeListener: vi.fn(), // Legacy method
        addEventListener: vi.fn(), // Modern method
        removeEventListener: vi.fn(), // Modern method
        dispatchEvent: vi.fn(),
    }));
});

