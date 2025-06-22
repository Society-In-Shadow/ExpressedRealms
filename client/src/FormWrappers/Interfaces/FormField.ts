import type {Ref, ComputedRef} from "vue";

export interface FormField {
    field: Ref<string | object | boolean | null>; // Ref for the input value
    error: ComputedRef<string | undefined>; // ComputedRef for potential error
    label: string; // Label for the field
    isRequired: boolean; // dynamically generated from the validations applied to the field
}
