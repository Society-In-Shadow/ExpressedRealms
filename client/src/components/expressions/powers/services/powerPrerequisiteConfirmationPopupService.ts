import { useConfirm } from "primevue/useconfirm";
import toaster from "@/services/Toasters";
import axios from "axios";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";

export const powerPrerequisiteConfirmationPopups = (powerPathId: number) => {

    const confirm = useConfirm();
    const powerStore = powersStore();

    const deleteConfirmation = (event: MouseEvent, id: number, prerequisiteId: number): Promise<void> => {
        return new Promise((resolve, reject) => {
            confirm.require({
                target: event.target as HTMLElement,
                group: 'popup',
                message: `Do you want to delete this prerequisite?  This is a permanent action and cannot be undone.`,
                icon: 'pi pi-info-circle',
                rejectProps: {
                    label: 'Cancel',
                    severity: 'secondary',
                    outlined: true
                },
                acceptProps: {
                    label: 'Delete Prerequisite',
                    severity: 'danger'
                },
                accept: async () => {
                    await axios.delete(`/powers/${id}/prerequisite/${prerequisiteId}`)
                        .then(async () => {
                            await powerStore.updatePowersByPathId(powerPathId);
                            toaster.success(`Successfully Deleted Prerequisite!`);
                            resolve();
                        })
                        .catch(async () => {
                            reject();
                        });
                },
                reject: () => {
                    reject();
                }
            });
        });
    }
    return { deleteConfirmation };

};

