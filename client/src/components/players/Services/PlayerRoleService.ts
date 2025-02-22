import axios from "axios";
import toaster from "@/services/Toasters";

export function fetchData(userId: string) {
    return axios.get(`/admin/user/${userId}/roles`);
}

export function updateRole(userId: string, roleName: string, isEnabled: boolean) {
    axios.put(`/admin/user/${userId}/role`,
        {
            userId: userId,
            roleName: roleName,
            isEnabled: isEnabled
        })
        .then((response) => {
            toaster.success(`Successfully Updated Policy!`);
        });
}