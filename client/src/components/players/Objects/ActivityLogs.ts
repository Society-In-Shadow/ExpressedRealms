import type {ChangedProperty} from "@/components/players/Objects/ChangedProperty";

export interface Log {
    id: number;
    location: string;
    timeStamp: Date;
    action: string;
    changedProperties: string;
    changedPropertiesList: ChangedProperty[];
}
