import { defineStore } from 'pinia'

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
});
