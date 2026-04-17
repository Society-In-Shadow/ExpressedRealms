import axios from 'axios'
import { type CharacterList, CharacterState } from '@/components/navbar/types.ts'

export const navMenuService = {
  getNavMenuCharacters: (): Promise<CharacterList> => axios.get<CharacterList>(`/navMenu/characters`)
    .then(async (response) => {
      const characters = response.data

      if (characters.length > 0) {
        const insertSpot = characters.length / 2
        characters.splice(insertSpot, 0, { id: -1, name: 'View Characters', expression: '', state: CharacterState.Regular })
      }

      characters.push({ id: -2, name: 'Add Character', expression: '', state: CharacterState.Regular })
      return response.data
    }),
}
