import { createRouter, createWebHistory } from 'vue-router';
import {userStore} from "@/stores/userStore";
import {updateUserStoreWithEmailInfo, isLoggedIn, updateUserStoreWithPlayerInfo} from "@/services/Authentication";
import {UserRoutes} from "@/router/Routes/UserRoutes";
import {AdminRoutes} from "@/router/Routes/AdminRoutes";
import {OverallRoutes} from "@/router/Routes/OverallNavigationRoutes";
import axios from "axios";

export const routes = [
    UserRoutes,
    OverallRoutes,
    AdminRoutes
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.isReady().then(() => {
    const userInfo = userStore();
    axios.get("/navMenu/permissions")
        .then(response => {
            userInfo.userRoles = response.data.roles;
        })
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

        if (to.meta.requiredRole && !userInfo.userRoles.includes(to.meta.requiredRole)) {
            return {name: 'characters'};
        }

        // Check to make sure that they have a confirmed email
        if(!userInfo.hasConfirmedEmail && routeName != 'pleaseConfirmEmail' && routeName != 'confirmAccount'){
            await updateUserStoreWithEmailInfo();
            if(!userInfo.hasConfirmedEmail){
                return { name: 'pleaseConfirmEmail' }
            }
        }
        
        // Prevent infinite loop with isPlayerSetup
        if(!userInfo.hasConfirmedEmail && routeName == 'pleaseConfirmEmail'){
            return;
        }

        // Check to see if they setup their player info yet
        if(!userInfo.isPlayerSetup && routeName != 'setupProfile'){
            await updateUserStoreWithPlayerInfo();

            if(!userInfo.isPlayerSetup){
                return { name: 'setupProfile'}
            }
        }
        
        // If they are on this page, and refresh it after confirming, redirect them to the characters page
        if(userInfo.hasConfirmedEmail && routeName == 'pleaseConfirmEmail'){
            return { name: 'characters' };
        }

        // if they are on the login page, redirect them to the characters page
        // Also, if on url root, redirect to characters page
        if(routeName == 'Login' || !routeName)
            return { name: 'characters' };
    }
    
});

export default router
