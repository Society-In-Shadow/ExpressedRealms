import { type InferType, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem'
import type {
  ContactFrequency,
  ContactKnowledgeLevels,
  EditContact,
} from '@/components/characters/character/wizard/contacts/types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  notes: string().nullable()
    .label('Notes')
    .max(5000),
  knowledge: object<ListItem>().nullable()
    .required()
    .typeError('That is not a valid Knowledge option')
    .label('Knowledge'),
  frequency: object<ContactFrequency>()
    .required()
    .label('Contact Frequency Per Week'),
  knowledgeLevel: object<ContactKnowledgeLevels>()
    .nullable()
    .label('Knowledge Level'),
})

export type ContactForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (model: EditContact) => {
    form.fields.name.field.value = model.name
    form.fields.notes.field.value = model.notes
    form.fields.knowledge.field.value = model.knowledge
    form.fields.knowledgeLevel.field.value = model.knowledgeLevel
    form.fields.frequency.field.value = model.usesPerWeek
  }

  return {
    ...form,
    setValues,
  }
}
