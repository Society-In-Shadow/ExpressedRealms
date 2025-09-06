import {defineStore} from 'pinia'
import axios from "axios";
import type {
    CharacterKnowledge,
    CharacterKnowledgeResponse,
    KnowledgeOptionResponse,
    KnowledgeOptions
} from "@/components/characters/character/knowledges/types";
import toaster from "@/services/Toasters";
import type {
    CharacterKnowledgeForm
} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import type {
    SpecializationForm
} from "@/components/characters/character/knowledges/validations/specializationValidations";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";

const experienceInfo = experienceStore();

export const characterKnowledgeStore =
    defineStore('characterKnowledge', {
        state: () => {
            return {
                isLoading: true as boolean,
                isLoadingLevels: true as boolean,
                knowledges: [] as Array<CharacterKnowledge>,
                knowledgeLevels: [] as Array<KnowledgeOptions>,
                currentExperience: 0 as number,
                activeKnowledgeId: 0 as number,
            }
        },    
        actions: {
            async getCharacterKnowledges(characterId: number){
                this.isLoading = true;
                const response = await axios.get<CharacterKnowledgeResponse>(`/characters/${characterId}/knowledges`)
                    
                this.knowledges = response.data.knowledges;
                this.isLoading = false;
            },
            async getKnowledgeLevels(characterId: number){
                this.isLoadingLevels = true;
                const response = await axios.get<KnowledgeOptionResponse>(`/characters/${characterId}/knowledges/options`)
                this.knowledgeLevels = response.data.knowledgeLevels
                this.currentExperience = response.data.availableExperience;
                this.isLoadingLevels = false;
            },
            addKnowledge: async function (values:CharacterKnowledgeForm, characterId: number, knowledgeId: number): Promise<void> {
                await axios.post(`/characters/${characterId}/knowledges/`, {
                    knowledgeId: knowledgeId,
                    knowledgeLevelId: values.knowledgeLevel,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Added Knowledge!");
                    });
            },
            editKnowledge: async function (values:CharacterKnowledgeForm, characterId: number, mappingId: number): Promise<void> {
                await axios.put(`/characters/${characterId}/knowledges/${mappingId}`, {
                    knowledgeLevelId: values.knowledgeLevel,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Updated Knowledge!");
                    });
            },
            deleteKnowledge: async function (characterId: number, mappingId: number): Promise<void> {
                await axios.delete(`/characters/${characterId}/knowledges/${mappingId}`)
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Deleted Knowledge!");
                    });
            },
            addSpecialization: async function (values:SpecializationForm, characterId: number, mappingId: number): Promise<void> {
                await axios.post(`/characters/${characterId}/knowledges/${mappingId}/specialization`, {
                    name: values.name,
                    description: values.description,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Added Specialization!");
                    });
            },
            editSpecialization: async function (values:SpecializationForm, characterId: number, mappingId: number, specializationId: number): Promise<void> {
                await axios.put(`/characters/${characterId}/knowledges/${mappingId}/specialization/${specializationId}`, {
                    name: values.name,
                    description: values.description,
                    notes: values.notes,
                })
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Updated Specialization!");
                    });
            },
            deleteSpecialization: async function (characterId: number, mappingId: number, specializationNumber:number): Promise<void> {
                await axios.delete(`/characters/${characterId}/knowledges/${mappingId}/specialization/${specializationNumber}`)
                    .then(async () => {
                        await experienceInfo.updateExperience(characterId);
                        await this.getCharacterKnowledges(characterId);
                        toaster.success("Successfully Deleted Specialization!");
                    });
            },
        }
    });
