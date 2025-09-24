import {useDialog} from 'primevue/usedialog';
import UpdateCharacterXp from "@/components/admin/characterList/UpdateCharacterXp.vue";

export const adminCharacterDialogs = () => {

    const dialog = useDialog();

    const showUpdateXp = (characterId: number, playerNumber: number, xp: number) => {
        dialog.open(UpdateCharacterXp, {
            props: {
                header: 'Update Character XP',
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
                xp: xp,
                characterId: characterId,
                playerNumber: playerNumber
            }
        });
    }
    return {
        showUpdateXp
    }
}
