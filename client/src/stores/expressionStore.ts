import { defineStore } from 'pinia'
import axios from "axios";

export const expressionStore = 
defineStore('expression', {
    state: () => {
        return {
            sections: [] as any[],
            currentExpressionId: 0 as Number,
            currentExpressionName: "" as String,
            isDoneLoading: false as Boolean,
            canEdit: false as Boolean,
            showPowersTab: false as Boolean,
        }
    },
    actions: {
        async getExpressionSections(name: String){
            this.isDoneLoading = false;
            return await axios.get(`/expressionSubSections/${name}`)
                .then(async (json) => {
                    this.sections = json.data.expressionSections;
                    this.currentExpressionId = json.data.expressionId;
                    this.isDoneLoading = true;
                    this.canEdit = json.data.canEditPolicy
                    this.showPowersTab = json.data.showPowersTab
                });
        }
    }
});
