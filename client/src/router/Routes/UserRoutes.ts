import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";

export const UserRoutes = {
    path: '/auth',
    component: LoginBasePlate,
    children: [
        {
            path: "/login",
            name: "Login",
            component: () => import("./../../components/Login/UserLogin.vue"),
            meta: { isAnonymous: true, isUserSetup: true },
        },
        {
            path: "/createAccount",
            name: "createAccount",
            component: () => import("./../../components/Login/CreateAccount.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "/forgotPassword",
            name: "forgotPassword",
            component: () => import("./../../components/Login/ForgotPassword.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "/resetPassword",
            name: "resetPassword",
            component: () => import("./../../components/Login/ResetPassword.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "/confirmAccount",
            name: "confirmAccount",
            component: () => import("./../../components/Login/ConfirmEmail.vue"),
            meta: { isAnonymous: true, isUserSetup: true },
        },
        {
            path: "/pleaseConfirmEmail",
            name: "pleaseConfirmEmail",
            component: () => import("./../../components/Login/PleaseConfirmEmail.vue"),
            meta: { isUserSetup: true }
        },
        {
            path: "/setupProfile",
            name: "setupProfile",
            component: () => import("./../../components/Login/AddUserProfile.vue"),
            meta: { isUserSetup: true }
        },
    ]
}
