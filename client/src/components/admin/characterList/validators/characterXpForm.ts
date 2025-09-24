import {number, object} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";

const validationSchema = object({
    xp: number()
        .required()
        .label("Available Character XP"),
});

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (xpValue: number) => {
        form.fields.xp.field.value = xpValue;
    }
    
    const customResetForm = () => {
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        setErrors: form.setErrors,
        fields: form.fields,
    }
}
