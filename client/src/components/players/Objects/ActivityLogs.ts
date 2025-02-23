import type {ChangedProperty} from "@/components/players/Objects/ChangedProperty";

export interface Log {
    id: number;
    location: string;
    timeStamp: Date; // Date type in TypeScript
    action: string;
    changedProperties: string;
    changedPropertiesList: ChangedProperty[];
}
