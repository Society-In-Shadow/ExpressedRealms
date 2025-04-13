import type {Ref, ComputedRef} from "vue";

export interface FormField {
    field: Ref<string>; // Ref for the input value
    error: ComputedRef<string | undefined>; // ComputedRef for potential error
    label: string; // Label for the field
}
