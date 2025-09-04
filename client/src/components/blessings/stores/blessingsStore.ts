import {defineStore} from "pinia";
import axios from "axios";
import type {Blessing, BlessingRequest} from "@/components/blessings/types";
import type {BlessingForm} from "@/components/blessings/validations/blessingForm.ts";
import toaster from "@/services/Toasters";

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
            }
        }
    });
