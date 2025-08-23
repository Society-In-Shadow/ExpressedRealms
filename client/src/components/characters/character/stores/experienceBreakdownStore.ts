import {defineStore} from "pinia";
import axios from "axios";

export const experienceStore =
    defineStore('experienceStore', {
        state: () => {
            return {
                isLoading: true as Boolean,
                experienceBreakdown: {} as ExperienceBreakdownResponse,
                showAllExperience: true as Boolean,
            }
        },
        actions: {
            async updateExperience(characterId: Number){
                this.isLoading = true;
                await axios.get(`/characters/${characterId}/overallexperience`)
                    .then((response) => {
                        this.isLoading = false;
                        this.experienceBreakdown = response.data;
                    })
                },
            getCharacterLevel(): number{
                var total =this.experienceBreakdown.total;

                total = total - this.experienceBreakdown.setupTotal;
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
