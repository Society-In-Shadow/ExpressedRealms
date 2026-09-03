import { useAppDialog } from '@/utilities/dialogUtilities.ts'

const AddSpecialization = () => import('@/components/characters/wizard/knowledges/AddSpecializationKnowledge.vue')
const EditSpecialization = () => import('@/components/characters/wizard/knowledges/EditSpecializationKnowledge.vue')

export const addKnowledgeDialog = () => {
  const dialog = useAppDialog()

  return {
    showAddSpecialization: data => dialog.open(AddSpecialization, { header: 'Add Specialization', data }),
    showEditSpecialization: data => dialog.open(EditSpecialization, { header: 'Edit Specialization', data }),
  }
}
