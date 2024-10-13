import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import Router from "@/router";
import 'primevue/resources/themes/lara-dark-green/theme.css'
import 'primevue/resources/themes/lara-dark-green/fonts/Inter-italic.var.woff2'
import 'primevue/resources/themes/lara-dark-green/fonts/Inter-roman.var.woff2'
import "primeicons/primeicons.css";
import "primeicons/fonts/primeicons.ttf"
import "primeicons/fonts/primeicons.woff"
import "primeicons/fonts/primeicons.woff2"
import "primeflex/primeflex.css"

import 'bootstrap/scss/bootstrap-utilities.scss'
import 'bootstrap/scss/bootstrap-grid.scss'
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

const app = createApp(App)
    .use(PrimeVue, {ripple: true})
    .use(Router);
app.directive('ripple', Ripple);
app.use(pinia);
app.use(ToastService);
app.use(ConfirmationService)

app.mount('#app');
