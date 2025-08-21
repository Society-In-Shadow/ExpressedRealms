import {defineStore} from 'pinia'
import axios from "axios";
import toaster from "@/services/Toasters";
import type {
    CharacterKnowledgeForm
} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import type {CharacterPowerResponse, PowerPath} from "@/components/characters/character/powers/types.ts";

export const characterPowersStore =
    defineStore('characterPowers', {
        state: () => {
            return {
                isLoading: true as boolean,
                powers: [] as Array<PowerPath>,
                selectablePowers: [] as Array<PowerPath>,
                currentExperience: 0 as number
            }
        },    
        actions: {
            async getCharacterPowers(characterId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterPowerResponse>(`/characters/${characterId}/powers`)
                    
                this.powers = response.data.powers;
                this.isLoading = false;
            },
            async getSelectableCharacterPowers(characterId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterPowerResponse>(`/characters/${characterId}/pickablepowers`)

                this.selectablePowers = response.data.powers;
                this.isLoading = false;
            },
            addKnowledge: async function (values:CharacterKnowledgeForm, characterId: number, knowledgeId: number): Promise<void> {
                await axios.post(`/characters/${characterId}/knowledges/`, {
                    knowledgeId: knowledgeId,
                    knowledgeLevelId: values.knowledgeLevel,
                    notes: values.notes,
                })
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        toaster.success("Successfully Added Knowledge!");
                    });
            },
            editKnowledge: async function (values:CharacterKnowledgeForm, characterId: number, mappingId: number): Promise<void> {
                await axios.put(`/characters/${characterId}/knowledges/${mappingId}`, {
                    knowledgeLevelId: values.knowledgeLevel,
                    notes: values.notes,
                })
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        toaster.success("Successfully Updated Knowledge!");
                    });
            },
            deleteKnowledge: async function (characterId: number, mappingId: number): Promise<void> {
                await axios.delete(`/characters/${characterId}/knowledges/${mappingId}`)
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        toaster.success("Successfully Deleted Knowledge!");
                    });
            },
        }
    });
