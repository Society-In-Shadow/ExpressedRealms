import {useDialog} from 'primevue/usedialog';
import OverallExperience from "@/components/characters/character/OverallExperience.vue";

export const characterPopupDialogs = () => {

    const dialog = useDialog();

    const showExperience = () => {
        dialog.open(OverallExperience, {
            props: {
                header: 'Experience Breakdown',
                style: {
                    width: '500px',
                },
                breakpoints: {
                    '960px': '75vw',
                    '640px': '90vw'
                },
                modal: true
            }
        });
    }
    
    return {
        showExperience
    }
}
