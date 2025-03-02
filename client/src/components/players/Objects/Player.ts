export interface PlayerListItem {
    id: string;
    username: string;
    email: string;
    roles: string[];
    emailVerified: boolean;
    isDisabled: boolean;
    lockedOut: boolean;
    lockedOutExpires: Date;
}
