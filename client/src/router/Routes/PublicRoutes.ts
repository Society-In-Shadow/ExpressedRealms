import PublicLayout from "@/components/public/PublicLayout.vue";

export const PublicRoutes = {
    path: '/',
    component: PublicLayout,
    children: [
        {
            path: "about",
            name: "About",
            component: () => import("./../../components/public/AboutUs.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "",
            name: "home",
            component: () => import("./../../components/public/HomePage.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "expressions",
            name: "expressions",
            component: () => import("./../../components/public/PublicExpressions.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "contact-us",
            name: "contactus",
            component: () => import("./../../components/public/ContactUs.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "upcoming-events",
            name: "upcoming-events",
            component: () => import("./../../components/public/UpcomingEvents.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "terms-of-service",
            name: "terms-of-service",
            component: () => import("./../../components/public/legal/TermsOfService.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "privacy-policy",
            name: "privacy-policy",
            component: () => import("./../../components/public/legal/PrivacyPolicy.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "code-of-conduct",
            name: "code-of-conduct",
            component: () => import("./../../components/public/legal/CodeOfConduct.vue"),
            meta: { isAnonymous: true },
        },
        
    ]
}
