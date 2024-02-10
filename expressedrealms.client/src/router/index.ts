import { createRouter, createWebHistory } from 'vue-router';
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";
import Layout from "@/components/LoggedInLayout.vue";
import axios from "axios";
import {userStore} from "@/stores/userStore";

const routes = [
    {
        path: '/',
        component: LoginBasePlate,
        children: [
            {
                path: "/login",
                name: "Login",
                component: () => import("./../components/Login/UserLogin.vue"),
            },
            {
                path: "/createAccount",
                name: "createAccount",
                component: () => import("./../components/Login/CreateAccount.vue"),
            },
            {
                path: "/forgotPassword",
                name: "forgotPassword",
                component: () => import("./../components/Login/ForgotPassword.vue"),
            },
            {
                path: "/resetPassword",
                name: "resetPassword",
                component: () => import("./../components/Login/ResetPassword.vue"),
            },
            {
                path: "/confirmAccount",
                name: "confirmAccount",
                component: () => import("./../components/Login/ConfirmEmail.vue"),
            },
            {
                path: "/pleaseConfirmEmail",
                name: "pleaseConfirmEmail",
                component: () => import("./../components/Login/PleaseConfirmEmail.vue"),
            },
        ]
    },
    {
        path: '/expressedRealms',
        component: Layout,
        children: [
            {
                path: "/weatherforecast",
                name: "weatherforecast",
                component: () => import("./../components/WeatherForecast.vue"),
            },
            {
                path: "/characters",
                name: "characters",
                component: () => import("./../components/CharacterList.vue"),
            },
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach(async (to) => {


    let userInfo = userStore();
    let isAuthenticated = isHttpOnlyCookieExists(".AspNetCore.Identity.Bearer");
    const anonymouseEndpoints = ['Login', 'createAccount', 'forgotPassword', 'resetPassword', 'confirmAccount']
    const routeName:string = to.name as string;
    if (
        // make sure the user is authenticated
        !isAuthenticated &&
        // ❗️ Avoid an infinite redirect
        !anonymouseEndpoints.includes(routeName)
    ) {
        // redirect the user to the login page
        return { name: 'Login' }
    }
    
    if(isAuthenticated && !userInfo.hasConfirmedEmail){
        await axios.get("/api/auth/manage/info")
            .then(response => {
                userInfo.hasConfirmedEmail = response.data.isEmailConfirmed;
                userInfo.userEmail = response.data.userEmail;
                if(!userInfo.hasConfirmedEmail){
                    return { name: 'PleaseConfirmEmail' }
                }
            })
    }
})

function isHttpOnlyCookieExists(cookieName:string) {
    // Attempt to access the cookie
    var cookieValue = document.cookie.replace(`/(?:(?:^|.*;\s*)${cookieName}\s*\=\s*([^;]*).*$)|^.*$/`, "$1");

    // Check if the cookie value is not empty or undefined
    return (cookieValue !== "" && typeof cookieValue !== "undefined");
}

export default router