

export interface EventDetails{
    name: string;
    startDate: Date;
    endDate: Date;
    location: string;
    conWebsiteName: string;
    conWebsiteUrl: string;
    staff: StaffDetail[];
}

export interface StaffDetail{
    name: string;
    bio: string;
}