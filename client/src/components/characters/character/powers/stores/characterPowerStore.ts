import {defineStore} from 'pinia'
import axios from "axios";
import toaster from "@/services/Toasters";
import type {
    CharacterPowerOptionsResponse,
    CharacterPowerResponse,
    PowerPath
} from "@/components/characters/character/powers/types.ts";
import type {CharacterPowerForm} from "@/components/characters/character/powers/validations/powerValidations.ts";

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
            async getPowerOptions(characterId: number, powerId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterPowerOptionsResponse>(`/characters/${characterId}/powers/${powerId}/options`)

                return response.data
            },
            addPower: async function (values:CharacterPowerForm, characterId: number, powerId: number): Promise<void> {
                await axios.post(`/characters/${characterId}/powers/`, {
                    powerId: powerId,
                    notes: values.notes,
                })
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        await this.getSelectableCharacterPowers(characterId);
                        toaster.success("Successfully Added Power!");
                    });
            },
            editPower: async function (values:CharacterPowerForm, characterId: number, powerId: number): Promise<void> {
                await axios.put(`/characters/${characterId}/powers/${powerId}`, {
                    notes: values.notes,
                })
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        await this.getSelectableCharacterPowers(characterId);
                        toaster.success("Successfully Updated Power!");
                    });
            },
            deletePower: async function (characterId: number, powerId: number): Promise<void> {
                await axios.delete(`/characters/${characterId}/powers/${powerId}`)
                    .then(async () => {
                        await this.getCharacterPowers(characterId);
                        await this.getSelectableCharacterPowers(characterId);
                        toaster.success("Successfully Deleted Power!");
                    });
            },
        }
    });
