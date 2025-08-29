import { useDialog } from 'primevue/usedialog';
import type {Knowledge} from "@/components/knowledges/types";
import AddExpression from "@/components/expressions/AddExpression.vue";
import EditExpression from "@/components/expressions/EditExpression.vue";

export const expressionDialogService = () => {

    const dialog = useDialog();

    const showAddExpression = (knowledge: Knowledge) => {
        dialog.open(AddExpression, {
            props: {
                header: 'Add Expression',
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
                knowledge: knowledge,
                isReadOnly: false,
            }
        });
    }

    const showEditExpression = (expressionId: number) => {
        dialog.open(EditExpression, {
            props: {
                header: 'Edit Expression',
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
                expressionId: expressionId
            }
        });
    }
    
    return {
        showAddExpression,
        showEditExpression,
    }
}
