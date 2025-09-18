import {defineStore} from "pinia";
import axios from "axios";
import type {ExperienceBreakdownResponse} from "@/components/characters/character/types.ts"

export const XpSectionTypes = {
    advantage: 1,
    disadvantage: 2,
    knowledges: 3,
    powers: 4,
    skills: 5,
    stats: 6,
    discretionary: 7,
} as const;
export type XpSectionType = (typeof XpSectionTypes)[keyof typeof XpSectionTypes];

export const experienceStore =
    defineStore('experienceStore', {
        state: () => {
            return {
                isLoading: true as boolean,
                experienceBreakdown: {} as ExperienceBreakdownResponse,
                availableDiscretionary: 0 as number,
                showAllExperience: true as boolean,
            }
        },
        actions: {
            async updateExperience(characterId: number){
                this.isLoading = true;
                await axios.get<ExperienceBreakdownResponse>(`/characters/${characterId}/overallexperience`)
                    .then((response) => {
                        this.isLoading = false;
                        this.experienceBreakdown = response.data;
                        this.availableDiscretionary = response.data.availableDiscretionary
                    })
                },
            getExperienceInfo(name: string){
                return this.experienceBreakdown.experience.filter(x => x.name === name)[0];
            },
            getExperienceInfoForSection(sectionTypeId: XpSectionType){
                return this.experienceBreakdown.experience.filter(x => x.sectionTypeId === sectionTypeId)[0];
            },
            getCharacterLevel(): number{
                
                let totals = this.experienceBreakdown.experience.filter(x => x.name === "Total")[0];
                
                let total = totals.levelXp;
                if(total <= 0)
                    return 0;
                else if(total <= 25)
                    return 1;
                else if(total <= 75)
                    return 2;
                else if(total <= 150)
                    return 3;
                else if(total <= 250)
                    return 4;
                else if(total <= 375)
                    return 5;
                else if(total <= 525)
                    return 6;
                else if(total <= 700)
                    return 7;

                return 8;

            }
        }
    });
