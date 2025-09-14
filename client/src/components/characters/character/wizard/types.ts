import type {Component} from "vue";

export interface WizardContent {
    headerName: string,
    component: Component
    props?: Record<string, unknown>
}
