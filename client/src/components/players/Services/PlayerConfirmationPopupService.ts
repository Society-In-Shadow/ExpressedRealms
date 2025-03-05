import { useConfirm } from "primevue/useconfirm";
import toaster from "@/services/Toasters";
import axios from "axios";
import {playerList} from "@/components/players/Stores/PlayerListStore";

export const userConfirmationPopups = (userId: string) => {

    const confirm = useConfirm();
    const playerListStore = playerList();

    const deleteConfirmation = (event: MouseEvent) =>
        confirm.require({
        target: event.target as HTMLElement,
        group: 'popup',
        message: 'Do you want to disable this player?',
        icon: 'pi pi-info-circle',
        rejectProps: {
            label: 'Cancel',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Disable Player',
            severity: 'danger'
        },
        accept: () => {
            axios.put(`/admin/user/${userId}/lockout`,
            {
                userId: userId,
                lockoutEnabled: true
            })
            .then(async () => {
                await playerListStore.fetchPlayers();
                toaster.success('Successfully Disabled Player!');
            });
        }
    });

    const enableConfirmation = (event: MouseEvent) => {
        confirm.require({
            target: event.currentTarget as HTMLElement,
            group: 'popup',
            message: 'Do you want to enable this player?',
            icon: 'pi pi-info-circle',
            rejectProps: {
                label: 'Cancel',
                severity: 'secondary',
                outlined: true
            },
            acceptProps: {
                label: 'Enable Player',
                severity: 'warning'
            },
            accept: () => {
                axios.put(`/admin/user/${userId}/lockout`,
                {
                    userId: userId,
                    lockoutEnabled: false
                })
                .then(async () => {
                    await playerListStore.fetchPlayers();
                    toaster.success('Successfully Enabled Player!');
                });
            }
        });
    };

    const unlockConfirmation = (event: MouseEvent) => {
        confirm.require({
            target: event.currentTarget as HTMLElement,
            group: 'popup',
            message: 'Do you want to unlock this player?',
            icon: 'pi pi-info-circle',
            rejectProps: {
                label: 'Cancel',
                severity: 'secondary',
                outlined: true
            },
            acceptProps: {
                label: 'Unlock Player',
                severity: 'warning'
            },
            accept: () => {
                axios.put(`/admin/user/${userId}/lockout`,
                {
                    userId: userId,
                    lockoutEnabled: false
                })
                .then(async () => {
                    await playerListStore.fetchPlayers();
                    toaster.success('Successfully Unlocked Player!');
                });
            }
        });
    };

    return { deleteConfirmation, enableConfirmation, unlockConfirmation };

};

