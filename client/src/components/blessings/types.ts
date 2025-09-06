
export interface BlessingRequest {
    advantages: Array<SubCategory>
    disadvantages: Array<SubCategory>
    mixedBlessings: Array<SubCategory>
}

export interface BlessingType{
    name: string,
    subCategories: Array<SubCategory>
}

export interface SubCategory {
    name: string,
    blessings: Array<Blessing>
}

export interface Blessing {
    id: number,
    name: string,
    description: string,
    type: string,
    subCategory: string | null,
    levels: Array<BlessingLevel>
    typeId: number
}

export interface BlessingLevel {
    id: number,
    level: string,
    description: string,
    xpCost: number
    xpGain: number
}
