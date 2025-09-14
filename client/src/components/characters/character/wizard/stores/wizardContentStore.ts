import {defineStore} from 'pinia'
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";

export const wizardContentStore =
    defineStore('wizardContentStore', {
        state: () => {
            return {
                contentComponent: {} as WizardContent,
                showContent: false as boolean,
            }
        },
        actions: {
            updateContent(content: WizardContent){
                this.contentComponent = content;
                this.showContent = true;
            }
        }
    });
