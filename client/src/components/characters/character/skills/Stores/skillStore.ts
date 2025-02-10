import { defineStore } from 'pinia'

export const skillStore =
    defineStore('skills', {
        state: () => {
            return {
                showExperience: false as Boolean
            }
        },
    });
