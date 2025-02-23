export interface ChangedProperty {
    id: number;
    propertyName: string;
    oldValue?: string | null;
    newValue?: string | null;
}
