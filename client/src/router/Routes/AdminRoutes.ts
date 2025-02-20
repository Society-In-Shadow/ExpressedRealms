import Layout from "@/components/LoggedInLayout.vue";

export const AdminRoutes = {
    path: '/admin',
    component: Layout,
    children: [
        {
            path: "players",
            name: "viewPlayers",
            component: () => import("./../../components/players/PlayerList.vue"),
            meta: { requiredRole: "UserManagementRole" },
        }
    ]
}
