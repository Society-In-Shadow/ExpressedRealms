import {defineStore} from "pinia";
import axios from "axios";

import type {PowerPath, EditPowerPath} from "@/components/expressions/powerPaths/types";

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
