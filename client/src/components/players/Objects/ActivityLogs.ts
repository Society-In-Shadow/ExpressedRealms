export interface Log {
    id: number;
    location: string;
    timeStamp: Date; // Date type in TypeScript
    action: string;
    changedProperties: string;
}
