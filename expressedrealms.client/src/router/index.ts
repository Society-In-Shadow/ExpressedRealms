import { createRouter, createWebHistory } from 'vue-router';
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";
import Layout from "@/components/LoggedInLayout.vue";
import axios from "axios";
import {userStore} from "@/stores/userStore";
import {isLoggedIn} from "@/services/Authentication";

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
            {
                path: "/userProfile",
                name: "userProfile",
                component: () => import("./../components/profile/UserProfileBase.vue")
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach(async (to) => {

    const loggedIn = isLoggedIn();
    const anonymousEndpoints = ['Login', 'createAccount', 'forgotPassword', 'resetPassword', 'confirmAccount']
    const routeName:string = to.name as string;
    const canCauseInfiniteRedirects = anonymousEndpoints.includes(routeName)
    
    if (!loggedIn && !canCauseInfiniteRedirects) {
        return  { name: 'Login' };
    }
    
    if(loggedIn){
        
        const userInfo = userStore();
        
        // Check to make sure that they have a confirmed email
        if(!userInfo.hasConfirmedEmail && routeName != 'pleaseConfirmEmail' && routeName != 'confirmAccount'){
            await axios.get("/api/auth/manage/info")
                .then(response => {
                    userInfo.hasConfirmedEmail = response.data.isEmailConfirmed;
                    userInfo.userEmail = response.data.email;
                });
            if(!userInfo.hasConfirmedEmail){
                return { name: 'pleaseConfirmEmail' }
            }
        }

        // If they are on this page, and refresh it after confirming, redirect them to the characters page
        if(userInfo.hasConfirmedEmail && routeName == 'pleaseConfirmEmail'){
            return { name: 'characters' };
        }

        // if they are on the login page, redirect them to the characters page
        if(routeName == 'Login')
            return { name: 'characters' };
    }
    
})

export default router