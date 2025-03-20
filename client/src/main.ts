import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import Router from "@/router";
import "primeicons/primeicons.css";
import "primeicons/fonts/primeicons.ttf"
import "primeicons/fonts/primeicons.woff"
import "primeicons/fonts/primeicons.woff2"
import "primeflex/primeflex.css"

import "bootstrap/dist/css/bootstrap-grid.css";
import "bootstrap/dist/css/bootstrap-utilities.css";
import App from "@/App.vue";
import Ripple from 'primevue/ripple';
import piniaPluginPersistedState from "pinia-plugin-persistedstate"
import { createPinia } from 'pinia'
import ToastService from 'primevue/toastservice';
import axiosConfig from "@/config/axiosConfig";
import ConfirmationService from 'primevue/confirmationservice';
axiosConfig.setupErrorHandlingInterceptors();
axiosConfig.setAPIUrl();

const pinia = createPinia();
pinia.use(piniaPluginPersistedState);

import { definePreset } from '@primevue/themes';
import Lara from '@primevue/themes/lara';
import 'bootstrap-icons/font/bootstrap-icons.css';

const MyPreset = definePreset(Lara, {
    semantic: {
        colorScheme: {
            light: {
                root:{
                    background: '#edf1fa'
                },
                surface: {
                    0: '#ffffff',
                    50: '{slate.50}',
                    100: '{slate.100}',
                    200: '{slate.200}',
                    300: '{slate.300}',
                    400: '{slate.400}',
                    500: '{slate.500}',
                    600: '{slate.600}',
                    700: '{slate.700}',
                    800: '{slate.800}',
                    900: '{slate.900}',
                    950: '{slate.950}'
                }
            },
            dark: {
                root:{
                    background: '#121212'
                },
                surface: {
                    0: '#ffffff',
                    50: '{slate.50}',
                    100: '{slate.100}',
                    200: '{slate.200}',
                    300: '{slate.300}',
                    400: '{slate.400}',
                    500: '{slate.500}',
                    600: '{slate.600}',
                    700: '{slate.700}',
                    800: '{slate.800}',
                    900: '{slate.900}',
                    950: '{slate.950}'
                }
            }
        }
    }
});

const app = createApp(App)
    .use(PrimeVue, {
        theme: {
            preset: MyPreset
        }
    })
    .use(Router);
app.directive('ripple', Ripple);
app.use(pinia);
app.use(ToastService);
app.use(ConfirmationService)

app.mount('#app');
