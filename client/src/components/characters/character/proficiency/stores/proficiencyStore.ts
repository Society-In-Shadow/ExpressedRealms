import { defineStore } from 'pinia'
import axios from 'axios'

export const proficiencyStore
  = defineStore('proficiencies', {
    state: () => {
      return {
        isLoading: true as boolean,
        offensive: [] as ProficienciesDto[],
        defensive: [] as ProficienciesDto[],
        secondary: [] as ProficienciesDto[],
      }
    },
    actions: {
      async getUpdateProficiencies(characterId: number) {
        this.isLoading = true
        await axios.get(`proficiencies/${characterId}`)
          .then((response) => {
            this.offensive = response.data.proficiencies.filter(x => x.type === 1)
            this.defensive = response.data.proficiencies.filter(x => x.type === 2)
            this.secondary = response.data.proficiencies.filter(x => x.type === 3)

            this.isLoading = false
          })
      },
    },
  })
