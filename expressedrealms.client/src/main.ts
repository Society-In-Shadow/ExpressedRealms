import './assets/main.css'

import { createApp } from 'vue'
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";
import PrimeVue from 'primevue/config';

import 'primevue/resources/themes/lara-dark-green/theme.css'

import 'bootstrap/scss/bootstrap-utilities.scss'
import 'bootstrap/scss/bootstrap-grid.scss'

createApp(LoginBasePlate)
    .use(PrimeVue)
    .mount('#app');

