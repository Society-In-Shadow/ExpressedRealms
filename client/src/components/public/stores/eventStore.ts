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
                    name: 'Sioux City Geek Con',
                    location: 'Sioux City, 801 4th St, Sioux City, IA 51101, USA',
                    startDate: new Date(2025, 7, 23),
                    endDate: new Date(2025, 7, 25),
                    conWebsiteName: 'Sioux City Table Top RPG',
                    conWebsiteUrl: 'https://siouxcitytabletopg.wixsite.com/my-site',
                    staff: [
                        {
                            name: 'Staffer',
                            bio: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas vehicula' +
                                ' viverra. Aenean sagittis pellentesque ultrices. Aenean eu sollicitudin odio, vel faucibus ' +
                                'dolor. Proin vitae turpis vulputate, lacinia justo ut, elementum ligula. Cras in nulla ' +
                                'turpis. Maecenas id risus bibendum, molestie felis id, viverra nulla. Cras velit sem, ' +
                                'condimentum ac facilisis sit amet, placerat nec velit. Morbi urna purus, pharetra vel ' +
                                'placerat vel, vehicula non elit. Phasellus maximus justo sit amet accumsan pretium. ' +
                                'Proin convallis nisi cursus pretium volutpat. Donec et metus et eros viverra auctor ' +
                                'non ut lacus. Pellentesque vel erat vitae ligula scelerisque gravida.'
                        }
                    ]
                }]
            },
        }
    });
