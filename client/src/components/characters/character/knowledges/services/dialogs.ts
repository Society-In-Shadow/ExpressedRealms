import {useDialog} from 'primevue/usedialog';
import type {Knowledge} from "@/components/knowledges/types";
import type {CharacterKnowledge, Specialization} from "@/components/characters/character/knowledges/types";
import AddSpecializationKnowledge
    from "@/components/characters/character/wizard/knowledges/AddSpecializationKnowledge.vue";
import EditSpecializationKnowledge
    from "@/components/characters/character/wizard/knowledges/EditSpecializationKnowledge.vue";

export const addKnowledgeDialog = () => {

    const dialog = useDialog();


    const showAddSpecialization = (knowledge: Knowledge) => {
        dialog.open(AddSpecializationKnowledge, {
            props: {
                header: 'Add Specialization',
                style: {
                    width: '500px',
                },
                breakpoints: {
                    '960px': '75vw',
                    '640px': '90vw'
                },
                modal: true
            },
            data: {
                knowledge: knowledge
            }
        });
    }

    const showEditSpecialization = (knowledge: CharacterKnowledge, specialization: Specialization) => {
        dialog.open(EditSpecializationKnowledge, {
            props: {
                header: 'Edit Specialization',
                style: {
                    width: '500px',
                },
                breakpoints: {
                    '960px': '75vw',
                    '640px': '90vw'
                },
                modal: true
            },
            data: {
                knowledge: knowledge,
                specialization: specialization
            }
        });
    }
    
    return {
        showAddSpecialization,
        showEditSpecialization
    }
}
