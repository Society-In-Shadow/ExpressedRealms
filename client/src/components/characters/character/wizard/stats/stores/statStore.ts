import {defineStore} from "pinia";
import axios from "axios";
import type {LevelInfo, Stat} from "@/components/characters/character/wizard/stats/types.ts";
import toasters from "@/services/Toasters.ts";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore.ts";

const experienceInfo = experienceStore();
const profStore = proficiencyStore();

export const statStore =
    defineStore('statStore', {
        state: () => {
            return {
                stats: [ {}, {}, {}, {}, {}, {}],
                isLoading: false as boolean,
                statLevels: [] as Array<LevelInfo>,
            }
        },
        actions: {
            async loadData(characterId: number){
                this.isLoading = true;
                await axios.get(`/characters/${characterId}/stats`)
                    .then((response) => {
                        this.stats = response.data;
                        this.isLoading = false;
                    })
            },
            async getEditOptions(statTypeId: number) {
                await axios.get(`/stats/${statTypeId}`)
                    .then((response) => {

                        /*const selectedXP = response.data.find(x => x.level == stat.value.statLevel).totalXP;

                        response.data.forEach(function(level:LevelInfo) {
                            level.disabled = level.totalXP > stat.value.availableXP + selectedXP && level.level > stat.value.statLevel;
                        });*/

                        this.statLevels = response.data;
                    })
            },
            async updateStat(stat:Stat, characterId: number, statTypeId: number){
                await axios.put(`/characters/${characterId}/stat/${statTypeId}`, {
                    levelTypeId: stat.statLevelInfo.level,
                    statTypeId: statTypeId,
                    characterId: characterId
                })
                await experienceInfo.updateExperience(characterId);
                await profStore.getUpdateProficiencies(characterId);
                await this.loadData(characterId);
                toasters.success("Successfully updated Level!");
            }
        }
    });