import {defineStore} from "pinia";
import axios from "axios";
import type {ListItem} from "@/types/ListItem";
import toaster from "@/services/Toasters";
import type {KnowledgeForm} from "@/components/knowledges/Validations/knowledgeValidations";
import type {
    EditExpressionSection,
    EditExpressionSectionRequest
} from "@/components/expressions/expressionSection/types";
import type {
    ExpressionSectionForm
} from "@/components/expressions/expressionSection/validators/expressionSectionValidator";

export const expressionSectionStore =
    defineStore(`expressionSection`, {
        state: () => {
            return {
                sectionTypes: [] as ListItem[],
                haveSectionTypes: false,
            }
        },
        actions: {
            async getOptions() {
                if (this.haveSectionTypes)
                    return;

                await axios.get(`/expressionSubSections/options`)
                    .then(async (response) => {
                        this.sectionTypes = response.data.sectionTypes;
                    });
            },
            getExpressionSection: async function (expressionId: number, id: number): Promise<EditExpressionSection> {
                await this.getOptions()

                const response = await axios.get<EditExpressionSectionRequest>(
                    `/expressionSubSections/${expressionId}/${id}`);

                return {
                    id: id,
                    name: response.data.name,
                    content: response.data.content,
                    isHeaderSection: response.data.isHeaderSection,
                    sectionType: this.sectionTypes.find((x: ListItem) => x.id == response.data.sectionTypeId) as ListItem
                };
            },
            updateSection: async function (values:ExpressionSectionForm, expressionId: number, id: number): Promise<void> {
                axios.put(`/expressionSubSections/${expressionId}/${id}`, {
                    name: values.name,
                    content: values.content,
                    sectionTypeId: values.sectionType.id,
                }).then(() => {
                    // TODO: Refresh the list
                    toaster.success("Successfully Updated Expression Section Info!");
                });
            },
            addKnowledge: async function (values:KnowledgeForm): Promise<void> {
                await axios.post(`/knowledges/`, {
                    name: values.name,
                    description: values.description,
                    knowledgeTypeId: values.knowledgeType.id,
                })
                    .then(async () => {
                        // TODO: Refresh the list
                        toaster.success("Successfully Added Knowledge!");
                    });
            },
            deleteExpressionSection: async function(expressionId: number, id: number){
                await axios.delete(`/expressionSubSections/${expressionId}/${id}`).then(() => {
                    // TODO: Refresh List
                    toaster.success(`Successfully Deleted Section!`);
                });
            }
        }
    });
