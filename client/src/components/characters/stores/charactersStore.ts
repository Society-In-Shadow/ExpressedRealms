import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import { characterListQuery } from '@/components/navbar/stores/navMenuStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'

const { refresh } = useQueryWithLoading(characterListQuery)

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
