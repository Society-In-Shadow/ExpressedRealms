import { useConfirm } from "primevue/useconfirm";
import toaster from "@/services/Toasters";
import axios from "axios";
import {expressionStore} from "@/stores/expressionStore";
import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";

export const powerPathConfirmationPopups = (id: number, name: string) => {

    const confirm = useConfirm();
    const powerStore = powerPathStore();
    const expressionInfo = expressionStore();

    const deleteConfirmation = (event: MouseEvent) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete ${name}?  This will also remove all associated powers.`,
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Delete Power Path',
            severity: 'danger'
        },
        accept: () => {
            axios.delete(`/powerpath/${id}`)
            .then(async () => {
                await powerStore.getPowerPaths(expressionInfo.currentExpressionId);
                toaster.success(`Successfully Deleted ${name}!`);
            });
        }
    });

    return { deleteConfirmation };

};

