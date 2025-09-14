import {defineStore} from 'pinia'
import axios from "axios";
import toaster from "@/services/Toasters";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {
    CharacterBlessingForm
} from "@/components/characters/character/wizard/blessings/validators/blessingValidations.ts";
import type {
    CharacterBlessing,
    CharacterBlessingsBaseResponse
} from "@/components/characters/character/wizard/blessings/types.ts";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore.ts";
import type {BlessingType, SubCategory} from "@/components/blessings/types.ts";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";

const experienceInfo = experienceStore();
const blessingInfo = blessingsStore();
const wizardContentInfo = wizardContentStore();

export const characterBlessingsStore =
    defineStore('characterBlessings', {
        state: () => {
            return {
                isLoading: true as boolean,
                types: [] as Array<BlessingType>,
                advantages: [] as Array<SubCategory>,
                disadvantages: [] as Array<SubCategory>,
                mixedBlessings: [] as Array<CharacterBlessing>,
                blessings: [] as Array<CharacterBlessing>,
                selectedBlessingIds: [] as Array<number>,
            }
        },    
        actions: {
            async getCharacterBlessings(characterId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterBlessingsBaseResponse>(`/characters/${characterId}/blessings`)
                
                const selectedIds = response.data.blessings.map(x => x.blessingId);
                this.selectedBlessingIds = selectedIds;

                this.types = blessingInfo.types.map(type => {
                    
                    const subCategories = type.subCategories.map(subCategory => {
                        
                        const blessings = subCategory.blessings.filter(x => selectedIds.includes(x.id))
                        
                        if(blessings.length > 0){
                            return {
                                name: subCategory.name,
                                blessings: blessings
                            } as SubCategory
                        }
                        return {
                            name: "empty",
                            blessings: []
                        }
                    })
                    
                    const validCategories = subCategories.filter(x => x.blessings.length > 0);
                    if(validCategories.length > 0)
                        return {
                            name: type.name,
                            subCategories: validCategories
                        };
                    return {
                        name: "empty",
                        subCategories: []
                    };
                }).filter(x => x.subCategories.length > 0);
                
                this.blessings = response.data.blessings;
                this.isLoading = false;
            },
            addBlessing: async function (values:CharacterBlessingForm, characterId: number, blessingId: number): Promise<void> {
                await axios.post(`/characters/${characterId}/blessings/`, {
                    blessingId: blessingId,
                    blessingLevelId: values.blessingLevel?.id,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterBlessings(characterId);
                        toaster.success("Successfully Added Advantage / Disadvantage!");
                        wizardContentInfo.hideContent();
                    });
            },
            updateBlessing: async function (values:CharacterBlessingForm, characterId: number, mappingId: number): Promise<void> {
                
                await axios.put(`/characters/${characterId}/blessings/${mappingId}`, {
                    blessingLevelId: values.blessingLevel.id,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterBlessings(characterId);
                        toaster.success("Successfully Updated Advantage / Disadvantage!");
                    });
            },
            deleteBlessing: async function (characterId: number, mappingId: number): Promise<void> {
                await axios.delete(`/characters/${characterId}/blessings/${mappingId}`)
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterBlessings(characterId);
                        wizardContentInfo.hideContent();
                        toaster.success("Successfully Deleted Advantage / Disadvantage!");
                    });
            },
        }
    });
