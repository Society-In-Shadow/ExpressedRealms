import {describe, it, expect} from "vitest";
import {handleSubmit, name, category, description, isPowerUse, powerDuration, powerLevel, powerActivationType, limitation, gameMechanicEffect, areaOfEffect, other} from "../../../../src/components/expressions/powers/Validations/AddPowerValidations";

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
            expect([undefined, '']).toContain(name.error.value);
        });
        it("Label is correct", async () => {
            expect(name.label).toEqual("Name");
        })
    });

    describe("Category", () => {
        it("No Errors when it's a valid value", async () => {
            category.field.value =  [1, 2];
            await handleSubmit(() => {})();
            expect([undefined, '']).toContain(category.error.value);
        });

        it("fails validation when it's empty", async () => {
            category.field.value = [];
            await handleSubmit(() => {})();
            expect(category.error.value).toEqual("At least one category is required");
        });

        it("fails validation when it contains a negative number", async () => {
            category.field.value = [-1, 2];
            await handleSubmit(() => {})();
            expect(category.error.value).toEqual("Category must have positive numbers");
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
            expect([undefined, ""]).toContain(description.error.value);
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
            expect([undefined, ""]).toContain(gameMechanicEffect.error.value);
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
            expect([undefined, ""]).toContain(limitation.error.value);
        });

        it("Label is correct", () => {
            expect(limitation.label).toEqual("Limitation");
        });
    });

    describe("Power Duration", () => {
        it("Fails validation when it's below 0", async () => {
            powerDuration.field.value = -1;
            await handleSubmit(() => {})();
            expect(powerDuration.error.value).toEqual("Power Duration must be between 1 and 255");
        });

        it("Fails validation when it's above 255", async () => {
            powerDuration.field.value = 256;
            await handleSubmit(() => {})();
            expect(powerDuration.error.value).toEqual("Power Duration must be between 1 and 255");
        });

        it("No errors when it's a valid value", async () => {
            powerDuration.field.value = 128;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(powerDuration.error.value);
        });

        it("Label is correct", () => {
            expect(powerDuration.label).toEqual("Power Duration");
        });
    });

    describe("Area of Effect", () => {
        it("Fails validation when it's below 0", async () => {
            areaOfEffect.field.value = -10;
            await handleSubmit(() => {})();
            expect(areaOfEffect.error.value).toEqual("Area of Effect must be greater than 0");
        });

        it("No errors when it's a valid value", async () => {
            areaOfEffect.field.value = 50;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(areaOfEffect.error.value);
        });

        it("Label is correct", () => {
            expect(areaOfEffect.label).toEqual("Area of Effect");
        });
    });

    describe("Power Level", () => {
        it("Fails validation when it's missing", async () => {
            powerLevel.field.value = undefined;
            await handleSubmit(() => {})();
            expect(powerLevel.error.value).toEqual("Power Level is a required field");
        });

        it("No errors when it's a valid value", async () => {
            powerLevel.field.value = 5;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(powerLevel.error.value);
        });

        it("Label is correct", () => {
            expect(powerLevel.label).toEqual("Power Level");
        });
    });

    describe("Power Activation Type", () => {
        it("Fails validation when it's below 0", async () => {
            powerActivationType.field.value = -1;
            await handleSubmit(() => {})();
            expect(powerActivationType.error.value).toEqual("Power Activation Type is a required field");
        });

        it("No errors when it's a valid value", async () => {
            powerActivationType.field.value = 128;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(powerActivationType.error.value);
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

        it("No errors when it's set to true", async () => {
            isPowerUse.field.value = true;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(isPowerUse.error.value);
        });

        it("No errors when it's set to false", async () => {
            isPowerUse.field.value = false;
            await handleSubmit(() => {})();
            expect([undefined, ""]).toContain(isPowerUse.error.value);
        });

        it("Label is correct", () => {
            expect(isPowerUse.label).toEqual("Is Power Use");
        });
    });

});
