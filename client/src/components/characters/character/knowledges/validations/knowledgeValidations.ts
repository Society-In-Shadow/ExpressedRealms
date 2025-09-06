import {type InferType, number, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {CharacterKnowledge, KnowledgeOptions} from "@/components/characters/character/knowledges/types";

const validationSchema = object({
    notes: string().nullable()
        .max(10000)
        .label("Notes"),
    knowledgeLevel2: object<KnowledgeOptions>()
        .label("Knowledge Level"),
    knowledgeLevel: number()
        .label("Knowledge Level"),
});

export type CharacterKnowledgeForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (model: CharacterKnowledge, knowledgeLevel: KnowledgeOptions | null) => {
        form.fields.notes.field.value = model.notes;
        if(knowledgeLevel != null){
            form.fields.knowledgeLevel2.field.value = knowledgeLevel;
        }else{
            form.fields.knowledgeLevel.field.value = model.levelId;
        }
        
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
        knowledgeLevel2: form.fields.knowledgeLevel2,
    }
}
