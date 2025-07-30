import {defineStore} from "pinia";
import axios from "axios";
import type {Blessing, BlessingRequest} from "@/components/blessings/types";

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
            }
        }
    });
