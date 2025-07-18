import {defineStore} from "pinia";
import type {EventDetails} from "@/components/public/types";

export const eventStore =
    defineStore(`events`, {
        state: () => {
            return {
                events: [] as EventDetails[]
            }
        },
        actions: {
            async getEvents(){
                
                this.events = [{
                    id: 1,
                    name: 'Sioux City Geek Con',
                    location: '801 4th St Sioux City, Iowa',
                    startDate: new Date(2025, 7, 23),
                    endDate: new Date(2025, 7, 25),
                    conWebsiteName: 'Sioux City Table Top RPG',
                    conWebsiteUrl: 'https://siouxcitytabletopg.wixsite.com/my-site',
                }]
            },
        }
    });
