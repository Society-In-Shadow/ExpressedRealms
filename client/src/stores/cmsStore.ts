import {defineStore} from 'pinia'
import axios from "axios";
import type {ExpressionMenuItem, ExpressionMenuResponse} from "@/components/navbar/navMenuItems/types.ts";

export const cmsStore = 
defineStore('cmsStore', {
    state: () => {
        return {
            rulebookItems: [] as ExpressionMenuItem[],
            worldBackgroundItems: [] as ExpressionMenuItem[],
            expressionItems: [] as ExpressionMenuItem[],
            canEdit: false as boolean,
            isDoneLoading: false as boolean,
        }
    },
    actions: {
        async getCmsInformation(){
            if(this.isDoneLoading) return;
            
            const expressionData = await axios.get<ExpressionMenuResponse>("/navMenu/content");

            this.canEdit = expressionData.data.canEdit;
            this.expressionItems = expressionData.data.menuItems.filter(x => x.expressionTypeId == 1);
            this.rulebookItems = expressionData.data.menuItems.filter(x => x.expressionTypeId == 13);
            this.worldBackgroundItems = expressionData.data.menuItems.filter(x => x.expressionTypeId == 14);
            
            this.isDoneLoading = true;
        }
    }
});
