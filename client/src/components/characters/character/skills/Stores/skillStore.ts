import {defineStore} from 'pinia'
import axios from 'axios'
import type {SkillResponse} from '@/components/characters/character/skills/interfaces/SkillOptionsResponse.ts'
import type {
    CharacterSkillsResponse,
} from '@/components/characters/character/skills/interfaces/CharacterSkillsResponse.ts'

export const skillStore
  = defineStore('skills', {
    state: () => {
      return {
        showExperience: false as boolean,
        editSkillTypeId: 0 as number,
        isLoadingSkills: false as boolean,
        offensiveSkills: [{}, {}, {}, {}, {}, {}] as CharacterSkillsResponse[],
        defensiveSkills: [{}, {}, {}, {}, {}, {}] as CharacterSkillsResponse[],
        skillLevels: [] as SkillResponse[],
        isLoadingSkillLevels: true as boolean,
      }
    },
    actions: {
      async getSkills(characterId: number) {
        this.isLoadingSkills = true
        return axios.get(`characters/${characterId}/skills`)
          .then((response) => {
            this.offensiveSkills = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 1)
            this.defensiveSkills = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 2)
            this.isLoadingSkills = false
          })
      },
      async getSkillOptions(characterId: number, skillTypeId: number) {
        this.isLoadingSkillLevels = true
        return axios.get(`characters/${characterId}/skills/${skillTypeId}`)
          .then((response) => {
            this.skillLevels = response.data
            this.isLoadingSkillLevels = false
          })
      },
    },
  })
