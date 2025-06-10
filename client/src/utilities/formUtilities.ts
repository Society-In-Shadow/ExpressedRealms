import { useForm } from "vee-validate";
import type { ObjectSchema } from "yup";
import { computed } from "vue";
import type { FormField } from "@/FormWrappers/Interfaces/FormField";

export function useGenericForm<T extends Record<string, any>>(validationSchema: ObjectSchema<T>) {
    const { defineField, handleSubmit, errors, handleReset } = useForm<T>({
        validationSchema,
        validateOnMount: false,
        keepValuesOnUnmount: false,
    });

    function createFormField<K extends keyof T>(fieldName: K): FormField {
        return {
            field: defineField(fieldName)[0],
            error: computed(() => errors.value[fieldName]),
            label: (validationSchema.fields[fieldName as string] as any)?.spec?.label ?? fieldName,
        };
    }

    // Generate all form fields dynamically
    const fields = {} as Record<keyof T, FormField>;
    Object.keys(validationSchema.fields).forEach((key) => {
        fields[key as keyof T] = createFormField(key as keyof T);
    })

    return {
        fields,
        handleSubmit,
        handleReset,
        errors,
    };
}
