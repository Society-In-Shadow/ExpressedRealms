import {defineStore} from "pinia";
import axios from "axios";
import type {ListItem} from "@/components/expressions/powers/Validations/AddPowerValidations";
import type {Power} from "@/components/expressions/powers/types/power";

export const powersStore =
    defineStore('powers', {
        state: () => {
            return {
                categories: [] as ListItem[],
                powerDurations: [] as ListItem[],
                powerLevels: [] as ListItem[],
                areaOfEffects: [] as ListItem[],
                powerActivationTypes: [] as ListItem[],
                powers: [] as Power[]
            }
        },
        actions: {
            async getPowerOptions(){
                await axios.get("/powers/options")
                    .then((response) => {
                        this.categories = response.data.category;
                        this.powerDurations = response.data.powerDuration;
                        this.powerLevels = response.data.powerLevel;
                        this.areaOfEffects = response.data.areaOfEffect;
                        this.powerActivationTypes = response.data.powerActivationType;
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
            }
        }
    });
