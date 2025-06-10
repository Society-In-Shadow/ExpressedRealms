import {object, string} from "yup";
import type {EditPowerPath} from "@/components/expressions/powerPaths/types";
import { useGenericForm } from "@/utilities/formUtilities";

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
    
    const form = useGenericForm(validationSchema);
    
    const setValues = (power: EditPowerPath) => {
        form.fields.name.field.value = power.name;
        form.fields.description.field.value = power.description;
    }
    
    const customResetForm = () => {
        form.fields.description.field.value = "";
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        name: form.fields.name,
        description: form.fields.description,
    }
}
