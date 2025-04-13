import {useForm} from "vee-validate";
import {object, string} from "yup";
import {computed} from "vue";
import type {FormField} from "@/FormWrappers/Interfaces/FormField";

const validationSchema = object({
    name: string()
        .required()
        .max(50)
        .label('Name'),
    shortDescription: string()
        .required()
        .max(125)
        .label('Short Description'),
    navMenuImage: string()
        .required()
        .label('Nav Menu Icon')
});

const { defineField, handleSubmit, errors } = useForm({
    initialValues:{
        navMenuImage: 'pi-prime'
    },
    validationSchema: validationSchema
});

function createFormField(fieldName: string): FormField {
    return {
        field: defineField(fieldName)[0],
        error: computed(() => errors.value[fieldName]),
        label: validationSchema.fields[fieldName].spec.label
    };
}

export const nameField = createFormField("name");
export const shortDescriptionField = createFormField("shortDescription");
export const navMenuImageField = createFormField("navMenuImage");

export { handleSubmit };
