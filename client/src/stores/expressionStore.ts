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
        async getExpressionId(route: object){
            if(route.meta.isCMS){
                console.log("is cms");
                await axios.get(`/expression/getCmsByName/${route.meta.id}`)
                    .then(async (json) => {
                        this.currentExpressionId = json.data.id;
                    })
            }
            else{
                await axios.get(`/expression/getByName/${route.params.name}`)
                    .then(async (json) => {
                        this.currentExpressionId = json.data.id;
                        this.showPowersTab = json.data.showPowersTab;
                    })
            }
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
