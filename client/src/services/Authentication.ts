import axios from "axios";
import Router from "@/router";
import {userStore} from "@/stores/userStore";

export async function logOff() {
    const userInfo = userStore();
    axios.post('/auth/logoff').then(() => {
        userInfo.$reset();
        Router.push('login');
    });
}

export function resetEmailConfirmation() {
    const userInfo = userStore();
    userInfo.hasConfirmedEmail = false;
}

export function isLoggedIn() {
    document.cookie = ".AspNetCore.Identity.Bearer=1;path=/;domain=" + import.meta.env.VITE_COOKIE_DOMAIN + ";";
    if(document.cookie.indexOf(".AspNetCore.Identity.Bearer") >= 0){
        document.cookie = ".AspNetCore.Identity.Bearer=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        return false
    }
    return true;
}

export async function updateUserStoreWithPlayerInfo() {
    const userInfo = userStore();
    await axios.get('/player/playerName')
        .then ((response) => {
            if(response.data.name){
                userInfo.isPlayerSetup = true;
                userInfo.name = response.data.name;
            }
        });
}

export async function updateUserStoreWithEmailInfo() {
    const userInfo = userStore();
    await axios.get("/auth/manage/info")
        .then(response => {
            userInfo.hasConfirmedEmail = response.data.isEmailConfirmed;
            userInfo.userEmail = response.data.email;
        });
}
