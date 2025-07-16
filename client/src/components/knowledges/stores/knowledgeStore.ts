import {defineStore} from "pinia";
import axios from "axios";

import type {ListItem} from "@/types/ListItem";
import toaster from "@/services/Toasters";
import type {EditKnowledge, EditKnowledgeRequest, Knowledge} from "@/components/knowledges/types";
import type {KnowledgeForm} from "@/components/knowledges/Validations/knowledgeValidations";

export const knowledgeStore =
    defineStore(`powers`, {
        state: () => {
            return {
                knowledgeTypes: [] as ListItem[],
                haveKnowledgeTypes: false,
                haveKnowledges: false,
                knowledges: [] as Knowledge[]
            }
        },
        actions: {
            async getOptions() {
                if (this.haveKnowledgeTypes)
                    return;

                await axios.get(`/knowledgetypes`)
                    .then((response) => {
                        this.knowledgeTypes = response.data.knowledgeTypes;
                        this.haveKnowledgeTypes = true;
                    });

            },
            async getKnowledges(){
                const response = await axios.get<Knowledge[]>(`/knowledges`);
                this.knowledges = response.data.knowledges
            },
            getKnowledge: async function (id: number): Promise<EditKnowledge> {
                await this.getOptions()
                
                const response = await axios.get<EditKnowledgeRequest>(`/knowledges/${id}`);

                return {
                    id: response.data.id,
                    name: response.data.name,
                    description: response.data.description,
                    knowledgeType: this.knowledgeTypes.find((x: ListItem) => x.id == response.data.typeId) as ListItem
                };
            },
            updateKnowledge: async function (values:KnowledgeForm, id: number): Promise<void> {
                await axios.put(`/knowledges/${id}`, {
                    id: id,
                    name: values.name,
                    description: values.description,
                    knowledgeTypeId: values.knowledgeType.id,
                })
                    .then(async () => {
                        await this.getKnowledges();
                        toaster.success("Successfully Updated Knowledge!");
                    });
            },
            addKnowledge: async function (values:KnowledgeForm): Promise<void> {
                await axios.post(`/knowledges/`, {
                    name: values.name,
                    description: values.description,
                    knowledgeTypeId: values.knowledgeType.id,
                })
                    .then(async () => {
                        await this.getKnowledges();
                        toaster.success("Successfully Added Knowledge!");
                    });
            },
            deleteKnowledge: async function(id: number){
                
                let name = "knowledge";
                
                const knowledge = this.knowledges.find((x: Knowledge) => x.id == id);
                if(knowledge)
                    name = knowledge.name;
                
                await axios.delete(`/knowledges/${id}`)
                    .then(async () => {
                        await this.getKnowledges()
                        toaster.success(`Successfully Deleted ${name}!`);
                    });
            }
        }
    });
