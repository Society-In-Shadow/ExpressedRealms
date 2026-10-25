import { defineStore } from 'pinia'
import axios from 'axios'

export const characterStore
  = defineStore('character', {
    state: () => {
      return {
        isLoading: true as boolean,
        name: '' as string,
        background: '' as string,
        expression: '' as string,
        expressionId: 0 as number,
        expressionSubTypeId: 0 as number | undefined,
        isPrimaryCharacter: false as boolean,
        isRetired: false as boolean,
        isInCharacterCreation: false as boolean,
        primaryProgressionId: 0 as number,
        secondaryProgressionId: 0 as number,
        isArchetypeCharacter: false as boolean,
        isOwner: false as boolean,
        characterId: 0 as number,
        canModifyPrimaryCharacter: false as boolean,
        factionRank: '' as string,
        factionName: '' as string,
        isPlaytestExpression: false as boolean,
        isLegacyExpression: false as boolean,
      }
    },
    actions: {
      async getCharacterDetails(characterId: number) {
        this.isLoading = false
        return await axios.get(`/characters/${characterId}`)
          .then(async (response) => {
            this.characterId = characterId
            this.name = response.data.name
            this.background = response.data.background
            this.expression = response.data.expression
            this.expressionId = response.data.expressionId
            this.isPrimaryCharacter = response.data.isPrimaryCharacter
            this.isInCharacterCreation = response.data.isInCharacterCreation
            this.isOwner = response.data.isOwner
            this.primaryProgressionId = response.data.primaryProgressionId
            this.secondaryProgressionId = response.data.secondaryProgressionId
            this.isRetired = response.data.isRetired
            this.isArchetypeCharacter = response.data.isArchetypeCharacter
            this.expressionSubTypeId = response.data.expressionSubTypeId
            this.factionRank = response.data.factionRank
            this.factionName = response.data.factionName
            this.isPlaytestExpression = response.data.isPlaytestExpression
            this.isLegacyExpression = response.data.isLegacyExpression
          })
      },
      async getEditOptions(id: number) {
        await axios.get(`/characters/${id}/options`)
          .then((response) => {
            this.canModifyPrimaryCharacter = response.data.canModifyPrimaryCharacter
          })
      },
    },
  })
