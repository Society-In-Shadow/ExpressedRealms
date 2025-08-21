import {type InferType, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types";

const validationSchema = object({
    notes: string().nullable()
        .max(5000)
        .label("Notes"),
});

export type CharacterPowerForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (model: CharacterKnowledge) => {
        form.fields.notes.field.value = model.notes;
    }

    const customResetForm = () => {
        form.fields.notes.field.value = "";
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        customResetForm,
        setValues,
        notes: form.fields.notes,
    }
}
