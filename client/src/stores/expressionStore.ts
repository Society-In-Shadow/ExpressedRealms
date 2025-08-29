import {defineStore} from 'pinia'
import axios from "axios";
import {UserRoles, userStore} from "@/stores/userStore";
import {cmsStore} from "@/stores/cmsStore.ts";

const userInfo = userStore();
const cmsInfo = cmsStore();
export const expressionStore = 
defineStore('expression', {
    state: () => {
        return {
            sections: [] as any[],
            currentExpressionId: 0 as number,
            currentExpressionName: "" as string,
            isDoneLoading: false as boolean,
            canEdit: userInfo.hasUserRole(UserRoles.PowerManagementRole),
            isSpecialExpression: false as boolean
        }
    },
    actions: {
        async getExpressionId(route: object){
            if(route.meta.isRuleBook){
                await cmsInfo.getCmsInformation();
                this.currentExpressionId = cmsInfo.rulebookItems.filter(x => x.slug == route.params.slug)[0].id;
            } else if(route.meta.isWorldBackground){
                await cmsInfo.getCmsInformation();
                this.currentExpressionId = cmsInfo.worldBackgroundItems.filter(x => x.slug == route.params.slug)[0].id;
            }
            else{
                await axios.get(`/expression/getByName/${route.params.name}`)
                    .then(async (json) => {
                        this.currentExpressionId = json.data.id;
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
