import axios from 'axios'
import { type CharacterList } from '@/components/navbar/types.ts'

export const navMenuService = {
  getNavMenuCharacters: (): Promise<CharacterList> => axios.get<CharacterList>(`/navMenu/characters`)
    .then(async (response) => { return response.data }),
}
