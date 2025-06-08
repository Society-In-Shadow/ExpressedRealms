import {defineStore} from "pinia";
import axios from "axios";

import type {PowerPath} from "@/components/expressions/powerPaths/types";

export const powerPathStore =
    defineStore('powers', {
        state: () => {
            return {
                havePowerOptions: false,
                powerPaths: [] as PowerPath[]
            }
        },
        actions: {
            async getPowerPaths(expressionId: Number){
                if(expressionId === 0) {
                    console.log("expressionId isn't being loaded in");
                    return;
                }
                const response = await axios.get<PowerPath[]>("/expression/{expressionId}/powerPaths");
                this.powerPaths = response.data;
            },
/*            getPower: async function (expressionId: Number, powerPathId: Number): Promise<EditPower> {
                if (expressionId === 0) {
                    console.log("expressionId isn't being loaded in");
                }
                if (powerPathId === 0) {
                    console.log("powerId isn't being loaded in");
                }
                const response = await axios.get<>(`/powers/${expressionId}/${powerPathId}`);
                
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
            }*/
        }
    });
