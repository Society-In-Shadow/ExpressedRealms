import {defineStore} from "pinia";
import axios from "axios";

import type {ProgressionPath, ProgressionPathResponse} from "@/components/expressions/progressionPaths/types.ts";

export const progressionPathStore =
    defineStore('progressionPaths', {
        state: () => {
            return {
                havePowerOptions: false,
                progressionPaths: [] as ProgressionPath[]
            }
        },
        actions: {
            async getProgressionPaths(expressionId: number){
                const response = await axios.get<ProgressionPathResponse>(`/expression/${expressionId}/progressions`);
                this.progressionPaths = response.data.paths;
            },
            getProgressionPath: async function (powerPathId: number): Promise<ProgressionPath> {
                console.log(powerPathId);

                const path = this.progressionPaths.find(x => x.id === powerPathId);
                if (!path) {
                    throw new Error(`ProgressionPath with id ${powerPathId} not found`);
                }
                return path;
            }
        }
    });
