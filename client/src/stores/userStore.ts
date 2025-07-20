import { defineStore } from 'pinia'
import axios from "axios";
import {isLoggedIn, updateUserStoreWithEmailInfo, updateUserStoreWithPlayerInfo} from "@/services/Authentication";

export const UserRoles = {
    ExpressionEditor: "ExpressionEditorRole",
    UserManagementRole: "UserManagementRole",
    PowerManagementRole: "PowerManagementRole",
    KnowledgeManagementRole: "KnowledgeManagementRole"
} as const;

export const FeatureFlags = {
    ShowsPowerTab: "show-power-tab",
    ShowRuleBook: "show-rule-book-nav",
    ShowTreasureTales: "show-treasured-tales-nav",
    ShowKnowledges: "show-knowledges",
    ShowMarketing: "show-marketing",
    ShowMarketingContactUs: "show-marketing-contact-us",
} as const;

export type UserRole = (typeof UserRoles)[keyof typeof UserRoles];
export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags];

export const userStore = 
defineStore('user', {
    state: () => {
        return {
            userEmail: "" as string, 
            name: "" as string,
            hasConfirmedEmail: false as boolean,
            isPlayerSetup: false as boolean,
            userRoles: [] as string[],
            userFeatureFlags: [] as string[],
            loadedUserInfo: false as boolean,
        }
    },
    actions: { 
        isLoggedIn() {
            return isLoggedIn();
        },
        async updateUserRoles(){
            await axios.get("/navMenu/permissions")
                .then(response => {
                    this.userRoles = response.data.roles;
                })
        },
        async updateUserFeatureFlags(){
            return await axios.get("/navMenu/featureFlags")
                .then(response => {
                    this.userFeatureFlags = response.data.featureFlags;
                })
        },
        hasUserRole(role: UserRole): boolean {
            return this.userRoles.includes(role);
        },
        async hasFeatureFlag(featureFlag: FeatureFlag): Promise<boolean> {
            await this.updateUserFeatureFlags();
            return this.userFeatureFlags.includes(featureFlag);
        },
        async getUserInfo(){
            await updateUserStoreWithPlayerInfo();
            await updateUserStoreWithEmailInfo();
        },
        hasStepsToComplete(): boolean {
            
            // User needs email confirmed
            if(!this.hasConfirmedEmail){
                return true;
            }
            
            // User Needs profile setup
            return !this.isPlayerSetup;
            
        },
        userNextStepUrl(nextStep: string): string {

            // confirm account should stay on the same page if the email hasn't been confirmed
            if(nextStep == "confirmAccount" && !this.hasConfirmedEmail)
                return "confirmAccount";
            
            if(!this.hasConfirmedEmail){
                return "pleaseConfirmEmail";
            }
            
            return "setupProfile";
            
        }
    }
});
