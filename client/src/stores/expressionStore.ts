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
            await cmsInfo.getCmsInformation();
            if(route.meta.isRuleBook){
                this.currentExpressionId = cmsInfo.rulebookItems.filter(x => x.slug == route.params.slug)[0].id;
            } else if(route.meta.isWorldBackground){
                this.currentExpressionId = cmsInfo.worldBackgroundItems.filter(x => x.slug == route.params.slug)[0].id;
            }
            else{
                this.currentExpressionId = cmsInfo.expressionItems.filter(x => x.slug == route.params.slug)[0].id;
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
