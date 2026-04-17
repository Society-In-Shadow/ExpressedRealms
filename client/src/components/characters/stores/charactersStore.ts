import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import { useQuery } from '@pinia/colada'
import { characterListQuery } from '@/components/navbar/stores/navMenuStore.ts'

const { refresh } = useQuery(characterListQuery)

export const charactersStore
  = defineStore('charactersStore', {
    state: () => {
      return {
        characters: [] as any[],
      }
    },
    actions: {
      async getCharacters() {
        await axios.get('/characters')
          .then((json) => {
            this.characters = json.data
          })
      },
      async deleteCharacter(id: number) {
        await axios.delete(`/characters/${id}`)
          .then(async () => {
            await this.getCharacters()
            await refresh()
            toaster.success('Successfully Deleted Character!')
          })
      },
    },
  })
