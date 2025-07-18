import {defineStore} from "pinia";
import type {ExpressionInfo} from "@/components/public/types";

export const publicExpressionsStore =
    defineStore(`publicExpressions`, {
        state: () => {
            return {
                expressions: [] as ExpressionInfo[]
            }
        },
        actions: {
            async getExpressions(){

                this.expressions = [{
                    name: 'Adepts',
                    archetypes: 'Martial artists, medics, mentalists, negotiators, philosophers',
                    description: ''
                }]
            },
        }
    });
