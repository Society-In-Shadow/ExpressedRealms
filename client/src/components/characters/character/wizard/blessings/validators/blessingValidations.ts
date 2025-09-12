import {type InferType, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {CharacterKnowledge, KnowledgeOptions} from "@/components/characters/character/knowledges/types";
import type {BlessingLevel} from "@/components/blessings/types.ts";

const validationSchema = object({
    notes: string().nullable()
        .max(5000)
        .label("Notes"),
    blessingLevel: object<BlessingLevel>().nullable()
        .label("Blessing Level"),
});

export type CharacterBlessingForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (model: CharacterKnowledge, knowledgeLevel: KnowledgeOptions | null) => {
        form.fields.notes.field.value = model.notes;
        form.fields.blessingLevel.field.value = model.levelId;
    }

    const customResetForm = () => {
        form.fields.notes.field.value = "";
        form.fields.blessingLevel.field.value = null;
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        customResetForm,
        setValues,
        notes: form.fields.notes,
        blessingLevel: form.fields.blessingLevel
    }
}
