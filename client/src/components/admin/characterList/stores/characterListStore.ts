import { defineStore } from 'pinia'
import axios from 'axios'
import type { CharacterListResponse, PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import type { CharacterXpForm } from '@/components/admin/characterList/validators/characterXpForm.ts'

export const adminCharacterListStore
  = defineStore('adminCharacterList', {
    state: () => {
      return {
        primaryCharacters: [] as Array<PrimaryCharacter>,
        filteredCharacters: [] as Array<PrimaryCharacter>,
      }
    },
    actions: {
      async fetchCharacters() {
        await axios.get<CharacterListResponse>('/admin/characters')
          .then((response) => {
            this.primaryCharacters = response.data.characters
            this.filteredCharacters = response.data.characters
          })
      },
      async updateCharacterXp(formValues: CharacterXpForm, characterId: number) {
        await axios.put(`/admin/characters/${characterId}/updateXp`, { playerNumber: formValues.playerNumber })
          .then(() => {
            this.fetchCharacters()
          })
      },
      filterCharacters(query: string) {
        const lowercasedQuery = query.toLowerCase().trim()

        if (lowercasedQuery === '' || lowercasedQuery === null || lowercasedQuery === undefined) {
          this.filteredCharacters = this.primaryCharacters
        }
        else {
          this.filteredCharacters = this.primaryCharacters.filter(character =>
            character.name.toLowerCase().includes(lowercasedQuery)
            || character.playerName.toLowerCase().includes(lowercasedQuery)
            || character.playerNumber.toString().padStart(3, '0').includes(lowercasedQuery),
          )
        }
      },
    },

  })
