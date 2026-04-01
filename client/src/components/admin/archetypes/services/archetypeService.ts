import axios from 'axios'
import type { ArchetypesResponse } from '@/components/admin/archetypes/types.ts'

export const userService = {
  getArchetypes: () => axios.get<ArchetypesResponse>(`/admin/archetypes`)
    .then(async (response) => { return response.data }),
}
