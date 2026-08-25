import { defineStore } from 'pinia'
import axios from 'axios'
import type { LevelInfo, Stat } from '@/components/characters/wizard/stats/types.ts'
import toasters from '@/services/Toasters.ts'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'

const experienceInfo = experienceStore()

export const statStore
  = defineStore('statStore', {
    state: () => {
      return {
        stats: [{}, {}, {}, {}, {}, {}],
        isLoading: false as boolean,
        statLevels: [] as Array<LevelInfo>,
        hasExtraMortis: false as boolean,
      }
    },
    actions: {
      async loadData(characterId: number) {
        this.isLoading = true
        await axios.get(`/characters/${characterId}/stats`)
          .then((response) => {
            this.stats = response.data.stats
            this.hasExtraMortis = response.data.hasExtraMortis
            this.isLoading = false
          })
      },
      async updateMortis(characterId: number) {
        await axios.put(`/characters/${characterId}/stats/extraMortis`, {
          hasExtraMortis: this.hasExtraMortis,
        })
        await experienceInfo.updateExperience(characterId)
        toasters.success('Successfully updated Extra Mortis!')
      },
      async getEditOptions(statTypeId: number) {
        await axios.get(`/stats/${statTypeId}`)
          .then((response) => {
            /* const selectedXP = response.data.find(x => x.level == stat.value.statLevel).totalXP;

                        response.data.forEach(function(level:LevelInfo) {
                            level.disabled = level.totalXP > stat.value.availableXP + selectedXP && level.level > stat.value.statLevel;
                        }); */

            this.statLevels = response.data
          })
      },
      async updateStat(stat: Stat, characterId: number, statTypeId: number) {
        await axios.put(`/characters/${characterId}/stat/${statTypeId}`, {
          levelTypeId: stat.statLevelInfo.level,
        })
        await experienceInfo.updateExperience(characterId)
        await this.loadData(characterId)
        toasters.success('Successfully updated Level!')
      },
    },
  })
