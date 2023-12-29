import './assets/main.css'

import { createApp } from 'vue'
import Login from "@/components/Login.vue";
import PrimeVue from 'primevue/config';

import 'primevue/resources/themes/lara-dark-green/theme.css'

import 'bootstrap/scss/bootstrap-utilities.scss'
import 'bootstrap/scss/bootstrap-grid.scss'
import App from "@/App.vue";

const app = createApp(App)
    .use(PrimeVue)
    .mount('#app');

