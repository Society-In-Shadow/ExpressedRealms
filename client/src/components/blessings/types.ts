
export interface BlessingRequest {
    advantages: Array<Blessing>
    disadvantages: Array<Blessing>
    mixedBlessings: Array<Blessing>
}

export interface Blessing {
    id: number,
    name: string,
    description: string,
    subCategory: string | null,
    levels: Array<BlessingLevel>
    typeId: number
}

export interface BlessingLevel {
    id: number,
    name: string,
    description: string,
    XpCost: number
    XpGain: number
}
