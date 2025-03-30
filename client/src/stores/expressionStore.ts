import { defineStore } from 'pinia'
import axios from "axios";

export const expressionStore = 
defineStore('expression', {
    state: () => {
        return {
            sections: [
                {
                    id: 1,
                    subSections: [
                        { id: 2, subSections: []},
                        { id: 3, subSections: []},
                        { id: 4, subSections: []}
                    ]
                },
                {
                    id: 5,
                    subSections: []
                },
                {
                    id: 6,
                    subSections: [{id: 7}]
                },
                {
                    id: 8,
                    subSections: [{id: 9,}]
                }
            ] as any[],
            currentExpressionId: 0 as Number,
            currentExpressionName: "" as String,
            isLoading: false as Boolean,
            canEdit: false as Boolean,
        }
    },
    actions: {
        async getExpressionSections(name: String){
            this.isLoading = true;
            return await axios.get(`/expressionSubSections/${name}`)
                .then(async (json) => {
                    this.sections = json.data.expressionSections;
                    this.currentExpressionId = json.data.expressionId;
                    this.isLoading = false;
                    this.canEdit = json.data.canEditPolicy
                });
        }
    }
});
