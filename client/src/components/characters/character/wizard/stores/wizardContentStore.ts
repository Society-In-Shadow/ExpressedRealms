import {defineStore} from 'pinia'
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import {markRaw} from "vue";

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
                this.contentComponent = {
                    ...content,
                    component: markRaw(content.component)
                };

                this.showContent = true;
            },
            hideContent(){
                this.showContent = false;
                this.contentComponent = {} as WizardContent;
            }
        }
    });
