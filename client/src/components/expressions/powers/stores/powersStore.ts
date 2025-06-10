import {defineStore} from "pinia";
import axios from "axios";

import type {EditPower, EditPowerResponse, Power, PowerStore} from "@/components/expressions/powers/types";
import type {ListItem} from "@/types/ListItem";

export const powersStore = 
    defineStore(`powers`, {
        state: () => {
            return {
                categories: [] as ListItem[],
                powerDurations: [] as ListItem[],
                powerLevels: [] as ListItem[],
                areaOfEffects: [] as ListItem[],
                powerActivationTypes: [] as ListItem[],
                havePowerOptions: false,
                powers: [] as PowerStore[]
            }
        },
        actions: {
            async getPowerOptions() {
                if (this.havePowerOptions)
                    return;

                await axios.get("/powers/options")
                    .then((response) => {
                        this.categories = response.data.category;
                        this.powerDurations = response.data.powerDuration;
                        this.powerLevels = response.data.powerLevel;
                        this.areaOfEffects = response.data.areaOfEffect;
                        this.powerActivationTypes = response.data.powerActivationType;
                        this.havePowerOptions = true;
                    })
            },
            async getPowers(powerPathId: number){         
                const response = await axios.get<Power[]>(`/powerpath/${powerPathId}/powers`);
               
                const newItem:PowerStore = { powerPathId: powerPathId, powers: response.data};
                
                const index = this.powers.findIndex(item => item.powerPathId === powerPathId);
                if (index === -1) {
                    this.powers.push(newItem);
                } else {
                    this.powers[index].powers = response.data;
                }
            },
            getPower: async function (powerId: number): Promise<EditPower> {
                await this.getPowerOptions()
                const response = await axios.get<EditPowerResponse>(`/powers/${powerId}`);

                return {
                    id: response.data.id,
                    name: response.data.name,
                    description: response.data.description,
                    gameMechanicEffect: response.data.gameMechanicEffect,
                    limitation: response.data.limitation,
                    categories: this.categories.filter((x: ListItem) => response.data.categoryIds.includes(x.id)) as ListItem[],
                    powerDuration: this.powerDurations.find((x: ListItem) => x.id == response.data.powerDurationId) as ListItem,
                    areaOfEffect: this.areaOfEffects.find((x: ListItem) => x.id == response.data.areaOfEffectId) as ListItem,
                    powerLevel: this.powerLevels.find((x: ListItem) => x.id == response.data.powerLevelId) as ListItem,
                    powerActivationType: this.powerActivationTypes.find((x: ListItem) => x.id == response.data.powerActivationTypeId) as ListItem,
                    other: response.data.other,
                    isPowerUse: response.data.isPowerUse,
                };
            }
        }
    });
