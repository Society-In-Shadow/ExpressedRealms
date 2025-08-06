import AddCharacterKnowledge from "@/components/characters/character/knowledges/AddCharacterKnowledge.vue";
import { useDialog } from 'primevue/usedialog';
import type {Knowledge} from "@/components/knowledges/types";
import type {CharacterKnowledge, Specialization} from "@/components/characters/character/knowledges/types";
import EditCharacterKnowledge from "@/components/characters/character/knowledges/EditCharacterKnowledge.vue";
import EditSpecializationKnowledge from "@/components/characters/character/knowledges/EditSpecializationKnowledge.vue";
import AddSpecializationKnowledge from "@/components/characters/character/knowledges/AddSpecializationKnowledge.vue";

export const addKnowledgeDialog = () => {

    const dialog = useDialog();

    const showAddCharacter = (knowledge: Knowledge) => {
        dialog.open(AddCharacterKnowledge, {
            props: {
                header: 'Add Knowledge',
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
                isReadOnly: false,
            }
        });
    }

    const showEditCharacter = (knowledge: CharacterKnowledge) => {
        dialog.open(EditCharacterKnowledge, {
            props: {
                header: 'Edit Knowledge',
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
                isReadOnly: false,
            }
        });
    }

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
        showAddCharacter,
        showEditCharacter,
        showAddSpecialization,
        showEditSpecialization
    }
}