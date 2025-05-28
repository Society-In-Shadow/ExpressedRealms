import {array, boolean, number, object, string} from "yup";
import {useForm} from "vee-validate";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";
import {computed} from "vue";

export interface ListItem {
    id: number;
    name: string;
    description: string;
}

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

const customResetForm = () => {
    powerDuration.field.value = null;
    areaOfEffect.field.value = null;
    powerLevel.field.value = null;
    powerActivationType.field.value = null;
    category.field.value = []; // TODO: This isn't working for some reason
    handleReset();
};

export { handleSubmit, customResetForm as resetForm };
