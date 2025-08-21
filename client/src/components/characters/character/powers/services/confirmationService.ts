import {useConfirm} from "primevue/useconfirm";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";

export const confirmationPopup = (characterId: number) => {

    const confirm = useConfirm();
    const store = characterPowersStore();

    const deleteConfirmation = (event: MouseEvent, powerId: number) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete this power?`,
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Delete Power',
            severity: 'danger'
        },
        accept: () => {
            store.deletePower(characterId, powerId);
        }
    });
    
    return { deleteConfirmation };

};

