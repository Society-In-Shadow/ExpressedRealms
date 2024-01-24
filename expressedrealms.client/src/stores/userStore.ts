import { ref } from 'vue'
import { defineStore } from 'pinia'

export const userStore = defineStore('user', () => {
    const userEmail = ref("");

    return { userEmail }
}, {
    persist: true
})