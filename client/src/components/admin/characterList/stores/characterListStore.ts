import {defineStore} from 'pinia'
import axios from "axios";
import type {CharacterListResponse, PrimaryCharacter} from "@/components/admin/characterList/types.ts";

export const adminCharacterListStore =
    defineStore('adminCharacterList', {
        state: () => {
            return {
                primaryCharacters: [] as Array<PrimaryCharacter>,
                filteredCharacters: [] as Array<PrimaryCharacter>,
            }
        },
        actions: {
            async fetchCharacters() {
                await axios.get<CharacterListResponse>('/admin/characters')
                    .then((response) => {
                        this.primaryCharacters = response.data.characters;
                        this.filteredCharacters = response.data.characters;
                    });
            },
            async updateCharacterXp(characterId: number, xp: number) {
                await axios.put(`/admin/characters/${characterId}/updateXp`, {xp: xp})
                    .then(() => {
                        this.fetchCharacters();
                    });
            },
            filterCharacters(query: string) {
                const lowercasedQuery = query.toLowerCase().trim();

                if (!lowercasedQuery) {
                    this.filteredCharacters = this.primaryCharacters;
                } else {
                    this.filteredCharacters = this.primaryCharacters.filter((character) =>
                        character.name.toLowerCase().includes(lowercasedQuery) ||
                        character.playerName.toLowerCase().includes(lowercasedQuery)
                    );
                }
            },
        }
        
    });
