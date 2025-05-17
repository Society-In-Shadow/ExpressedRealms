import {describe, it, expect} from "vitest";
import {handleSubmit, name, category, description, isPowerUse, powerDuration, powerLevel, powerActivationType, limitation, gameMechanicEffect, areaOfEffect, other} from "../../../../src/components/expressions/powers/Validations/AddPowerValidations";

const listItem1 = {description: 'Lorem Ipsum',
    id: 1,
    name: 'General' };

const listItem2 = {description: 'Lorem Ipsum 2',
    id: 2,
    name: 'General 2' }

describe("Power Model Schema - Field Validations", () => {
    describe("Name", () => {
        it("Fails when there are more then 250 characters", async () => {
            name.field.value = "a".repeat(251);
            await handleSubmit(() => {})();
            expect(name.error.value).toEqual("Name must be at most 250 characters");
        });
        it("Says it's required when not filled in", async () => {
            name.field.value = "";
            await handleSubmit(() => {})();
            expect(name.error.value).toEqual("Name is a required field");
        });
        it("No Errors when it's a valid value", async () => {
            name.field.value = "asdf";
            await handleSubmit(() => {})();
            expect(name.error?.value).toBeUndefined();
        });
        it("Label is correct", async () => {
            expect(name.label).toEqual("Name");
        })
    });

    describe("Category", () => {
        it("No Errors when one value is selected", async () => {
            category.field.value =  [ listItem1 ];
            await handleSubmit(() => {})();
            expect(category.error?.value).toBeUndefined();
        });

        it("No Errors when two values is selected", async () => {
            category.field.value =  [ listItem1, listItem2 ];
            await handleSubmit(() => {})();
            expect(category.error?.value).toBeUndefined();
        });

        it("fails validation when it's empty", async () => {
            category.field.value = null;
            await handleSubmit(() => {})();
            expect(category.error.value).toEqual("At least one category is required");
        });

        it("Label is correct", async () => {
            expect(category.label).toEqual("Category");
        })
    });

    describe("Description", () => {
        it("Fails validation when it's missing", async () => {
            description.field.value = "";
            await handleSubmit(() => {})();
            expect(description.error.value).toEqual("Description is a required field");
        });

        it("No errors when it's a valid value", async () => {
            description.field.value = "This is a valid description";
            await handleSubmit(() => {})();
            expect(description.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(description.label).toEqual("Description");
        });
    });

    describe("Game Mechanic Effect", () => {
        it("Fails validation when it's missing", async () => {
            gameMechanicEffect.field.value = "";
            await handleSubmit(() => {})();
            expect(gameMechanicEffect.error.value).toEqual("Game Mechanic Effect is a required field");
        });

        it("No errors when it's a valid value", async () => {
            gameMechanicEffect.field.value = "Some valid effect";
            await handleSubmit(() => {})();
            expect(gameMechanicEffect.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(gameMechanicEffect.label).toEqual("Game Mechanic Effect");
        });
    });

    describe("Limitation", () => {
        it("Fails validation when it's missing", async () => {
            limitation.field.value = "";
            await handleSubmit(() => {})();
            expect(limitation.error.value).toEqual("Limitation is a required field");
        });

        it("No errors when it's a valid value", async () => {
            limitation.field.value = "Some valid limitation";
            await handleSubmit(() => {})();
            expect(limitation.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(limitation.label).toEqual("Limitation");
        });
    });

    describe("Power Duration", () => {

        it("No errors when it's a valid value", async () => {
            powerDuration.field.value = listItem1;
            await handleSubmit(() => {})();
            expect(powerDuration.error.value).toBeUndefined();
        });
        
        it("Is Required", async () => {
            powerDuration.field.value = undefined;
            await handleSubmit(() => {})();
            expect(powerDuration.error.value).toContain("Power Duration is a required field");
        });

        it("Label is correct", () => {
            expect(powerDuration.label).toEqual("Power Duration");
        });
        
    });

    describe("Area of Effect", () => {

        it("No errors when it's a valid value", async () => {
            areaOfEffect.field.value = listItem1;
            await handleSubmit(() => {})();
            expect(areaOfEffect.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            areaOfEffect.field.value = undefined;
            await handleSubmit(() => {})();
            expect(areaOfEffect.error.value).toContain("Area of Effect is a required field");
        });

        it("Label is correct", () => {
            expect(areaOfEffect.label).toEqual("Area of Effect");
        });
        
    });

    describe("Power Level", () => {
        it("No errors when it's a valid value", async () => {
            powerLevel.field.value = listItem1;
            await handleSubmit(() => {})();
            expect(powerLevel.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            powerLevel.field.value = undefined;
            await handleSubmit(() => {})();
            expect(powerLevel.error.value).toContain("Power Level is a required field");
        });

        it("Label is correct", () => {
            expect(powerLevel.label).toEqual("Power Level");
        });
        
    });

    describe("Power Activation Type", () => {
        it("No errors when it's a valid value", async () => {
            powerActivationType.field.value = listItem1;
            await handleSubmit(() => {})();
            expect(powerActivationType.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            powerActivationType.field.value = undefined;
            await handleSubmit(() => {})();
            expect(powerActivationType.error.value).toContain("Power Activation Type is a required field");
        });

        it("Label is correct", () => {
            expect(powerActivationType.label).toEqual("Power Activation Type");
        });
    });

    describe("Is Power Use", () => {
        it("Fails validation when it's missing", async () => {
            isPowerUse.field.value = undefined;
            await handleSubmit(() => {})();
            expect(isPowerUse.error.value).toEqual("Is Power Use is a required field");
        });

        // TODO: Figure out way to test default value in vee validate form
        /*it("Has a default value of false", async () => {
            expect(isPowerUse.field.value).toBe(false);
            expect(isPowerUse.error.value).toBeUndefined();
        });*/
        
        it("No errors when it's set to true", async () => {
            isPowerUse.field.value = true;
            await handleSubmit(() => {})();
            expect(isPowerUse.error.value).toBeUndefined();
        });

        it("No errors when it's set to false", async () => {
            isPowerUse.field.value = false;
            await handleSubmit(() => {})();
            expect(isPowerUse.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(isPowerUse.label).toEqual("Is Power Use");
        });
    });

});
