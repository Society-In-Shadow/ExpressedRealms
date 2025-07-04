import {describe, it, expect} from "vitest";
import {getValidationInstance} from "../../../../src/components/expressions/powers/Validations/PrerequisiteValidations";

const form = getValidationInstance();
const listItem1 = {description: 'Lorem Ipsum',
    id: 1,
    name: 'General' };

const listItem2 = {description: 'Lorem Ipsum 2',
    id: 2,
    name: 'General 2' }

const oversizedPowerRequirement = {description: 'Lorem Ipsum 2',
    id: 5,
    name: 'General 2' }


describe("Power Prerequisite Model Schema - Field Validations", () => {

    describe("Powers", () => {
        it("No Errors when one value is selected", async () => {
            form.powers.field.value =  [ listItem1 ];
            await form.handleSubmit(() => {})();
            expect(form.powers.error?.value).toBeUndefined();
        });

        it("No Errors when two values is selected", async () => {
            form.powers.field.value =  [ listItem1, listItem2 ];
            await form.handleSubmit(() => {})();
            expect(form.powers.error?.value).toBeUndefined();
        });

        it("Is a required field", async () => {
            form.powers.field.value = null;
            await form.handleSubmit(() => {})();
            expect(form.powers.error.value).toEqual("of the following powers is a required field");
        });

        it("Label is correct", async () => {
            expect(form.powers.label).toEqual("of the following powers");
        })
    });

    describe("Required Amount", () => {
        it("Fails validation when it's missing", async () => {
            form.requiredAmount.field.value = null;
            await form.handleSubmit(() => {})();
            expect(form.requiredAmount.error.value).toEqual("Power Requires is a required field");
        });

        it("No errors when it's a valid value", async () => {
            form.requiredAmount.field.value = listItem1;
            await form.handleSubmit(() => {})();
            expect(form.requiredAmount.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(form.requiredAmount.label).toEqual("Power Requires");
        });
        
        it("Amount needs to be less than powers", async () => {
            form.powers.field.value = [ listItem1 ];
            
            form.requiredAmount.field.value = oversizedPowerRequirement;
            await form.handleSubmit(() => {})();
            expect(form.requiredAmount.error.value).toEqual("Required amount needs to be less then or equal to the number of selected powers");
        })
    });
    
});
