import {type InferType, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {ListItem} from "@/types/ListItem";
import type {Blessing} from "@/components/blessings/types.ts";

const validationSchema = object({
    name: string()
        .required()
        .max(250)
        .label("Name"),
    description: string()
        .required()
        .label("Description"),
    type: string()
        .required()
        .label("Type"),
    subCategory: string()
        .required()
        .label("Sub Category Type"),
});

export type BlessingForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (power: Blessing) => {
        form.fields.name.field.value = power.name;
        form.fields.description.field.value = power.description;
        form.fields.type.field.value = power.type;
        form.fields.subCategory.field.value = power.subCategory;
    }
    
    const customResetForm = () => {
        form.fields.description.field.value = "";
        form.fields.name.field.value = "";
        form.fields.type.field.value = "";
        form.fields.subCategory.field.value = "";
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        name: form.fields.name,
        description: form.fields.description,
        type: form.fields.type,
        subCategory: form.fields.subCategory,
    }
}
