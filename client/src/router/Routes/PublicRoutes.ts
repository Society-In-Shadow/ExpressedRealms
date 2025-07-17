import PublicLayout from "@/components/public/PublicLayout.vue";

export const PublicRoutes = {
    path: '/',
    component: PublicLayout,
    children: [
        {
            path: "about",
            name: "About",
            component: () => import("./../../components/public/About.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "",
            name: "home",
            component: () => import("./../../components/public/Home.vue"),
            meta: { isAnonymous: true },
        },
        {
            path: "expressions",
            name: "expressions",
            component: () => import("./../../components/public/Expressions.vue"),
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
    ]
}