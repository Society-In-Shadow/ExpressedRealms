import {defineStore} from 'pinia'
import axios from 'axios'
import type {BlessingLevel, BlessingRequest, BlessingType, SubCategory} from '@/components/blessings/types'
import type {BlessingForm} from '@/components/blessings/validations/blessingForm.ts'
import toaster from '@/services/Toasters'

export const blessingsStore
  = defineStore(`blessings`, {
    state: () => {
      return {
        advantages: [] as SubCategory[],
        disadvantages: [] as SubCategory[],
        types: [] as BlessingType[],
      }
    },
    actions: {
      async getBlessings() {
        const response = await axios.get<BlessingRequest>(`/blessings`)
        this.advantages = response.data.advantages
        this.disadvantages = response.data.disadvantages
        this.types = [
          { name: 'Advantage', subCategories: response.data.advantages },
          { name: 'Disadvantage', subCategories: response.data.disadvantages },
        ]
      },
      async addBlessing(blessing: BlessingForm) {
        await axios.post(`/blessings/`, {
          name: blessing.name,
          description: blessing.description,
          category: blessing.subCategory,
          type: blessing.type,
        })
        toaster.success('Successfully added Blessing!')
        await this.getBlessings()
      },
      async editBlessing(blessingId: number, blessing: BlessingForm) {
        await axios.put(`/blessings/${blessingId}`, {
          name: blessing.name,
          description: blessing.description,
          category: blessing.subCategory,
          type: blessing.type,
        })
        toaster.success('Successfully edited Blessing!')
        await this.getBlessings()
      },
      async deleteBlessing(blessingId: number) {
        await axios.delete(`/blessings/${blessingId}`)
        toaster.success('Successfully deleted Blessing!')
        await this.getBlessings()
      },
      async getBlessingLevel(blessingId: number, levelId: number): Promise<BlessingLevel> {
        const response = await axios.get<BlessingLevel>(`/blessings/${blessingId}/level/${levelId}`)
        return response.data
      },
      async addBlessingLevel(blessingId: number, form) {
        try {
          await axios.post(`/blessings/${blessingId}/level/`, {
            level: form.fields.level.field.value,
            description: form.fields.description.field.value,
            xpCost: form.fields.xpCost.field.value,
            xpGain: form.fields.xpGain.field.value,
          })
          toaster.success('Successfully added Blessing Level!')
          await this.getBlessings()
          return true
        }
        catch (error) {
          const errors = error?.response.data?.errors as Record<string, string[] | string> | undefined
          if (errors) {
            form.setErrors(errors)
          }
          return false
        }
      },
      async editBlessingLevel(blessingId: number, levelId: number, form): Promise<boolean> {
        try {
          await axios.put(`/blessings/${blessingId}/level/${levelId}`, {
            level: form.fields.level.field.value,
            description: form.fields.description.field.value,
            xpCost: form.fields.xpCost.field.value,
            xpGain: form.fields.xpGain.field.value,
          })
          toaster.success('Successfully edited Blessing Level!')
          await this.getBlessings()
          return true
        }
        catch (error) {
          const errors = error?.response.data?.errors as Record<string, string[] | string> | undefined
          if (errors) {
            form.setErrors(errors)
          }
          return false
        }
      },
      async deleteBlessingLevel(blessingId: number, levelId: number) {
        await axios.delete(`/blessings/${blessingId}/level/${levelId}`)
        toaster.success('Successfully deleted Blessing Level!')
        await this.getBlessings()
      },
    },
  })
