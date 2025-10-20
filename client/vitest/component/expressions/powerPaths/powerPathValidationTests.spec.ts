import {describe, expect, it} from "vitest";
import {getValidationInstance} from "@/components/expressions/powerPaths/validations/powerPathValidations.ts";
import {addRunCommonRequiredTests} from "../../../utilities/formUtilities.ts";


const form = addRunCommonRequiredTests(getValidationInstance());
describe("Power Model Schema - Field Validations", () => {
    describe("Name", () => {
        it("Fails when there are more then 250 characters", async () => {
            form.name.field.value = "a".repeat(251);
            await form.handleSubmit(() => {})();
            expect(form.name.error.value).toEqual("Name must be at most 250 characters");
        });
        form.runCommonRequiredTests("name", "Name");
    });

    describe("Description", () => {
        form.runCommonRequiredTests("description", "Description");
    });
});
