import { ref } from 'vue'
import { defineStore } from 'pinia'

export const userStore = defineStore('user', () => {
    const userEmail = ref("");
    const hasConfirmedEmail = ref(false);
    const isPlayerSetup = ref(false);

    return { userEmail, hasConfirmedEmail, isPlayerSetup }
}, {
    persist: true
})