import addUserProfile from "../../../../src/components/characters/character/AddCharacter.vue";

const name = 'name';
const nameHelp = 'name-help';
const expression = 'expression';
const expressionHelp = 'expression-help';
const faction = 'faction';
const factionHelp = 'faction-help';
const background = 'background';
const backgroundHelp = 'background-help'

const addCharacterButton = 'add-character-button';
const expressionValues = [
    { id: 1, name: "Foo", shortDescription: "Bar" },
    { id: 2, name: "Boo", shortDescription: "Goo" }
]

const factionValues = [
    { id: 4, name: "Too", description: "Far" },
    { id: 5, name: "Loo", description: "Yoo" }
]

const factionValues2 = [
    { id: 6, name: "Hoo", description: "Gar" },
    { id: 7, name: "Moo", description: "Boo" }
]

describe('<AddCharacter />', () => {
    beforeEach(() => {

        cy.intercept('POST', '/characters', {
            statusCode: 200,
        }).as('addProfile');

        cy.intercept('GET', '/characters/options', {
            statusCode: 200,
            body: {
                expressions: expressionValues
            }
        }).as('addOptions');

        cy.intercept('GET', '/characters/factionOptions/1', {
            statusCode: 200,
            body: factionValues
        }).as('factionOptions');

        cy.intercept('GET', '/characters/factionOptions/2', {
            statusCode: 200,
            body: factionValues2
        }).as('factionOptions2');
        
        cy.mount(addUserProfile);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(backgroundHelp).should('not.be.visible');
        cy.dataCy(factionHelp).should('not.exist');
    });
    
    it('Name Field follows all Schema Validations', () => {
        cy.dataCy(name).clear();
        cy.dataCy(addCharacterButton).click();
        cy.dataCy(nameHelp).contains("Name is a required field");
        cy.dataCy(name).type("1".repeat(151), { delay: 0 });
        cy.dataCy(nameHelp).contains("Name must be at most 150 characters");
        cy.dataCy(name).type("{backspace}");
        cy.dataCy(nameHelp).should('not.be.visible');
    });
    
    it('Expression Field Populates Data', () => {
        cy.dataCy(addCharacterButton).click();
        cy.dataCy(expressionHelp).contains("Expression is a required field");
        cy.dataCy(expression).click();
        cy.get("#expression_list li").each(($ele, i) => {
            expect($ele).to.have.text(expressionValues[i].name)
        });
        cy.get("#expression_0").click();
        cy.dataCy(expressionHelp).should('not.be.visible');
        cy.dataCy("expression-short-description").contains(expressionValues[0].shortDescription);
    })

    it('Faction Field Populates Data', () => {
        // Faction needs to be disabled until after an expression has been selected
        cy.dataCy(faction).should('not.exist');
        cy.dataCy(expression).click();
        cy.get("#expression_0").click();
        cy.get("@factionOptions");
        // Select after selecting, it should now be visable and testable
        cy.dataCy(faction).should('exist');
        cy.dataCy(addCharacterButton).click();
        cy.dataCy(faction).click();
        cy.get("#faction_list li").each(($ele, i) => {
            expect($ele).to.have.text(factionValues[i].name)
        });
        cy.get("#faction_0").click();
        cy.dataCy(factionHelp).should('not.be.visible');
        // If you change expression, it should clear out the old stuff
        cy.dataCy(expression).click();
        cy.get("#expression_1").click();
        cy.get("@factionOptions2")
    })

    it('Passes Data Through Data To API', () => {
        
        cy.dataCy(name).clear();
        cy.dataCy(name).type("John Doe");
        cy.dataCy(expression).click();
        cy.get("#expression_0").click();
        cy.dataCy(faction).click();
        cy.get("#faction_0").click();
        cy.dataCy(background).clear();
        cy.dataCy(background).type("5555555555");
        cy.dataCy(addCharacterButton).click();

        cy.get('@addProfile').its('request.body').should('deep.equal', {
            name: 'John Doe',
            expressionId: expressionValues[0].id,
            background: '5555555555',
            factionId: factionValues[0].id
        });
    });
    
});
