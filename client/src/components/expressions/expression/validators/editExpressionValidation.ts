import { number, object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { EditExpressionFields, EditItem } from '@/components/expressions/expression/types.ts'
import type { ListItem } from '@/types/ListItem.ts'

const validationSchema = object({
  name: string().required()
    .max(50)
    .label('Name'),
  shortDescription: string()
    .required()
    .max(125)
    .label('Short Description'),
  navMenuImage: string().required()
    .label('Nav Menu Icon'),
  publishStatus: object<ListItem>().required()
    .label('Publish Status'),
  sortOrder: number().required()
    .label('Sort Order'),
})

export function getValidationInstance(): GenericForm<EditExpressionFields> {
  const form = useGenericForm(validationSchema)

  const setValues = (data: EditItem) => {
    form.fields.name.field.value = data.name
    form.fields.shortDescription.field.value = data.shortDescription
    form.fields.navMenuImage.field.value = data.navMenuImage
    form.fields.publishStatus.field.value = data.publishTypes.find(x => x.id == data.publishStatus)
    form.fields.sortOrder.field.value = data.sortOrder
  }

  return {
    ...form,
    setValues,
  }
}
