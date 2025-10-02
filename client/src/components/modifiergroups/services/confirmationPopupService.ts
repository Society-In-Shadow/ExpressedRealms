import {useConfirm} from "primevue/useconfirm";
import modifierGroupStore from "@/components/modifiergroups/stores/modifierGroupStore.ts";

export const modifierConfirmationPopup = () => {

    const confirm = useConfirm();
    const store = modifierGroupStore();

    const deleteConfirmation = (event: MouseEvent, groupId: number, mappingId: number ) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete this modifier?`,
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Delete Modifier',
            severity: 'danger'
        },
        accept: () => {
            store.deleteModifier(groupId, mappingId);
        }
    });

    return { deleteConfirmation };

};

