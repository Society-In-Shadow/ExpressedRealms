import { defineStore } from 'pinia'
import axios from "axios";

export const userStore = 
defineStore('user', {
    state: () => {
        return {
            userEmail: "" as String,
            name: "" as String,
            hasConfirmedEmail: false as Boolean,
            isPlayerSetup: false as Boolean,
            userRoles: [] as string[]
        }
    },
    actions: {
        async updateUserRoles(){
            await axios.get("/navMenu/permissions")
                .then(response => {
                    this.userRoles = response.data.roles;
                })
        }
    }
});
