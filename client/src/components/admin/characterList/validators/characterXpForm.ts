import {number, object} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";

const validationSchema = object({
    xp: number()
        .required()
        .label("Available Character XP"),
    playerNumber: number()
        .required()
        .label("Player Number"),
});

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (playerNumber: number, xpValue: number) => {
        form.fields.xp.field.value = xpValue;
        form.fields.playerNumber.field.value = playerNumber;
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
