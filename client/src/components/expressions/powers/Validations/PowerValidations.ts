import {array, boolean, type InferType, number, object, string} from "yup";
import type {ListItem} from "@/types/ListItem";
import {useGenericForm} from "@/utilities/formUtilities";
import type {EditPower} from "@/components/expressions/powers/types";

const validationSchema = object({
    name: string()
        .required()
        .max(250)
        .label("Name"),
    category: array()
        .of(
            object({
                id: number().required(),
                name: string().required(),
                description: string(),
            })
        )
        .label("Category"),
    description: string()
        .required()
        .label("Description"),
    gameMechanicEffect: string()
        .required()
        .label("Game Mechanic Effect"),
    limitation: string()
        .nullable()
        .label("Limitation"),
    powerDuration: object<ListItem>().nullable()
        .required()
        .label("Power Duration"),
    areaOfEffect: object<ListItem>()
        .nullable()
        .required()
        .label("Area of Effect"),
    powerLevel: object<ListItem>().nullable()
        .required()
        .label("Power Level"),
    powerActivationType: object<ListItem>().nullable()
        .required()
        .label("Power Activation Type"),
    other: string()
        .nullable()
        .label("Other"),
    cost: string()
        .nullable()
        .label("Cost"),
    isPowerUse: boolean()
        .label("Is Power Use")
});

export type PowerFormData = InferType<typeof validationSchema>;
export function getValidationInstance() {
    
    const form = useGenericForm(validationSchema);
    
    const setValues = (power: EditPower) => {
        form.fields.name.field.value = power.name;
        form.fields.category.field.value = power.categories;
        form.fields.description.field.value = power.description;
        form.fields.gameMechanicEffect.field.value = power.gameMechanicEffect;
        form.fields.limitation.field.value = power.limitation;
        form.fields.powerDuration.field.value = power.powerDuration;
        form.fields.areaOfEffect.field.value = power.areaOfEffect;
        form.fields.powerLevel.field.value = power.powerLevel;
        form.fields.powerActivationType.field.value = power.powerActivationType;
        form.fields.other.field.value = power.other;
        form.fields.isPowerUse.field.value = power.isPowerUse;
        form.fields.cost.field.value = power.cost;
    }
    
    const customResetForm = () => {
        form.fields.powerDuration.field.value = null;
        form.fields.areaOfEffect.field.value = null;
        form.fields.powerLevel.field.value = null;
        form.fields.powerActivationType.field.value = null;
        form.fields.category.field.value = []; // TODO: This isn't working for some reason
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        name: form.fields.name,
        category: form.fields.category,
        description: form.fields.description,
        gameMechanicEffect: form.fields.gameMechanicEffect,
        limitation: form.fields.limitation,
        powerDuration: form.fields.powerDuration,
        areaOfEffect: form.fields.areaOfEffect,
        powerLevel: form.fields.powerLevel,
        powerActivationType: form.fields.powerActivationType,
        other: form.fields.other,
        isPowerUse: form.fields.isPowerUse,
        cost: form.fields.cost
    }
}
