import {boolean, type InferType, object, string} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";
import type {BlessingLevel} from "@/components/blessings/types.ts";
import type {CharacterBlessing} from "@/components/characters/character/wizard/blessings/types.ts";
import type {Faction} from "@/components/characters/character/interfaces/Faction.ts";

const validationSchema = object({
    name: string().required()
        .max(150)
        .label("Name"),
    expression: string().nullable()
        .label('Expression'),
    faction: object<Faction>().nullable()
        .label('Faction'),
    background: string().nullable()
        .label('Background'),
    isPrimaryCharacter: boolean()
        .label('Is Primary Character')
});

export type CharacterBlessingForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (model: CharacterBlessing, level: BlessingLevel ) => {
    }

    const customResetForm = () => {
        form.fields.name.field.value = "";
        form.fields.expression.field.value = "";
        form.fields.faction.field.value = null;
        form.fields.background.field.value = "";
        form.fields.isPrimaryCharacter.field.value = false;
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        fields: form.fields,
        customResetForm,
        setValues
    }
}
