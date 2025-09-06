import {type InferType, number, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {BlessingLevel} from "@/components/blessings/types.ts";

const validationSchema = object({
    level: string()
        .required()
        .max(250)
        .label("Level"),
    description: string()
        .required()
        .label("Description"),
    xpCost: number()
        .required()
        .label("XP Cost"),
    xpGain: number()
        .required()
        .label("XP Gain"),
});

export type BlessingLevelForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (model: BlessingLevel) => {
        form.fields.level.field.value = model.level;
        form.fields.description.field.value = model.description;
        form.fields.xpCost.field.value = model.xpCost;
        form.fields.xpGain.field.value = model.xpGain;
    }
    
    const customResetForm = () => {
        form.fields.description.field.value = "";
        form.fields.level.field.value = "";
        form.fields.xpCost.field.value = 0;
        form.fields.xpGain.field.value = 0;
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        setErrors: form.setErrors,
        level: form.fields.level,
        description: form.fields.description,
        xpCost: form.fields.xpCost,
        xpGain: form.fields.xpGain,
    }
}
