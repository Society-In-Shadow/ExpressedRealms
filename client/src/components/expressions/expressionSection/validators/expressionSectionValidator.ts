import {type InferType, object, string} from "yup";
import { useGenericForm } from "@/utilities/formUtilities";
import type {ListItem} from "@/types/ListItem";
import type {EditExpressionSection} from "@/components/expressions/expressionSection/types";

const validationSchema = object({
    name: string().required()
        .label('Name'),
    content: string()
        .required()
        .label('Content'),
    sectionType: object<ListItem>().nullable()
        .label('Section Type')
})

export type ExpressionSectionForm = InferType<typeof validationSchema>;

export function getValidationInstance() {

    const form = useGenericForm(validationSchema);

    const setValues = (power: EditExpressionSection) => {
        form.fields.name.field.value = power.name;
        form.fields.content.field.value = power.content;
        form.fields.sectionType.field.value = power.sectionType
    }

    const customResetForm = () => {
        form.fields.name.field.value = "";
        form.fields.content.field.value = "";
        form.fields.sectionType.field.value = null;
        form.handleReset();
    };

    return {
        handleSubmit: form.handleSubmit,
        customResetForm,
        setValues,
        name: form.fields.name,
        content: form.fields.content,
        sectionType: form.fields.sectionType,
    }
}
