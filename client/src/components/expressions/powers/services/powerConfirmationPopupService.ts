import { useConfirm } from "primevue/useconfirm";
import toaster from "@/services/Toasters";
import axios from "axios";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";

export const powerConfirmationPopups = (id: number, name: string, powerPathId: number) => {

    const confirm = useConfirm();
    const powerStore = powersStore();

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
            label: 'Delete Power',
            severity: 'danger'
        },
        accept: () => {
            axios.delete(`/powers/${id}`)
            .then(async () => {
                await powerStore.updatePowersByPathId(powerPathId);
                toaster.success(`Successfully Deleted ${name}!`);
            });
        }
    });

    return { deleteConfirmation };

};

