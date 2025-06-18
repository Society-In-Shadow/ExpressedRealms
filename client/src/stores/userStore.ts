import { defineStore } from 'pinia'
import axios from "axios";

export const UserRoles = {
    ExpressionEditor: "ExpressionEditorRole",
    UserManagementRole: "UserManagementRole",
    PowerManagementRole: "PowerManagementRole",
} as const;

export const FeatureFlags = {
    ShowsPowerTab: "show-power-tab",
    ShowRuleBook: "show-rule-book-nav",
    ShowTreasureTales: "show-treasured-tales-nav",
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
            userFeatureFlags: [] as string[]
        }
    },
    actions: {
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
                    console.log("feature flags laoded");
                })
        },
        hasUserRole(role: UserRole): boolean {
            return this.userRoles.includes(role);
        },
        hasFeatureFlag(featureFlag: FeatureFlag): boolean {
            return this.userFeatureFlags.includes(featureFlag);
        },
    }
});
