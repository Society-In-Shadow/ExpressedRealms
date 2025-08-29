import { useDialog } from 'primevue/usedialog';
import AddExpression from "@/components/expressions/AddExpression.vue";
import EditExpression from "@/components/expressions/EditExpression.vue";

export const expressionDialogService = () => {

    const dialog = useDialog();

    function getDialogTitle(expressionTypeId: number) {
        switch(expressionTypeId){
            case 1: return 'Expression';
            case 13: return 'Rule Book Section';
            case 14: return 'World Background Section';
        }
    }
    const showAddExpression = (expressionTypeId: number) => {
        dialog.open(AddExpression, {
            props: {
                header: `Add ${getDialogTitle(expressionTypeId)}`,
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
                expressionTypeId: expressionTypeId
            }
        });
    }

    const showEditExpression = (expressionId: number, expressionTypeId: number) => {
        dialog.open(EditExpression, {
            props: {
                header: `Edit ${getDialogTitle(expressionTypeId)}`,
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
