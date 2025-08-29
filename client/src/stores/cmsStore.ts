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
        }
    },
    actions: {
        async getCmsInformation(){
            const expressionData = await axios.get<ExpressionMenuResponse>("/navMenu/content/1");

            this.canEdit = expressionData.data.canEdit;
            this.expressionItems = expressionData.data.menuItems;
            
            const rulebookData = await axios.get<ExpressionMenuResponse>("/navMenu/content/13");
            
            this.canEdit = rulebookData.data.canEdit;
            this.rulebookItems = rulebookData.data.menuItems;
            
            const worldBackgroundData = await axios.get<ExpressionMenuResponse>("/navMenu/content/14");
            
            this.canEdit = worldBackgroundData.data.canEdit;
            this.worldBackgroundItems = worldBackgroundData.data.menuItems;
            
        }
    }
});
