import {defineStore} from "pinia";
import axios from "axios";
import toaster from "@/services/Toasters";
import {
    type CreateStatModifier,
    SourceTableEnum,
    type StatModifier,
    type StatModifierReturnModel,
    type StatModifiersResponse
} from "@/components/modifiergroups/types.ts";
import type {ModifierForm} from "@/components/modifiergroups/validations/modifierValidations.ts";

const modifierGroupStore  =
    defineStore(`modifierGroups`, {
        state: () => {
            return {
                modifierTypes: [] as StatModifier[],
                expressions: [] as StatModifier[],
                haveModifierTypes: false,
                statModifiers: new Map<number, StatModifierReturnModel[]>()
            }
        },
        actions: {
            async getOptions() {
                if (this.haveModifierTypes)
                    return;

                await axios.get(`/modifiergroups/modifiers/options`)
                    .then((response) => {
                        this.modifierTypes = response.data.modifierTypes;
                        this.expressions = response.data.expressions;
                        this.haveModifierTypes = true;
                    });

            },
            async getModifiers(groupId: number){
                await this.getOptions();
                const response = await axios.get<StatModifiersResponse>(`/modifiergroups/${groupId}/modifiers`);
                
                response.data.modifiers.forEach(modifier => {
                    modifier.statModifier = this.modifierTypes.find(x => x.id == modifier.statModifierId);
                    modifier.targetExpression = this.expressions.find(x => x.id == modifier.targetExpressionId);
                })
                
                this.statModifiers.set(groupId, response.data.modifiers);
            },
            getModifierList(groupId: number) : StatModifierReturnModel[]{
                const modifiers = this.statModifiers.get(groupId);
                if(modifiers == null){
                    return []
                }
                return modifiers;
            },
            getModifier: async function (id: number): StatModifier {
                await this.getOptions()
                return this.modifierTypes.find((x: StatModifier) => x.id == id) as StatModifier
            },
            updateModifier: async function (values:ModifierForm, groupId: number, mappingId: number): Promise<void> {
                await axios.put(`/modifiergroups/${groupId}/modifiers/${mappingId}`, {
                    scaleWithLevel: values.scaleWithLevel,
                    modifier: values.modifier,
                    creationSpecificBonus: values.creationSpecificBonus,
                    statModifierId: values.modifierType.id,
                    targetExpressionId: values.targetExpression.id
                })
                .then(async () => {
                    await this.getModifiers(groupId);
                    toaster.success("Successfully Updated Modifier!");
                });
            },
            addModifier: async function (values:ModifierForm, groupId: number | null, sourceId: number, sourceTable: SourceTableEnum): Promise<number> {
                console.log("hii");
                
                let newGroupId;
                let url = `/modifiergroups/${groupId}/modifiers/`;
                if(groupId == null || groupId == 0){
                    url = `/modifiergroups/modifiers`;
                }
                
                await axios.post(url, {
                    sourceTable: sourceTable,
                    sourceId: sourceId,
                    scaleWithLevel: values.scaleWithLevel,
                    modifier: values.modifier,
                    creationSpecificBonus: values.creationSpecificBonus,
                    statModifierId: values.modifierType.id,
                    targetExpressionId: values.targetExpression.id
                } as CreateStatModifier)
                .then(async (response) => {
                    newGroupId = response.data.groupId;
                    await this.getModifiers(response.data.groupId);
                    toaster.success("Successfully Added Modifier!");
                });
                return newGroupId
            },
            deleteModifier: async function(groupId: number, mappingId: number){
                await axios.delete(`/modifiergroups/${groupId}/modifiers/${mappingId}`)
                .then(async () => {
                    await this.getModifiers(groupId);
                    toaster.success(`Successfully Deleted Modifier!`);
                });
            }
        }
    });
export default modifierGroupStore
