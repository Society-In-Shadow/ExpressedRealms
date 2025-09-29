import {number, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {ProgressionLevel} from "@/components/expressions/progressionLevels/types.ts";

export function getValidationInstance() {
    
    const validationSchema = object({
        xlLevel: number()
            .required()
            .min(1)
            .label("XL Level"),
        description: string()
            .required()
            .max(5000)
            .label("Description")
    });
    
    const form = useGenericForm(validationSchema);
    
    const setValues = (power: ProgressionLevel) => {
        form.fields.xlLevel.field.value = power.xlLevel;
        form.fields.description.field.value = power.description;
    }
    
    const customResetForm = () => {
        form.fields.xlLevel.field.value = 1;
        form.fields.description.field.value = "";
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        fields: form.fields
    }
}
