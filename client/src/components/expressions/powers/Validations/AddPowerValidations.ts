import {array, boolean, number, object, string} from "yup";
import {useForm} from "vee-validate";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";
import {computed} from "vue";

const validationSchema = object({
    name: string()
        .required()
        .max(250)
        .label("Name"),
    category: object().nullable()
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
    powerDuration: object().nullable()
        .required()
        .label("Power Duration"),
    areaOfEffect: object()
        .nullable()
        .label("Area of Effect"),
    powerLevel: object().nullable()
        .required()
        .label("Power Level"),
    powerActivationType: object().nullable()
        .required()
        .label("Power Activation Type"),
    other: string()
        .label("Other"),
    isPowerUse: boolean()
        .required()
        .label("Is Power Use")
});

// Destructure `useForm` to define handlers and fields
const { defineField, handleSubmit, errors } = useForm({
    validationSchema: validationSchema
});

// Define all fields using `defineField`
function createFormField(fieldName: string): FormField {
    return {
        field: defineField(fieldName)[0],
        error: computed(() => errors.value[fieldName]),
        label: validationSchema.fields[fieldName].spec.label
    };
}

export const name = createFormField("name");
export const category = createFormField("category");
export const description = createFormField("description");
export const gameMechanicEffect = createFormField("gameMechanicEffect");
export const limitation = createFormField("limitation");
export const powerDuration = createFormField("powerDuration");
export const areaOfEffect = createFormField("areaOfEffect");
export const powerLevel = createFormField("powerLevel");
export const powerActivationType = createFormField("powerActivationType");
export const other = createFormField("other");
export const isPowerUse = createFormField("isPowerUse");

export { handleSubmit };