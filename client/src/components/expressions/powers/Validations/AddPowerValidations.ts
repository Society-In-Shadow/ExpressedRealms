import {array, boolean, number, object, string} from "yup";

export const createPowerModelSchema = object({
    name: string().required("Name is required").label("Name"),
    category: array()
        .of(number().positive("Category must have positive numbers"))
        .min(1, "At least one category is required")
        .required("At least one category is required")
        .label("Category"),
    description: string().required("Description is required").label("Description"),
    gameMechanicEffect: string()
        .required("Game Mechanic Effect is required")
        .label("Game Mechanic Effect"),
    limitation: string().required("Limitation is required").label("Limitation"),
    powerDuration: number()
        .integer()
        .min(1, "Power Duration must be between 1 and 255")
        .max(255, "Power Duration must be between 1 and 255")
        .required("Power Duration is required")
        .label("Power Duration"),
    areaOfEffect: number().integer().min(1, "Area of Effect must be greater than 0").label("Area Of Effect"),
    powerLevel: number().integer().required("Power Level is required"),
    powerActivationType: number()
        .integer()
        .min(1, "Power Activation Type is required")
        .required("Power Activation Type is required"),
    other: string(),
    isPowerUse: boolean().required("Is Power Use is required")
});