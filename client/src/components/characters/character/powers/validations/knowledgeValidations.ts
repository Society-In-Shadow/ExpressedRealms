import {type InferType, number, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types";

const validationSchema = object({
    notes: string().nullable()
        .max(10000)
        .label("Notes"),
    knowledgeLevel: number()
        .required()
        .label("Knowledge Level"),
});

export type CharacterKnowledgeForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
        
    const form = useGenericForm(validationSchema);
    
    const setValues = (model: CharacterKnowledge) => {
        form.fields.notes.field.value = model.notes;
        form.fields.knowledgeLevel.field.value = model.levelId;
    }
    
    const customResetForm = () => {
        form.fields.notes.field.value = "";
        form.fields.knowledgeLevel.field.value = null;
        form.handleReset();
    };
    
    return {
        handleSubmit: form.handleSubmit, 
        customResetForm,
        setValues,
        notes: form.fields.notes,
        knowledgeLevel: form.fields.knowledgeLevel,
    }
}
