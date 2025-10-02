import {boolean, type InferType, number, object} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {ListItem} from "@/types/ListItem";
import type {StatModifierReturnModel} from "@/components/modifiergroups/types.ts";

const validationSchema = object({
    modifierType: object<ListItem>().nullable()
        .required()
        .label("Modifier Type"),
    modifier: number().required()
        .label("Modifier"),
    creationSpecificBonus: boolean()
        .label("Creation Specific Bonus"),
    scaleWithLevel: boolean()
        .label("Scale With Level")
});

export type ModifierForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (model: StatModifierReturnModel) => {
        form.fields.modifier.field.value = model.modifier;
        form.fields.creationSpecificBonus.field.value = model.creationSpecificBonus;
        form.fields.scaleWithLevel.field.value = model.scaleWithLevel;
        form.fields.modifierType.field.value = model.statModifier;
    }
    
    const customResetForm = () => {
        form.fields.modifier.field.value = 0;
        form.fields.creationSpecificBonus.field.value = false;
        form.fields.scaleWithLevel.field.value = false;
        form.fields.modifierType.field.value = null;
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        fields: form.fields,
    }
}
