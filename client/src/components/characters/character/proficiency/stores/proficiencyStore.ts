import {defineStore} from 'pinia'
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
            this.offensive = response.data.proficiencies.filter(x => x.type === 'Offensive')
            this.defensive = response.data.proficiencies.filter(x => x.type === 'Defensive')
            this.secondary = response.data.proficiencies.filter(x => x.type === 'Secondary')

            this.secondary.map((x) => {
              x.maxValue = x.value
            })
            this.isLoading = false
          })
      },
    },
  })
