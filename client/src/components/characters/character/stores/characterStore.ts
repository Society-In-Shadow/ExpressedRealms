import {defineStore} from 'pinia'
import axios from "axios";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";

const userInfo = userStore();
export const characterStore =
    defineStore('character', {
        state: () => {
            return {
                isLoading: true as boolean,
                name: '' as string,
                background: '' as string,
                expression: '' as string,
                expressionId: 0 as number,
                isPrimaryCharacter: false as boolean,
                isInCharacterCreation: false as boolean,
                isOwner: false as boolean,
                factions: [] as any[],
                faction: {} as any,
            }
        },
        actions: {
            async getCharacterDetails(characterId: Number){
                this.isLoading = false;
                return await axios.get(`/characters/${characterId}`)
                    .then(async (response) => {
                        this.name = response.data.name;
                        this.background = response.data.background;
                        this.expression = response.data.expression;
                        this.expressionId = response.data.expressionId;
                        this.isPrimaryCharacter = response.data.isPrimaryCharacter;
                        this.isInCharacterCreation = response.data.isInCharacterCreation;
                        this.isOwner = response.data.isOwner;
                        
                        if(await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown)){
                            await axios.get(`/characters/${characterId}/factionOptions`)
                                .then((factionResponse) => {
                                    this.factions = factionResponse.data;

                                    this.faction = factionResponse.data.find(x => x.id == response.data.factionId);
                                    this.isLoading = false;
                                })
                        }

                    })
            }
        }
    });
