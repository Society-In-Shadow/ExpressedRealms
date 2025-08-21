import {useDialog} from 'primevue/usedialog';
import AddCharacterPower from "@/components/characters/character/powers/AddCharacterPower.vue";
import type {Power} from "@/components/characters/character/powers/types.ts";
import EditCharacterPower from "@/components/characters/character/powers/EditCharacterPower.vue";

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

    const showEditPower = (power: Power) => {
        dialog.open(EditCharacterPower, {
            props: {
                header: 'Edit Power',
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
    
    return {
        showAddPower,
        showEditPower
    }
}
