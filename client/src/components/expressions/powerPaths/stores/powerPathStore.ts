import {defineStore} from "pinia";
import axios from "axios";

import type {PowerPath, EditPowerPath} from "@/components/expressions/powerPaths/types";
import type {Power} from "@/components/expressions/powers/types";

export const powerPathStore =
    defineStore('powerPaths', {
        state: () => {
            return {
                havePowerOptions: false,
                powerPaths: [] as PowerPath[]
            }
        },
        actions: {
            async getPowerPaths(expressionId: number){
                const response = await axios.get<PowerPath[]>(`/expression/${expressionId}/powerPaths`);
                this.powerPaths = response.data;
            },
            async updatePowersForPath(powers: Power[], powerPathId:number){
                const powerPath = this.powerPaths.find(x => x.id == powerPathId)!;
                powerPath.powers = powers;
            },
            getPowerPath: async function (powerPathId: number): Promise<EditPowerPath> {
                const response = await axios.get<EditPowerPath>(`/powerpath/${powerPathId}`);
                
                return {
                    id: response.data.id,
                    name: response.data.name,
                    description: response.data.description,
                };
            }
        }
    });
