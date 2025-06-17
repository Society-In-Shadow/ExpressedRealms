import { defineStore } from 'pinia'
import axios from "axios";

export const UserRoles = {
    ExpressionEditor: "ExpressionEditorRole",
    UserManagementRole: "UserManagementRole",
    PowerManagementRole: "PowerManagementRole",
} as const;

export type UserRole = (typeof UserRoles)[keyof typeof UserRoles];

export const userStore = 
defineStore('user', {
    state: () => {
        return {
            userEmail: "" as string,
            name: "" as string,
            hasConfirmedEmail: false as boolean,
            isPlayerSetup: false as boolean,
            userRoles: [] as string[]
        }
    },
    actions: {
        async updateUserRoles(){
            await axios.get("/navMenu/permissions")
                .then(response => {
                    this.userRoles = response.data.roles;
                })
        },
        hasUserRole(role: UserRole): boolean {
            return this.userRoles.includes(role);
        }
    }
});
