import axios from "axios";
import Router from "@/router";
import {userStore} from "@/stores/userStore";

export async function logOff() {
    let userInfo = userStore();
    axios.post('api/auth/logoff').then(() => {
        userInfo.userEmail = "";
        userInfo.hasConfirmedEmail = false;
        Router.push('login');
    });
}
