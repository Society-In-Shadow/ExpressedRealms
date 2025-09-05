import {useDialog} from 'primevue/usedialog';
import EditBlessingLevel from "@/components/blessings/BlessingLevels/EditBlessingLevel.vue";
import AddBlessingLevel from "@/components/blessings/BlessingLevels/AddBlessingLevel.vue";

export const addBlessingDialog = () => {

    const dialog = useDialog();

    const showAddBlessingLevel = (blessingId: number) => {
        dialog.open(AddBlessingLevel, {
            props: {
                header: 'Add Blessing Level',
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
                blessingId: blessingId,
            }
        });
    }

    const showEditBlessingLevel = (blessingId: number, levelId: number) => {
        dialog.open(EditBlessingLevel, {
            props: {
                header: 'Edit Blessing Level',
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
                blessingId: blessingId,
                levelId: levelId,
            }
        });
    }
    
    return {
        showAddBlessingLevel,
        showEditBlessingLevel,
    }
}
