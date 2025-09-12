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

const experienceInfo = experienceStore();

export const characterBlessingsStore =
    defineStore('characterBlessings', {
        state: () => {
            return {
                isLoading: true as boolean,
                blessings: [] as Array<CharacterBlessing>,
                activeBlessingId: 0 as number,
            }
        },    
        actions: {
            async getCharacterBlessings(characterId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterBlessingsBaseResponse>(`/characters/${characterId}/blessings`)
                    
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
                        toaster.success("Successfully Added Trait!");
                    });
            },
            editKnowledge: async function (values:CharacterBlessingForm, characterId: number, mappingId: number): Promise<void> {
                
                await axios.put(`/characters/${characterId}/blessings/${mappingId}`, {
                    blessingLevelId: values.blessingLevel.id,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterBlessings(characterId);
                        toaster.success("Successfully Updated Trait!");
                    });
            },
            deleteKnowledge: async function (characterId: number, mappingId: number): Promise<void> {
                await axios.delete(`/characters/${characterId}/blessings/${mappingId}`)
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterBlessings(characterId);
                        toaster.success("Successfully Deleted Knowledge!");
                    });
            },
        }
    });
