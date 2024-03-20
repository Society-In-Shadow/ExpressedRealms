// ***********************************************************
// This example support/component.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
import './commands'

// Alternatively you can use CommonJS syntax:
// require('./commands')

import { mount } from 'cypress/vue'
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

import PrimeVue from 'primevue/config';
import { createMemoryHistory, createRouter } from 'vue-router';
import {routes} from "../../src/router";
import Ripple from 'primevue/ripple';
import piniaPluginPersistedState from "pinia-plugin-persistedstate"
import { createPinia } from 'pinia'

const pinia = createPinia();
pinia.use(piniaPluginPersistedState);

Cypress.Commands.add('mount', (component, options = {}) => {
    // Setup options object
    options.global = options.global || {}
    options.global.stubs = options.global.stubs || {}
    options.global.stubs['transition'] = false
    options.global.components = options.global.components || {}
    options.global.plugins = options.global.plugins || []

    // create router if one is not provided
    if (!options.router) {
        options.router = createRouter({
            routes: routes,
            history: createMemoryHistory(),
        })
    }
    
    if(options.pushRoute){
        cy.wrap(options.router.push(options.pushRoute));
    }
    
    /* Add any global plugins */
    options.global.plugins.push({
       install(app) {
         app.use(PrimeVue, {ripple: true})
             .use(options.router);
           app.use(pinia);
           app.directive('ripple', Ripple);
       },
    });

    /* Add any global components */
    // options.global.components['Button'] = Button;

    return mount(component, options)
})


Cypress.Commands.add("dataCy", (selector, ...args) => {
    return cy.get(`[data-cy=${selector}]`, ...args)
})
