import { defineStore } from 'pinia'
import axios from 'axios'
import type { CharacterListResponse, PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import type { CharacterXpForm } from '@/components/admin/characterList/validators/characterXpForm.ts'
import { CheckinStage } from '@/components/conCheckin/types.ts'

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
            this.sortCharacters()
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
        this.sortCharacters()
      },
      sortCharacters() {
        this.filteredCharacters = this.filteredCharacters.sort((a, b) => a.name.localeCompare(b.name))
      },
      getAwaitingCheckin() {
        const awaitingStageIds = [CheckinStage.AwaitingCheckin]
        return this.filteredCharacters.filter(x => x.activeStages.some(stage => awaitingStageIds.includes(stage)))
      },
      getAwaitingGoApproval() {
        const awaitingStageIds = [CheckinStage.AssignedXpCheck, CheckinStage.GoApproval]
        return this.filteredCharacters.filter(x => x.activeStages.some(stage => awaitingStageIds.includes(stage)))
      },
      getAwaitingCrbPrinting() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.CrbPrinted))
      },
      getPrintedCrbs() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.CrbAssembled))
      },
      getAwaitingPickup() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.CrbPickedUp))
      },
      getAwaitingDay2() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.Day2Checkin))
      },
      getAwaitingDay3() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.Day3Checkin))
      },
      getCompletedCharacters() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.FinalStage))
      },
      getAwaitingGoPreApproval() {
        return this.filteredCharacters.filter(x => x.activeStages.includes(CheckinStage.PlayerEarlyCheckin))
      },
      getFactionPromotions() {
        return this.filteredCharacters.filter(x => x.hasPromotionRequest)
      },
    },

  })
