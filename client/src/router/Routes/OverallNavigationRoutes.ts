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
            component: () => import("./../../components/expressions/CmsBase.vue")
        },
        {
            path: "/treasuredtales",
            name: "treasuredtales",
            component: () => import("./../../components/expressions/CmsBase.vue")
        },
        {
            path: "/expressions/:name",
            name: "viewExpression",
            component: () => import("./../../components/expressions/ExpressionBase.vue")
        }
    ]
}
