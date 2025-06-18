import { defineStore } from 'pinia'
import axios from "axios";
import {UserRoles, userStore} from "@/stores/userStore";

const userInfo = userStore();

export const expressionStore = 
defineStore('expression', {
    state: () => {
        return {
            sections: [] as any[],
            currentExpressionId: 0 as number,
            currentExpressionName: "" as string,
            isDoneLoading: false as boolean,
            canEdit: userInfo.hasUserRole(UserRoles.PowerManagementRole),
            showPowersTab: false as boolean,
            isSpecialExpression: false as boolean
        }
    },
    actions: {
        async getExpressionId(name: string){
            await axios.get(`/expression/getByName/${name}`)
                .then(async (json) => {
                    this.currentExpressionId = json.data.id;
                    this.showPowersTab = json.data.showPowersTab;
                })
        },
        async getExpressionSections(){
            this.isDoneLoading = false;
            return await axios.get(`/expressionSubSections/${this.currentExpressionId}`)
                .then(async (json) => {
                    this.sections = json.data.expressionSections;
                    this.isDoneLoading = true;
                });
        }
    }
});
