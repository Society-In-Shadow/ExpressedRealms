import axios from 'axios'
import type { ArchetypesResponse } from '@/components/admin/archetypes/types.ts'

export const archetypeService = {
  getArchetypes: () => axios.get<ArchetypesResponse>(`/admin/archetypes`)
    .then(async (response) => { return response.data }),
  delete: (id: number) => axios.delete(`/admin/archetypes/${id}`)
    .then(async (response) => { return response.data }),
}
