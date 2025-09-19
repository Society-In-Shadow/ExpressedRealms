import {type InferType, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {CharacterKnowledge, KnowledgeOptions} from "@/components/characters/character/knowledges/types";

const validationSchema = object({
    notes: string().nullable()
        .max(10000)
        .label("Notes"),
    knowledgeLevel2: object<KnowledgeOptions>()
        .label("Knowledge Level"),
});

export type CharacterKnowledgeForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (model: CharacterKnowledge) => {
        form.fields.notes.field.value = model.notes;
        //form.fields.knowledgeLevel2.field.value = knowledgeLevel;
        
    }

    const customResetForm = () => {
        form.fields.notes.field.value = "";
        form.fields.knowledgeLevel2.field.value = null;
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        customResetForm,
        setValues,
        notes: form.fields.notes,
        knowledgeLevel2: form.fields.knowledgeLevel2,
    }
}
