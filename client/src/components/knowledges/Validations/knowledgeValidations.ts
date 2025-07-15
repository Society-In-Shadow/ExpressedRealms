import {type InferType, object, string} from "yup";
import { useGenericForm } from "@/utilities/formUtilities";
import type {ListItem} from "@/types/ListItem";
import type {EditKnowledge} from "@/components/knowledges/types";

const validationSchema = object({
    name: string()
        .required()
        .max(250)
        .label("Name"),
    description: string()
        .required()
        .label("Description"),
    knowledgeType: object<ListItem>().nullable()
        .required()
        .label("Knowledge Type"),
});

export type KnowledgeForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (power: EditKnowledge) => {
        form.fields.name.field.value = power.name;
        form.fields.description.field.value = power.description;
        form.fields.knowledgeType.field.value = power.knowledgeType;
    }
    
    const customResetForm = () => {
        form.fields.description.field.value = "";
        form.fields.name.field.value = "";
        form.fields.knowledgeType.field.value = null;
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        name: form.fields.name,
        description: form.fields.description,
        knowledgeType: form.fields.knowledgeType,
    }
}
