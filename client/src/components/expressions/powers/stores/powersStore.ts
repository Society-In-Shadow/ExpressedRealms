import {defineStore} from "pinia";
import axios from "axios";

import type {
    EditPower,
    EditPowerResponse,
    Power, RawPowerPrerequisite,
    PowerPrerequisiteOptions,
    PowerStore
} from "@/components/expressions/powers/types";
import type {ListItem} from "@/types/ListItem";
import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";
import toaster from "@/services/Toasters";
import type {PowerFormData} from "@/components/expressions/powers/Validations/PowerValidations";

const powerPaths = powerPathStore();

export const powersStore = 
    defineStore(`powers`, {
        state: () => {
            return {
                categories: [] as ListItem[],
                powerDurations: [] as ListItem[],
                powerLevels: [] as ListItem[],
                areaOfEffects: [] as ListItem[],
                powerActivationTypes: [] as ListItem[],
                selectablePowers: [] as ListItem[],
                requiredAmount: [] as ListItem[],
                havePowerOptions: false,
                powers: [] as PowerStore[]
            }
        },
        actions: {
            async getPowerOptions() {
                if (this.havePowerOptions)
                    return;

                await axios.get(`/powers/options`)
                    .then((response) => {
                        this.categories = response.data.category;
                        this.powerDurations = response.data.powerDuration;
                        this.powerLevels = response.data.powerLevel;
                        this.areaOfEffects = response.data.areaOfEffect;
                        this.powerActivationTypes = response.data.powerActivationType;
                        this.selectablePowers = response.data.powers;
                        this.requiredAmount = response.data.requiredAmount;
                        this.havePowerOptions = true;
                    });
                
            },
            async updatePowersByPathId(powerPathId: number){         
                const response = await axios.get<Power[]>(`/powerpath/${powerPathId}/powers`);
               
                await powerPaths.updatePowersForPath(response.data, powerPathId);
            },
            getPower: async function (powerId: number): Promise<EditPower> {
                await this.getPowerOptions()
                const response = await axios.get<EditPowerResponse>(`/powers/${powerId}`);

                return {
                    id: response.data.id,
                    name: response.data.name,
                    description: response.data.description,
                    gameMechanicEffect: response.data.gameMechanicEffect,
                    limitation: response.data.limitation,
                    categories: this.categories.filter((x: ListItem) => response.data.categoryIds.includes(x.id)) as ListItem[],
                    powerDuration: this.powerDurations.find((x: ListItem) => x.id == response.data.powerDurationId) as ListItem,
                    areaOfEffect: this.areaOfEffects.find((x: ListItem) => x.id == response.data.areaOfEffectId) as ListItem,
                    powerLevel: this.powerLevels.find((x: ListItem) => x.id == response.data.powerLevelId) as ListItem,
                    powerActivationType: this.powerActivationTypes.find((x: ListItem) => x.id == response.data.powerActivationTypeId) as ListItem,
                    other: response.data.other,
                    isPowerUse: response.data.isPowerUse,
                    cost: response.data.cost
                };
            },
            getPrerequisitePowerOptions: async function (powerPathId: number): Promise<PowerPrerequisiteOptions> {
                const response = await axios.get<PowerPrerequisiteOptions>(`/powerpath/${powerPathId}/powerprerequisites/options`);
                return response.data;
            },
            getPrerequisitePowers: async function (powerId: number): Promise<RawPowerPrerequisite | null> {
                const response = await axios.get<RawPowerPrerequisite | null>(`/powers/${powerId}/prerequisites`);
                return response.data;
            },
            updatePower: async function (values:PowerFormData, powerId: number, powerPathId: number): Promise<void> {
                await axios.put(`/powers/${powerId}`, {
                    id: powerId,
                    name: values.name,
                    description: values.description,
                    gameMechanicEffect: values.gameMechanicEffect,
                    limitation: values.limitation,
                    powerDurationId: values.powerDuration.id,
                    areaOfEffectId: values.areaOfEffect.id,
                    powerLevelId: values.powerLevel.id,
                    powerActivationTypeId: values.powerActivationType.id,
                    categoryIds: values.category?.map((item: { id: string | number }) => item.id),
                    other: values.other,
                    isPowerUse: values.isPowerUse,
                    cost: values.cost
                })
                    .then(async () => {
                        await this.updatePowersByPathId(powerPathId);
                        toaster.success("Successfully Updated Power!");
                    });
            }
        }
    });
