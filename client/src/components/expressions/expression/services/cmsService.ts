import axios from 'axios'
import type {
  AddExpressionPost,
  CmsSections,
  CopyExpressionPost,
  EditExpressionPut,
  EditItem,
} from '@/components/expressions/expression/types.ts'
import type { ExpressionMenuResponse } from '@/components/navbar/navMenuItems/types.ts'

export const cmsService = {
  getCmsValues: (): Promise<CmsSections> => axios.get<ExpressionMenuResponse>('/navMenu/content')
    .then(async response => ({
      expressionItems: response.data.menuItems.filter(x => x.expressionTypeId === 1),
      rulebookItems: response.data.menuItems.filter(x => x.expressionTypeId === 13),
      worldBackgroundItems: response.data.menuItems.filter(x => x.expressionTypeId === 14),
    })),
  getEdit: (id: number): Promise<EditItem> => axios.get(`/expression/${id}`)
    .then(async (response) => { return response.data }),
  edit: (id: number, faction: EditExpressionPut) => axios.put<EditExpressionPut>(`/expression/${id}`, faction)
    .then(async (response) => { return response.data }),
  create: (model: AddExpressionPost): Promise<number> => axios.post<number>(`/expression/`, model)
    .then(async (response) => { return response.data }),
  copy: (id: number, model: CopyExpressionPost): Promise<number> => axios.post<number>(`/expression/${id}/copy`, model)
    .then(async (response) => { return response.data }),
  delete: (id: number) => axios.delete(`/factions/${id}`)
    .then(async (response) => { return response.data }),
}
