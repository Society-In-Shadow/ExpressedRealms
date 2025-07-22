import { knowledgeStore } from "@/components/knowledges/stores/knowledgeStore";
import { useConfirm } from "primevue/useconfirm";
import {expressionStore} from "@/stores/expressionStore";
import {expressionSectionStore} from "@/components/expressions/expressionSection/store/expressionSectionStore";


export const expressionSectionConfirmationPopup = (id: number) => {

const confirm = useConfirm();
const expressionInfo = expressionStore();
const expressionSectionInfo = expressionSectionStore();

const deleteConfirmation = (event: MouseEvent) =>   confirm.require({
    target: event.currentTarget,
    header: 'Deleting Section',
    message: `Are you sure you want delete this section?  This will delete this section and any sub children`,
    icon: 'pi pi-exclamation-triangle',
    group: 'popup',
    rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true
    },
    acceptProps: {
        label: 'Save'
    },
    accept: async () => {
        await expressionSectionInfo.deleteExpressionSection(expressionInfo.currentExpressionId, id);
    },
    reject: () => {}
});

return { deleteConfirmation };

};
