import {defineStore} from 'pinia'
import axios from "axios";

export const characterStore =
    defineStore('character', {
        state: () => {
            return {
                isLoading: true as Boolean,
                name: '' as String,
                background: '' as String,
                expression: '' as String,
                expressionId: 0 as number,
                isPrimaryCharacter: false as Boolean,
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
                        await axios.get(`/characters/${characterId}/factionOptions`)
                            .then((factionResponse) => {
                                this.factions = factionResponse.data;

                                this.faction = factionResponse.data.find(x => x.id == response.data.factionId);
                                this.isLoading = false;
                            })
                    })
            }
        }
    });
