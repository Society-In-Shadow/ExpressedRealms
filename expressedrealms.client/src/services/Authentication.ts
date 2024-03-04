import axios from "axios";
import Router from "@/router";
import {userStore} from "@/stores/userStore";

export async function logOff() {
    const userInfo = userStore();
    axios.post('api/auth/logoff').then(() => {
        userInfo.userEmail = "";
        userInfo.hasConfirmedEmail = false;
        Router.push('login');
    });
}

export function resetEmailConfirmation() {
    const userInfo = userStore();
    userInfo.hasConfirmedEmail = false;
}

export function isLoggedIn() {
    document.cookie = ".AspNetCore.Identity.Bearer=1";
    if(document.cookie.indexOf(".AspNetCore.Identity.Bearer") >= 0){
        document.cookie = ".AspNetCore.Identity.Bearer=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        return false
    }
    return true;
}