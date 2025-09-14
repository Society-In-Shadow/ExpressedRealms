import type {Component} from "vue";

export interface WizardContent {
    component: Component
    props?: Record<string, unknown>
}
