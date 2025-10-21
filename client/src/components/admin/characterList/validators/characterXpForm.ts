import {type InferType, number, object} from "yup";
import {useGenericForm} from "@/utilities/formUtilities";

const validationSchema = object({
    xp: number()
        .required()
        .label("Available Character XP"),
    playerNumber: number()
        .max(999)
        .required()
        .label("Player Number"),
});

export type CharacterXpForm = InferType<typeof validationSchema>;

export function getValidationInstance() {
    
    const form = useGenericForm(validationSchema);

    const setValues = (playerNumber: number, xpValue: number) => {
        form.fields.xp.field.value = xpValue;
        form.fields.playerNumber.field.value = playerNumber;
    }

    return {
        ...form,
        setValues,
    }
}
