import './assets/main.css'

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

var app = createApp(App)
    .use(PrimeVue, {ripple: true})
    .use(Router);

app.directive('ripple', Ripple);

app.mount('#app');

