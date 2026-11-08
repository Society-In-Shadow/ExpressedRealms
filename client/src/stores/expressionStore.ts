import { defineStore } from 'pinia'
import axios from 'axios'
import { cmsStore } from '@/stores/cmsStore.ts'
import router from '@/router'
import type { ExpressionSubTypes } from '@/components/expressions/expressionSubTypes'
import type { ExpressionSectionDto } from '@/components/expressions/types.ts'

const cmsInfo = cmsStore()
export const expressionStore
  = defineStore('expression', {
    state: () => {
      return {
        sections: [
          {
            id: 1,
            subSections: [
              { id: 2, subSections: [] },
              { id: 3, subSections: [] },
              { id: 4, subSections: [] },
            ],
          },
          {
            id: 5,
            subSections: [],
          },
          {
            id: 6,
            subSections: [{ id: 7 }],
          },
          {
            id: 8,
            subSections: [{ id: 9 }],
          },
        ] as ExpressionSectionDto[],
        currentExpressionId: 0 as number,
        currentExpressionName: '' as string,
        isDoneLoading: false as boolean,
        isSpecialExpression: false as boolean,
        expressionTypeId: 0 as number,
        expressionSubTypeId: 0 as ExpressionSubTypes,
      }
    },
    actions: {
      async getExpressionId(route: object) {
        await cmsInfo.getCmsInformation()
        let filter = []
        if (route.meta.isRuleBook) {
          filter = cmsInfo.rulebookItems.filter(x => x.slug == route.params.slug)
        }
        else if (route.meta.isWorldBackground) {
          filter = cmsInfo.worldBackgroundItems.filter(x => x.slug == route.params.slug)
        }
        else {
          filter = cmsInfo.expressionItems.filter(x => x.slug == route.params.slug)
        }
        if (filter && filter.length > 0) {
          this.currentExpressionId = filter[0].id
          this.currentExpressionName = filter[0].name
          this.expressionTypeId = filter[0].expressionTypeId
          this.expressionSubTypeId = filter[0].expressionSubTypeId
          return
        }
        router.push('/characters')
      },
      async getExpressionSections() {
        this.isDoneLoading = false
        return await axios.get(`/expressionSubSections/${this.currentExpressionId}`)
          .then(async (json) => {
            this.sections = json.data.expressionSections
            this.isDoneLoading = true
          })
      },
    },
  })
