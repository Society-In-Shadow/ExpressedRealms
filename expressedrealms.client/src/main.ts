import './assets/main.css'

import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import Router from "@/router";
import 'primevue/resources/themes/lara-dark-green/theme.css'

import 'bootstrap/scss/bootstrap-utilities.scss'
import 'bootstrap/scss/bootstrap-grid.scss'
import App from "@/App.vue";

createApp(App)
    .use(PrimeVue)
    .use(Router)
    .mount('#app');

