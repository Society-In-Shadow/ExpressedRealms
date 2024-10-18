import { defineStore } from 'pinia'

export const expressionStore = 
defineStore('expression', {
    state: () => {
        return {
            currentExpressionId: 0 as Number,
            currentExpressionName: "" as String
        }
    },
});
