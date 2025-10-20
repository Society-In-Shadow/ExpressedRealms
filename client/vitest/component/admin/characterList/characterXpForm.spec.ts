import {describe} from "vitest";
import {getValidationInstance} from "@/components/admin/characterList/validators/characterXpForm.ts";
import {addRunCommonRequiredTests} from "../../../utilities/formUtilities.ts";

const form = addRunCommonRequiredTests(getValidationInstance());
describe("Character XP Form - Field Validations", () => {
    
    describe("Available Character XP", () => {
        form.runCommonRequiredTests("xp", "Available Character XP");
    });
    
    describe("Player Number", () => {
        it("Fails greater than 1000", async () => {
            form.fields.playerNumber.field.value = 1001;
            await form.handleSubmit(() => {})();
            expect(form.fields.playerNumber.error.value).toEqual("Player Number must be less than or equal to 999");
        });
        form.runCommonRequiredTests("playerNumber", "Player Number");
    });
});
