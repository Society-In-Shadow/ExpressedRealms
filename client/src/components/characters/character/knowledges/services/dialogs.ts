import AddCharacterKnowledge from "@/components/characters/character/knowledges/AddCharacterKnowledge.vue";
import { useDialog } from 'primevue/usedialog';
import type {Knowledge} from "@/components/knowledges/types";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types";
import EditCharacterKnowledge from "@/components/characters/character/knowledges/EditCharacterKnowledge.vue";


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
    
    return {
        showAddCharacter,
        showEditCharacter
    }
}