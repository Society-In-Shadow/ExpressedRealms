import {describe, it, expect} from "vitest";
import {nameField, shortDescriptionField, navMenuImageField, handleSubmit} from "../../../../src/components/expressions/expression/AddExpressionValidation";
import {name} from "../../../../src/components/expressions/powers/Validations/AddPowerValidations";

describe("Add Expression Schema - Field Validations", () => {
    
    describe("Name", () => {
        it("Fails when there are more then 50 characters", async () => {
            nameField.field.value = "a".repeat(51);
            await handleSubmit(() => {})();
            expect(nameField.error.value).toEqual("Name must be at most 50 characters");
        });
        it("Says it's required when not filled in", async () => {
            nameField.field.value = "";
            await handleSubmit(() => {})();
            expect(nameField.error.value).toEqual("Name is a required field");
        });
        it("No Errors when it's a valid value", async () => {
            nameField.field.value = "asdf";
            await handleSubmit(() => {})();
            expect([undefined, '']).toContain(name.error.value);
        });
        it("Label is correct", async () => {
            expect(nameField.label).toEqual("Name");
        })
    });
    
    describe("Short Description", () => {
        it("Fails when there are more then 125 characters", async () => {
            shortDescriptionField.field.value = "a".repeat(126);
            await handleSubmit(() => {})();
            expect(shortDescriptionField.error.value).toEqual("Short Description must be at most 125 characters");
        });
        it("Says it's required when not filled in", async () => {
            shortDescriptionField.field.value = "";
            await handleSubmit(() => {})();
            await expect(shortDescriptionField.error.value).toEqual("Short Description is a required field");
        });
        it("Label is correct", async () => {
            expect(shortDescriptionField.label).toEqual("Short Description");
        })
        it("No Errors when it's a valid value", async () => {
            shortDescriptionField.field.value = "asdf";
            await handleSubmit(() => {})();
            expect([undefined, '']).toContain(shortDescriptionField.error.value);
        });
    });
    
    describe("Nav Menu Icon", () => {
        it("Default Value is 'pi-prime'", async () => {
            expect(navMenuImageField.field.value).toEqual("pi-prime");
        });
        it("Says it's required when not filled in", async () => {
            navMenuImageField.field.value = "";
            await handleSubmit(() => {})();
            await expect(navMenuImageField.error.value).toEqual("Nav Menu Icon is a required field");
        });
        it("No Errors when it's a valid value", async () => {
            navMenuImageField.field.value = "asdf";
            await handleSubmit(() => {})();
            expect([undefined, '']).toContain(navMenuImageField.error.value);
        });
        it("Label is correct", async () => {
            expect(navMenuImageField.label).toEqual("Nav Menu Icon");
        })
    });
    
});
