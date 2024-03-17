import axios from "axios";
import Router from "@/router";
import {userStore} from "@/stores/userStore";

export async function logOff() {
    const userInfo = userStore();
    axios.post('api/auth/logoff').then(() => {
        userInfo.$reset();
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

export async function updateUserStoreWithPlayerInfo() {
    const userInfo = userStore();
    await axios.get('/api/player/isSetup')
        .then ((response) => {
            if(response.data){
                userInfo.isPlayerSetup = true;
                userInfo.name = response.data;
            }
        });
}

export async function updateUserStoreWithEmailInfo() {
    const userInfo = userStore();
    await axios.get("/api/auth/manage/info")
        .then(response => {
            userInfo.hasConfirmedEmail = response.data.isEmailConfirmed;
            userInfo.userEmail = response.data.email;
        });
}