import { useConfirm } from "primevue/useconfirm";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";

export const knowledgeConfirmationPopup = (id: number, name: string) => {

    const confirm = useConfirm();
    const store = knowledgeStore();

    const deleteConfirmation = (event: MouseEvent) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete ${name}?`,
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
            store.deleteKnowledge(id);
        }
    });

    return { deleteConfirmation };

};

