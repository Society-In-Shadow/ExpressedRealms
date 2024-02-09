import { createRouter, createWebHistory } from 'vue-router';
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";
import Layout from "@/components/LoggedInLayout.vue";

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

    let isAuthenticated = false;
    await fetch('/api/auth/isLoggedIn')
        .then(r => r.json())
        .then(json => {
            isAuthenticated = json as boolean;
        });
    
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
})

export default router