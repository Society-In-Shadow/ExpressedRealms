import {object, string} from "yup";
import {useForm} from "vee-validate";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";
import {computed} from "vue";
import type {EditPower} from "@/components/expressions/powers/types/power";

export function getValidationInstance() {
    
    const validationSchema = object({
        name: string()
            .required()
            .max(250)
            .label("Name"),
        description: string()
            .required()
            .label("Description")
    });
    
    // Destructure `useForm` to define handlers and fields
    const { defineField, handleSubmit, errors, handleReset } = useForm({
        validationSchema: validationSchema,
        validateOnMount: false,
        keepValuesOnUnmount: false
    });
    
    // Define all fields using `defineField`
    function createFormField(fieldName: string): FormField {
        return {
            field: defineField(fieldName)[0],
            error: computed(() => errors.value[fieldName]),
            label: validationSchema.fields[fieldName].spec.label
        };
    }
    
    const name = createFormField("name");
    const description = createFormField("description");
    
    const setValues = (power: EditPower) => {
        name.field.value = power.name;
        description.field.value = power.description;
    }
    
    const customResetForm = () => {
        handleReset();
    };
    
    return {
        handleSubmit, 
        customResetForm,
        setValues,
        name,
        description,
    }
}
