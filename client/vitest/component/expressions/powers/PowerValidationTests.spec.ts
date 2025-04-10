import {describe, it, expect} from "vitest";
import {createPowerModelSchema} from "../../../../src/components/expressions/powers/Validations/AddPowerValidations";

describe("Power Model Schema - Field Validations", () => {
    describe("Name", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {name: "Valid Name"};
            await expect(createPowerModelSchema.validateAt("name", data)).resolves.toEqual("Valid Name");
        });

        it("fails validation when it's empty", async () => {
            const data = {name: ""};
            await expect(createPowerModelSchema.validateAt("name", data)).rejects.toThrow("Name is required");
        });
    });

    describe("Category", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {category: [1, 2]};
            await expect(createPowerModelSchema.validateAt("category", data)).resolves.toEqual([1, 2]);
        });

        it("fails validation when it's empty", async () => {
            const data = {category: []};
            await expect(createPowerModelSchema.validateAt("category", data)).rejects.toThrow("At least one category is required");
        });

        it("fails validation when it contains a negative number", async () => {
            const data = {category: [-1, 2]};
            await expect(createPowerModelSchema.validateAt("category", data)).rejects.toThrow("Category must have positive numbers");
        });
    });

    describe("Description", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {description: "This is a valid description"};
            await expect(createPowerModelSchema.validateAt("description", data)).resolves.toEqual("This is a valid description");
        });

        it("fails validation when it's missing", async () => {
            const data = {description: ""};
            await expect(createPowerModelSchema.validateAt("description", data)).rejects.toThrow("Description is required");
        });
    });

    describe("Game Mechanic Effect", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {gameMechanicEffect: "Some valid effect"};
            await expect(createPowerModelSchema.validateAt("gameMechanicEffect", data)).resolves.toEqual("Some valid effect");
        });

        it("fails validation when it's missing", async () => {
            const data = {gameMechanicEffect: ""};
            await expect(createPowerModelSchema.validateAt("gameMechanicEffect", data)).rejects.toThrow("Game Mechanic Effect is required");
        });
    });

    describe("Limitation", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {limitation: "Some valid limitation"};
            await expect(createPowerModelSchema.validateAt("limitation", data)).resolves.toEqual("Some valid limitation");
        });

        it("fails validation when it's missing", async () => {
            const data = {limitation: ""};
            await expect(createPowerModelSchema.validateAt("limitation", data)).rejects.toThrow("Limitation is required");
        });
    });

    describe("Power Duration", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {powerDuration: 128};
            await expect(createPowerModelSchema.validateAt("powerDuration", data)).resolves.toEqual(128);
        });

        it("fails validation when it's below 0", async () => {
            const data = {powerDuration: -1};
            await expect(createPowerModelSchema.validateAt("powerDuration", data)).rejects.toThrow("Power Duration must be between 1 and 255");
        });

        it("fails validation when it's above 255", async () => {
            const data = {powerDuration: 256};
            await expect(createPowerModelSchema.validateAt("powerDuration", data)).rejects.toThrow("Power Duration must be between 1 and 255");
        });
    });

    describe("Area of Effect", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {areaOfEffect: 50};
            await expect(createPowerModelSchema.validateAt("areaOfEffect", data)).resolves.toEqual(50);
        });

        it("fails validation when it's below 0", async () => {
            const data = {areaOfEffect: -10};
            await expect(createPowerModelSchema.validateAt("areaOfEffect", data)).rejects.toThrow("Area of Effect must be greater than 0");
        });
    });

    describe("Power Level", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {powerLevel: 5};
            await expect(createPowerModelSchema.validateAt("powerLevel", data)).resolves.toEqual(5);
        });

        it("fails validation when it's missing", async () => {
            const data = {powerLevel: undefined};
            await expect(createPowerModelSchema.validateAt("powerLevel", data)).rejects.toThrow("Power Level is required");
        });
    });

    describe("Power Activation Type", () => {
        it("succeeds when it's set to a value", async () => {
            const data = {powerActivationType: 128};
            await expect(createPowerModelSchema.validateAt("powerActivationType", data)).resolves.toEqual(128);
        });

        it("fails validation when it's set below 0", async () => {
            const data = {powerActivationType: -1};
            await expect(createPowerModelSchema.validateAt("powerActivationType", data)).rejects.toThrow("Power Activation Type is required");
        });
    });

    describe("Is Power Use", () => {
        it("succeeds when it's set to true", async () => {
            const data = {isPowerUse: true};
            await expect(createPowerModelSchema.validateAt("isPowerUse", data)).resolves.toEqual(true);
        });

        it("succeeds when it's set to false", async () => {
            const data = {isPowerUse: false};
            await expect(createPowerModelSchema.validateAt("isPowerUse", data)).resolves.toEqual(false);
        });

        it("fails validation when it's missing", async () => {
            const data = {isPowerUse: undefined};
            await expect(createPowerModelSchema.validateAt("isPowerUse", data)).rejects.toThrow("Is Power Use is required");
        });
    });
});
