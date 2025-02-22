import axios from "axios";
import toaster from "@/services/Toasters";

export function fetchUserPolicies(userId: string) {
    return axios.get(`/admin/user/${userId}/roles`);
}

export function updateRole(userId: string, roleName: string, isEnabled: boolean) {
    return axios.put(`/admin/user/${userId}/role`,
        {
            userId: userId,
            roleName: roleName,
            isEnabled: isEnabled
        })
        .then((response) => {
            toaster.success(`Successfully Updated Policy!`);
        });
}
