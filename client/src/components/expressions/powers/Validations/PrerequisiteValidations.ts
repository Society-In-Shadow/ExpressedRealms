import {array, type InferType, number, object, string} from "yup";
import type {ListItem} from "@/types/ListItem";
import {useGenericForm} from "@/utilities/formUtilities";

const validationSchema = object({
    powers: array()
        .of(
            object({
                id: number().required(),
                name: string().required(),
                description: string(),
            })
        )
        .required()
        .label("of the following powers"),

    requiredAmount: object<ListItem>().nullable()
        .required()
        .label("Power Requires")
        .test('less-than-powers', 'Required amount needs to be less then or equal to the number of selected powers', function(value: ListItem){
            const powers = this.parent.powers;
            if (!value || !powers || powers.length == 0) return true;
            return value.id <= powers.length;

        }),
});

export type PrerequisiteForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (power: PrerequisiteForm) => {
        form.fields.powers.field.value = power.powers;
        form.fields.requiredAmount.field.value = power.requiredAmount;
    }

    const customResetForm = () => {
        form.fields.requiredAmount.field.value = null;
        form.fields.powers.field.value = [];
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        customResetForm,
        setValues,
        powers: form.fields.powers,
        requiredAmount: form.fields.requiredAmount,
    }
}
