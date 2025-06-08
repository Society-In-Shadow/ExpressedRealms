import {describe, it, expect} from "vitest";
import {getValidationInstance} from "../../../../src/components/expressions/powerPaths/validations/powerPathValidations";

const form = getValidationInstance();
describe("Power Model Schema - Field Validations", () => {
    describe("Name", () => {
        it("Fails when there are more then 250 characters", async () => {
            form.name.field.value = "a".repeat(251);
            await form.handleSubmit(() => {})();
            expect(form.name.error.value).toEqual("Name must be at most 250 characters");
        });
        it("Says it's required when not filled in", async () => {
            form.name.field.value = "";
            await form.handleSubmit(() => {})();
            expect(form.name.error.value).toEqual("Name is a required field");
        });
        it("No Errors when it's a valid value", async () => {
            form.name.field.value = "asdf";
            await form.handleSubmit(() => {})();
            expect(form.name.error?.value).toBeUndefined();
        });
        it("Label is correct", async () => {
            expect(form.name.label).toEqual("Name");
        })
    });

    describe("Description", () => {
        it("Fails validation when it's missing", async () => {
            form.description.field.value = "";
            await form.handleSubmit(() => {})();
            expect(form.description.error.value).toEqual("Description is a required field");
        });

        it("No errors when it's a valid value", async () => {
            form.description.field.value = "This is a valid description";
            await form.handleSubmit(() => {})();
            expect(form.description.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(form.description.label).toEqual("Description");
        });
    });
});
