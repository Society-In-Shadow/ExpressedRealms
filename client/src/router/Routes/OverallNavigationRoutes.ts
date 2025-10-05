import Layout from "@/components/LoggedInLayout.vue";

export const OverallRoutes = {
    path: '/expressedRealms',
    component: Layout,
    children: [
        {
            path: "/stonePuller",
            name: "stonePuller",
            component: () => import("./../../components/stonePuller/StonePuller.vue"),
        },
        {
            path: "/characters",
            name: "characters",
            component: () => import("./../../components/characters/CharacterList.vue"),
        },
        {
            path: "/userProfile",
            name: "userProfile",
            component: () => import("./../../components/profile/UserProfileBase.vue")
        },
        {
            path: "/characters/:id",
            name: "characterSheet",
            component: () => import("./../../components/characters/character/CharacterSheet.vue")
        },
        {
            path: "/characters/wizard",
            name: "addWizard",
            component: () => import("./../../components/characters/character/wizard/CharacterWizard.vue")
        },
        {
            path: "/characters/:id/wizard",
            name: "characterWizard",
            component: () => import("./../../components/characters/character/wizard/CharacterWizard.vue")
        },
        {
            path: "/rulebook/:slug",
            name: "rulebook",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isRuleBook: true },
        },
        {
            path: "/worldbackground/:slug",
            name: "worldbackground",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isWorldBackground: true },
        },
        {
            path: "/expressions/:slug",
            name: "viewExpression",
            component: () => import("./../../components/expressions/ExpressionBase.vue")
        },
        {
            path: "/codeofconduct",
            name: "codeofconduct",
            component: () => import("./../../components/public/legal/CodeOfConduct.vue"),
            meta: { isAnonymous: true },
        },
    ]
}
