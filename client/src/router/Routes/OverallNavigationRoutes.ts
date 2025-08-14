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
            path: "/characters/add",
            name: "addCharacter",
            component: () => import("./../../components/characters/character/AddCharacter.vue")
        },
        {
            path: "/characters/:id",
            name: "editCharacter",
            component: () => import("./../../components/characters/character/EditCharacter.vue")
        },
        {
            path: "/rulebook",
            name: "rulebook",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 2 },
        },
        {
            path: "/treasuredtales",
            name: "treasuredtales",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 3 },
        },
        {
            path: "/adversaries",
            name: "adversaries",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 4 },
        },
        {
            path: "/factions",
            name: "factions",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 5 },
        },
        {
            path: "/society",
            name: "society",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 6 },
        },
        {
            path: "/characterSetup",
            name: "characterSetup",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 7 },
        },
        {
            path: "/knowledges",
            name: "knowledges",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 8 },
        },
        {
            path: "/blessings",
            name: "blessings",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 9 },
        },
        {
            path: "/combat",
            name: "combat",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 10 },
        },
        {
            path: "/inventory",
            name: "inventory",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 12 },
        },
        {
            path: "/characterQuickStart",
            name: "characterQuickStart",
            component: () => import("./../../components/expressions/CmsBase.vue"),
            meta: { isCMS: true, id: 11 },
        },
        {
            path: "/expressions/:name",
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
