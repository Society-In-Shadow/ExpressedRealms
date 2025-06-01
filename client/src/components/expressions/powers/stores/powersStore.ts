import {defineStore} from "pinia";
import axios from "axios";

import type {EditPower, EditPowerResponse, Power} from "@/components/expressions/powers/types/power";
import type {ListItem} from "@/types/ListItem";

export const powersStore =
    defineStore('powers', {
        state: () => {
            return {
                categories: [] as ListItem[],
                powerDurations: [] as ListItem[],
                powerLevels: [] as ListItem[],
                areaOfEffects: [] as ListItem[],
                powerActivationTypes: [] as ListItem[],
                havePowerOptions: false,
                powers: [] as Power[]
            }
        },
        actions: {
            async getPowerOptions(){
                if(this.havePowerOptions)
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
            async getPowers(expressionId: Number){
                if(expressionId === 0) {
                    console.log("expressionId isn't being loaded in");
                    return;
                }
                await axios.get(`/powers/${expressionId}`)
                    .then((response) => {
                        this.powers = response.data;
                    })
            },
            getPower: async function (expressionId: Number, powerId: Number): Promise<EditPower> {
                if (expressionId === 0) {
                    console.log("expressionId isn't being loaded in");
                }
                if (powerId === 0) {
                    console.log("powerId isn't being loaded in");
                }
                await this.getPowerOptions()
                const response = await axios.get<EditPowerResponse>(`/powers/${expressionId}/${powerId}`);
                
                return {
                    id: response.data.id,
                    name: response.data.name,
                    description: response.data.description,
                    gameMechanicEffect: response.data.gameMechanicEffect,
                    limitation: response.data.limitation,
                    categories: this.categories.filter((x: ListItem) => response.data.categoryIds.includes(x.id)) as ListItem[],
                    powerDuration: this.powerDurations.find((x: ListItem)  => x.id == response.data.powerDurationId) as ListItem,
                    areaOfEffect: this.areaOfEffects.find((x: ListItem)  => x.id == response.data.areaOfEffectId) as ListItem,
                    powerLevel: this.powerLevels.find((x: ListItem)  => x.id == response.data.powerLevelId) as ListItem,
                    powerActivationType: this.powerActivationTypes.find((x: ListItem)  => x.id == response.data.powerActivationTypeId) as ListItem,
                    other: response.data.other,
                    isPowerUse: response.data.isPowerUse,
                };
            }
        }
    });
