import type {ListItem} from "@/types/ListItem";

export interface EditExpressionSectionRequest{
    name: string;
    content: string;
    isHeaderSection: boolean;
    sectionTypeId: number;
}

export interface EditExpressionSection{
    id: number;
    name: string;
    content: string;
    isHeaderSection: boolean;
    sectionType: ListItem;
}