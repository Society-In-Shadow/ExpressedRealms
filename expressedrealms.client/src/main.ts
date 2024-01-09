import './assets/main.css'

import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import Router from "@/router";
import 'primevue/resources/themes/lara-dark-green/theme.css'
import "primeicons/primeicons.css";
import "primeflex/primeflex.css"

import 'bootstrap/scss/bootstrap-utilities.scss'
import 'bootstrap/scss/bootstrap-grid.scss'
import App from "@/App.vue";
import Ripple from 'primevue/ripple';

var app = createApp(App)
    .use(PrimeVue, {ripple: true})
    .use(Router);

app.directive('ripple', Ripple);

app.mount('#app');

