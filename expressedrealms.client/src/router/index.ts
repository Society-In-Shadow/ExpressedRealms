import { createRouter, createWebHistory } from 'vue-router';

const routes = [
    {
        path: "/login",
        name: "Login",
        component: () => import("./../components/Login/Login.vue"),
    },
    {
        path: "/createAccount",
        name: "createAccount",
        component: () => import("./../components/Login/CreateAccount.vue"),
    },
    {
        path: "/forgotPassword",
        name: "resetPassword",
        component: () => import("./../components/Login/ResetPassword.vue"),
    },
    {
        path: "/helloworld",
        name: "helloworld",
        component: () => import("./../components/HelloWorld.vue"),
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach(async (to, from) => {

    let isAuthenticated = false;
    fetch('/api/isLoggedIn')
        .then(r => r.json())
        .then(json => {
            isAuthenticated = json as boolean;
            return;
        });
    
    let anonymouseEndpoints = ['Login', 'createAccount', 'resetPassword']
    let routeName:string = to.name as string;
    
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