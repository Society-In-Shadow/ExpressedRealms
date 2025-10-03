import {defineStore} from "pinia";
import axios from "axios";
import type {CalculatedExperience, ExperienceBreakdownResponse} from "@/components/characters/character/types.ts"
import {characterStore} from "@/components/characters/character/stores/characterStore.ts";
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore.ts";

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

const characterInfo = characterStore();
const proficiencyInfo = proficiencyStore();
export const experienceStore =
    defineStore('experienceStore', {
        state: () => {
            return {
                isLoading: true as boolean,
                experienceBreakdown: {} as ExperienceBreakdownResponse,
                availableDiscretionary: 0 as number,
                showAllExperience: true as boolean,
                totalSpentLevelXp: 0 as number,
                totalAvailableXp: 0 as number,
                calculatedValues: [] as CalculatedExperience[]
            }
        },
        actions: {
            async updateExperience(characterId: number){
                this.isLoading = true;
                await proficiencyInfo.getUpdateProficiencies(characterId);
                await axios.get<ExperienceBreakdownResponse>(`/characters/${characterId}/overallexperience`)
                    .then((response) => {
                        this.isLoading = false;
                        this.experienceBreakdown = response.data;
                        this.totalSpentLevelXp = response.data.totalSpentLevelXp;
                        this.totalAvailableXp = response.data.totalAvailableXp;
                        
                        this.calculatedValues = response.data.experience.map(xp => {

                            const requiredXp =  xp.total >= xp.characterCreateMax ? xp.characterCreateMax : xp.total;
                            const currentOptionalXp =  xp.total >= xp.characterCreateMax ? xp.total - xp.characterCreateMax : 0;
                            const optionalMaxXP = currentOptionalXp + response.data.availableDiscretionary
                            let availableXp = optionalMaxXP - currentOptionalXp
                            
                            if (requiredXp < xp.characterCreateMax && xp.sectionTypeId != XpSectionTypes.advantage) {
                                availableXp = xp.characterCreateMax - requiredXp + optionalMaxXP;
                            }
                            
                            if(xp.sectionTypeId == XpSectionTypes.advantage){
                                availableXp = response.data.availableDiscretionary;
                            }
                            
                            if(characterInfo.isPrimaryCharacter && !characterInfo.isInCharacterCreation){
                                const xpSpentPostCreation = xp.characterCreateMax - xp.total; // total = spentxp, characterCreateMax = availableXp
                                availableXp = xpSpentPostCreation - this.totalSpentLevelXp + xp.levelXp;
                            }
                            
                            return {
                                name: xp.name,
                                requiredXp,
                                currentOptionalXp,
                                optionalMaxXP,
                                availableXp,
                                characterCreateMax: xp.characterCreateMax,
                                total: xp.total,
                                sectionTypeId: xp.sectionTypeId,
                                levelXp: xp.levelXp,
                            }
                        })
                        
                        
                        this.availableDiscretionary = response.data.availableDiscretionary
                    })
                },
            getExperienceInfo(name: string){
                return this.experienceBreakdown.experience.filter(x => x.name === name)[0];
            },
            getExperienceInfoForSection(sectionTypeId: XpSectionType){
                return this.calculatedValues.filter(x => x.sectionTypeId === sectionTypeId)[0];
            },
            getTotalXp(): number{
                const filteredSections = [XpSectionTypes.advantage, XpSectionTypes.discretionary, XpSectionTypes.disadvantage]

                const filteredCalculatedValues = this.calculatedValues.filter(section => {
                    return !filteredSections.includes(section.sectionTypeId as XpSectionTypes);
                })
                return filteredCalculatedValues.reduce((sum, item) => sum + item.levelXp, 0)
            },
            getCharacterLevel(): number{
                const total = this.getTotalXp();

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
