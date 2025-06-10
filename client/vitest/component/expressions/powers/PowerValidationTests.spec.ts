import {describe, it, expect} from "vitest";
import {getValidationInstance} from "../../../../src/components/expressions/powers/Validations/PowerValidations";

const form = getValidationInstance();
const listItem1 = {description: 'Lorem Ipsum',
    id: 1,
    name: 'General' };

const listItem2 = {description: 'Lorem Ipsum 2',
    id: 2,
    name: 'General 2' }

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

    describe("Category", () => {
        it("No Errors when one value is selected", async () => {
            form.category.field.value =  [ listItem1 ];
            await form.handleSubmit(() => {})();
            expect(form.category.error?.value).toBeUndefined();
        });

        it("No Errors when two values is selected", async () => {
            form.category.field.value =  [ listItem1, listItem2 ];
            await form.handleSubmit(() => {})();
            expect(form.category.error?.value).toBeUndefined();
        });

        it("fails validation when it's empty", async () => {
            form.category.field.value = null;
            await form.handleSubmit(() => {})();
            expect(form.category.error.value).toEqual("At least one category is required");
        });

        it("Label is correct", async () => {
            expect(form.category.label).toEqual("Category");
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

    describe("Game Mechanic Effect", () => {
        it("Fails validation when it's missing", async () => {
            form.gameMechanicEffect.field.value = "";
            await form.handleSubmit(() => {})();
            expect(form.gameMechanicEffect.error.value).toEqual("Game Mechanic Effect is a required field");
        });

        it("No errors when it's a valid value", async () => {
            form.gameMechanicEffect.field.value = "Some valid effect";
            await form.handleSubmit(() => {})();
            expect(form.gameMechanicEffect.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(form.gameMechanicEffect.label).toEqual("Game Mechanic Effect");
        });
    });

    describe("Limitation", () => {
        it("Fails validation when it's missing", async () => {
            form.limitation.field.value = "";
            await form.handleSubmit(() => {})();
            expect(form.limitation.error.value).toEqual("Limitation is a required field");
        });

        it("No errors when it's a valid value", async () => {
            form.limitation.field.value = "Some valid limitation";
            await form.handleSubmit(() => {})();
            expect(form.limitation.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(form.limitation.label).toEqual("Limitation");
        });
    });

    describe("Power Duration", () => {

        it("No errors when it's a valid value", async () => {
            form.powerDuration.field.value = listItem1;
            await form.handleSubmit(() => {})();
            expect(form.powerDuration.error.value).toBeUndefined();
        });
        
        it("Is Required", async () => {
            form.powerDuration.field.value = undefined;
            await form.handleSubmit(() => {})();
            expect(form.powerDuration.error.value).toContain("Power Duration is a required field");
        });

        it("Label is correct", () => {
            expect(form.powerDuration.label).toEqual("Power Duration");
        });
        
    });

    describe("Area of Effect", () => {

        it("No errors when it's a valid value", async () => {
            form.areaOfEffect.field.value = listItem1;
            await form.handleSubmit(() => {})();
            expect(form.areaOfEffect.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            form.areaOfEffect.field.value = undefined;
            await form.handleSubmit(() => {})();
            expect(form.areaOfEffect.error.value).toContain("Area of Effect is a required field");
        });

        it("Label is correct", () => {
            expect(form.areaOfEffect.label).toEqual("Area of Effect");
        });
        
    });

    describe("Power Level", () => {
        it("No errors when it's a valid value", async () => {
            form.powerLevel.field.value = listItem1;
            await form.handleSubmit(() => {})();
            expect(form.powerLevel.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            form.powerLevel.field.value = undefined;
            await form.handleSubmit(() => {})();
            expect(form.powerLevel.error.value).toContain("Power Level is a required field");
        });

        it("Label is correct", () => {
            expect(form.powerLevel.label).toEqual("Power Level");
        });
        
    });

    describe("Power Activation Type", () => {
        it("No errors when it's a valid value", async () => {
            form.powerActivationType.field.value = listItem1;
            await form.handleSubmit(() => {})();
            expect(form.powerActivationType.error.value).toBeUndefined();
        });

        it("Is Required", async () => {
            form.powerActivationType.field.value = undefined;
            await form.handleSubmit(() => {})();
            expect(form.powerActivationType.error.value).toContain("Power Activation Type is a required field");
        });

        it("Label is correct", () => {
            expect(form.powerActivationType.label).toEqual("Power Activation Type");
        });
    });

    describe("Is Power Use", () => {

        // TODO: Figure out way to test default value in vee validate form
        /*it("Has a default value of false", async () => {
            expect(form.isPowerUse.field.value).toBe(false);
            expect(form.isPowerUse.error.value).toBeUndefined();
        });*/
        
        it("No errors when it's set to true", async () => {
            form.isPowerUse.field.value = true;
            await form.handleSubmit(() => {})();
            expect(form.isPowerUse.error.value).toBeUndefined();
        });

        it("No errors when it's set to false", async () => {
            form.isPowerUse.field.value = false;
            await form.handleSubmit(() => {})();
            expect(form.isPowerUse.error.value).toBeUndefined();
        });

        it("Label is correct", () => {
            expect(form.isPowerUse.label).toEqual("Is Power Use");
        });
    });

    describe("Set Values updates the validator values", () => {
        it("Sets name", async () => {
            form.setValues({
                name: "name 1"
            })
            
           expect(form.name.field.value).toBe("name 1");
        })

        it("Sets Description", async () => {
            form.setValues({
                description: "description 1"
            })

            expect(form.description.field.value).toBe("description 1");
        })

        it("Sets Game Mechanic Effect", async () => {
            form.setValues({
                gameMechanicEffect: "game mechanic effect 1"
            })

            expect(form.gameMechanicEffect.field.value).toBe("game mechanic effect 1");
        })

        it("Sets Limitation", async () => {
            form.setValues({
                limitation: "limitation 1"
            })

            expect(form.limitation.field.value).toBe("limitation 1");
        })

        it("Sets Other", async () => {
            form.setValues({
                other: "other 1"
            })

            expect(form.other.field.value).toBe("other 1");
        })

        it("Sets Power Duration", async () => {
            form.setValues({
                powerDuration: 1
            })

            expect(form.powerDuration.field.value).toBe(1);
        })

        it("Sets Area of Effect", async () => {
            form.setValues({
                areaOfEffect: 2
            })

            expect(form.areaOfEffect.field.value).toBe(2);
        })

        it("Sets Power Level", async () => {
            form.setValues({
                powerLevel: 2
            })

            expect(form.powerLevel.field.value).toBe(2);
        })

        it("Sets Power Activation Type", async () => {
            form.setValues({
                powerActivationType: 3
            })

            expect(form.powerActivationType.field.value).toBe(3);
        })

        it("Sets Categories", async () => {
            form.setValues({
                categories: 4
            })

            expect(form.category.field.value).toBe(4);
        })
    })
});
