export interface ExpressionMenuItem {
    id: number;
    name: string;
    shortDescription: string;
    navMenuImage: string;
    statusName: string | null;
    statusId: number;
    slug: string;
}

export interface ExpressionMenuResponse{
    menuItems: Array<ExpressionMenuItem>;
    canEdit: boolean;
}