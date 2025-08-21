import {useDialog} from 'primevue/usedialog';
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types";
import EditCharacterKnowledge from "@/components/characters/character/knowledges/EditCharacterKnowledge.vue";
import AddCharacterPower from "@/components/characters/character/powers/AddCharacterPower.vue";
import type {Power} from "@/components/characters/character/powers/types.ts";

export const characterPowerDialogs = () => {

    const dialog = useDialog();

    const showAddPower = (power: Power) => {
        dialog.open(AddCharacterPower, {
            props: {
                header: 'Add Power',
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
                power: power
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
        showAddPower,
        showEditCharacter
    }
}
