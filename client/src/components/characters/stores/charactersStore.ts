import {defineStore} from 'pinia'
import axios from "axios";

export const charactersStore = 
defineStore('charactersStore', {
    state: () => {
        return {
            characters: [] as any[],
        }
    },
    actions: {
        async getCharacters(){
            await axios.get('/characters')
                .then((json) => {
                    this.characters = json.data;
                });
        },
        async deleteCharacter(id: number){
            await axios.delete(`/characters/${id}`)
                .then(async () => {
                    await this.getCharacters();
                });
        }
    }
});
