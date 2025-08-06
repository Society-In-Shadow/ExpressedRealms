import { useConfirm } from "primevue/useconfirm";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";

export const confirmationPopup = (characterId: number) => {

    const confirm = useConfirm();
    const store = characterKnowledgeStore();

    const deleteConfirmation = (event: MouseEvent, mappingId: number) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete this knowledge?`,
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Delete Knowledge',
            severity: 'danger'
        },
        accept: () => {
            store.deleteKnowledge(characterId, mappingId);
        }
    });

    return { deleteConfirmation };

};

