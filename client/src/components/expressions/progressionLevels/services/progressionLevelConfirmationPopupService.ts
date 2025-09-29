import {useConfirm} from "primevue/useconfirm";
import toaster from "@/services/Toasters";
import axios from "axios";
import {expressionStore} from "@/stores/expressionStore";
import {progressionPathStore} from "@/components/expressions/progressionPaths/stores/progressionPathsStore.ts";

export const progressionLevelConfirmationPopupService = (progressionId: number, name: string) => {

    const confirm = useConfirm();
    const progressionPaths = progressionPathStore();
    const expressionInfo = expressionStore();

    const deleteConfirmation = (event: MouseEvent, levelId: number) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: `Do you want to delete ${name}?  This will also remove all associated levels.`,
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Delete Progression Path',
            severity: 'danger'
        },
        accept: () => {
            axios.delete(`/expression/${expressionInfo.currentExpressionId}/progressions/${progressionId}/levels/${levelId}`)
            .then(async () => {
                await progressionPaths.getProgressionPaths(expressionInfo.currentExpressionId);
                toaster.success(`Successfully Deleted ${name}!`);
            });
        }
    });

    return { deleteConfirmation };

};

