import {array, boolean, number, object, string} from "yup";
import {useForm} from "vee-validate";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";
import {computed} from "vue";
import type {EditPower} from "@/components/expressions/powers/types/power";
import type {ListItem} from "@/types/ListItem";

export function getValidationInstance() {
    
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
            .min(1, "At least one category is required")
            .required("At least one category is required")
            .label("Category"),
        description: string()
            .required()
            .label("Description"),
        gameMechanicEffect: string()
            .required()
            .label("Game Mechanic Effect"),
        limitation: string()
            .required()
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
            .label("Other"),
        isPowerUse: boolean()
            .required()
            .label("Is Power Use")
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
    const category = createFormField("category");
    const description = createFormField("description");
    const gameMechanicEffect = createFormField("gameMechanicEffect");
    const limitation = createFormField("limitation");
    const powerDuration = createFormField("powerDuration");
    const areaOfEffect = createFormField("areaOfEffect");
    const powerLevel = createFormField("powerLevel");
    const powerActivationType = createFormField("powerActivationType");
    const other = createFormField("other");
    const isPowerUse = createFormField("isPowerUse");
    
    const setValues = (power: EditPower) => {
        name.field.value = power.name;
        category.field.value = power.categories;
        description.field.value = power.description;
        gameMechanicEffect.field.value = power.gameMechanicEffect;
        limitation.field.value = power.limitation;
        powerDuration.field.value = power.powerDuration;
        areaOfEffect.field.value = power.areaOfEffect;
        powerLevel.field.value = power.powerLevel;
        powerActivationType.field.value = power.powerActivationType;
        other.field.value = power.other;
        isPowerUse.field.value = power.isPowerUse;
    }
    
    const customResetForm = () => {
        powerDuration.field.value = null;
        areaOfEffect.field.value = null;
        powerLevel.field.value = null;
        powerActivationType.field.value = null;
        category.field.value = []; // TODO: This isn't working for some reason
        handleReset();
    };
    
    return {
        handleSubmit, 
        customResetForm,
        setValues,
        name,
        category,
        description,
        gameMechanicEffect,
        limitation,
        powerDuration,
        areaOfEffect,
        powerLevel,
        powerActivationType,
        other,
        isPowerUse,
    }
}
