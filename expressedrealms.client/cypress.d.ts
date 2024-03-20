import { mount } from "cypress/vue";
import { Router } from 'vue-router'

type MountParams = Parameters<typeof mount>
type OptionsParam = MountParams[1] & { router?: Router, pushRoute?: string }
// Augment the Cypress namespace to include type definitions for
// your custom command.
// Alternatively, can be defined in cypress/support/component.d.ts
// with a <reference path="./component" /> at the top of your spec.
declare global {
    namespace Cypress {
        interface Chainable {
            mount(component: any, options?: OptionsParam): Chainable<any>
            /**
             * Custom command to select DOM element by data-cy attribute.
             * @example cy.dataCy('greeting')
             */
            dataCy(value: string): Chainable<JQuery<HTMLElement>>;
        }
    }
}
