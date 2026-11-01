import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem.ts'
import type {
  EditExpressionSection,
  EditExpressionSectionTypeFormFields,
} from '@/components/expressions/expressionSection/types.ts'

const validationSchema = object({
  name: string().required()
    .max(50)
    .label('Name'),
  content: string()
    .required()
    .label('Content'),
  sectionType: object<ListItem>().required()
    .label('Section Type'),
})

export function getValidationInstance(): GenericForm<EditExpressionSectionTypeFormFields> {
  const form = useGenericForm(validationSchema)

  const setValues = (data: EditExpressionSection) => {
    form.fields.name.field.value = data.name
    form.fields.content.field.value = data.content
    form.fields.sectionType.field.value = data.sectionType
  }

  return {
    ...form,
    setValues,
  }
}
