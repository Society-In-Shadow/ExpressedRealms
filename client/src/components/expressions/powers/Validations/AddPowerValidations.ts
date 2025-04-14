import {array, boolean, number, object, string} from "yup";
import {useForm} from "vee-validate";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";
import {computed} from "vue";

const validationSchema = object({
    name: string()
        .required()
        .max(250)
        .label("Name"),
    category: array()
        .of(number().positive("Category must have positive numbers"))
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
    powerDuration: number()
        .integer()
        .min(1, "Power Duration must be between 1 and 255")
        .max(255, "Power Duration must be between 1 and 255")
        .required()
        .label("Power Duration"),
    areaOfEffect: number()
        .integer()
        .min(1, "Area of Effect must be greater than 0")
        .label("Area of Effect"),
    powerLevel: number()
        .integer()
        .required()
        .label("Power Level"),
    powerActivationType: number()
        .integer()
        .min(1, "Power Activation Type is a required field")
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