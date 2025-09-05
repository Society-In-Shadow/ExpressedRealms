import {defineStore} from "pinia";
import axios from "axios";
import type {Blessing, BlessingLevel, BlessingRequest} from "@/components/blessings/types";
import type {BlessingForm} from "@/components/blessings/validations/blessingForm.ts";
import toaster from "@/services/Toasters";
import type {BlessingLevelForm} from "@/components/blessings/validations/blessingLevelForm.ts";

export const blessingsStore =
    defineStore(`blessings`, {
        state: () => {
            return {
                advantages: [] as Blessing[],
                disadvantages: [] as Blessing[],
                mixedBlessings: [] as Blessing[]
            }
        },
        actions: {
            async getBlessings(){
                const response = await axios.get<BlessingRequest>(`/blessings`);
                this.advantages = response.data.advantages
                this.disadvantages = response.data.disadvantages
                this.mixedBlessings = response.data.mixedBlessings
            },
            async addBlessing(blessing: BlessingForm){
                await axios.post(`/blessings/`, {
                    name: blessing.name,
                    description: blessing.description,
                    category: blessing.subCategory,
                    type: blessing.type
                });
                toaster.success("Successfully added Blessing!");
                await this.getBlessings();
            },
            async editBlessing(blessingId: number, blessing: BlessingForm){
                await axios.put(`/blessings/${blessingId}`, {
                    name: blessing.name,
                    description: blessing.description,
                    category: blessing.subCategory,
                    type: blessing.type
                });
                toaster.success("Successfully edited Blessing!");
                await this.getBlessings();
            },
            async deleteBlessing(blessingId: number){
                await axios.delete(`/blessings/${blessingId}`);
                toaster.success("Successfully deleted Blessing!");
                await this.getBlessings();
            },
            async getBlessingLevel(blessingId: number, levelId: number): Promise<BlessingLevel>{
                const response = await axios.get<BlessingLevel>(`/blessings/${blessingId}/level/${levelId}`);
                return response.data;
            },
            async addBlessingLevel(blessingId: number, form: BlessingLevelForm){
                await axios.post(`/blessings/${blessingId}/level/`, {
                    level: form.level,
                    description: form.description,
                    xpCost: form.xpCost,
                    xpGain: form.xpGain
                });
                toaster.success("Successfully added Blessing Level!");
                await this.getBlessings();
            },
            async editBlessingLevel(blessingId: number, levelId: number, form: BlessingLevelForm){
                await axios.put(`/blessings/${blessingId}/level/${levelId}`, {
                    level: form.level,
                    description: form.description,
                    xpCost: form.xpCost,
                    xpGain: form.xpGain
                });
                toaster.success("Successfully edited Blessing Level!");
                await this.getBlessings();
            },
            async deleteBlessingLevel(blessingId: number, levelId: number){
                await axios.delete(`/blessings/${blessingId}/level/${levelId}`);
                toaster.success("Successfully deleted Blessing Level!");
                await this.getBlessings();
            }
        }
    });
