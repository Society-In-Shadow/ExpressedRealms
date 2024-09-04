import component from "../../../../src/components/characters/character/EditCharacter.vue";
import toasters from "../../../../src/services/Toasters";

const name = 'name';
const nameHelp = 'name-help';
const nameDefaultValue = "John Doe";
const background = 'background';
const backgroundHelp = 'background-help'
const backgroundDefaultValue = "The anonymous person";
const expression = "expression";
const expressionHelp = 'expression-help'
const faction = "faction";
const factionHelp = "faction-help"
const factionDefaultValue = 4;
const expressionDefaultValue = "Adept";
const factionValues = [
    { id: 4, name: "Too", description: "Far" },
    { id: 5, name: "Loo", description: "Yoo" }
]
describe('<EditCharacter />', () => {
    beforeEach(() => {

        cy.intercept('GET', '/characters/3', {
            statusCode: 200,
            body: {
                name: nameDefaultValue,
                background: backgroundDefaultValue,
                expression: expressionDefaultValue,
                factionId: factionDefaultValue
            }
        }).as('getCharacter');
        
        cy.intercept('GET', '/characters/3/stats', {
            statusCode: 200,
            body: [{}, {}, {}, {}, {}, {}]
        })
        
        cy.intercept('GET', '/characters/3/factionOptions', {
            statusCode: 200,
            body: factionValues
        }).as('getFactionOptions');

        cy.intercept('PUT', '/characters', {
            statusCode: 200,
        }).as('updateCharacter');

        cy.spy(toasters, 'success').as("toasterSuccess");

        cy.mount(component, {
            pushRoute: '/characters/3'
        });
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(backgroundHelp).should('not.be.visible');
        cy.dataCy(expressionHelp).should('not.be.visible');
        cy.dataCy(factionHelp).should('not.be.visible');
    });
    
    it('Loading Page Will Grab Data From API and Load It In', () => {
        cy.get('@getFactionOptions');
        cy.dataCy(name).should("have.value", nameDefaultValue);
        cy.dataCy(background).should("have.value", backgroundDefaultValue);
        cy.dataCy(expression).should("have.value", expressionDefaultValue);
        cy.dataCy(faction).contains(factionValues.find(x => x.id == factionDefaultValue).name);
    });
    
    it('Name Field follows all Schema Validations and Updates Automatically', () => {
        cy.dataCy(name).clear();
        cy.dataCy(nameHelp).contains("Name is a required field");
        cy.dataCy(name).type("1".repeat(151), { delay: 0 });
        cy.dataCy(nameHelp).contains("Name must be at most 150 characters");
        cy.dataCy(name).type("{backspace}");
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(name).clear();
        cy.dataCy(name).type("Jane Doe");
        cy.dataCy(name).focus();

        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'name', 'Jane Doe');
        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'id', '3');

        cy.get('@toasterSuccess').should('have.been.calledWith', 'Successfully Updated Character Info!');
    });
    
    it('Expression Field is Disabled', () => {
        cy.dataCy(expression).should('be.disabled');
    })

    it('Faction Field Populates Data', () => {
        cy.get("@getFactionOptions");
        cy.dataCy(faction).click();
        cy.get("#faction_list li").each(($ele, i) => {
            expect($ele).to.have.text(factionValues[i].name)
        });
        cy.get("#faction_0").click();
        cy.dataCy(factionHelp).should('not.be.visible');

        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'factionId', 4);
        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'id', '3');
        cy.get('@toasterSuccess').should('have.been.calledWith', 'Successfully Updated Character Info!');
    })

    it('Background Follows all Schema Validations and Updates Automatically', () => {
        
        cy.dataCy(background).clear();
        cy.dataCy(background).type("5555555555");
        cy.dataCy(background).focus();

        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'background', '5555555555');
        cy.get('@updateCharacter').its('request.body')
            .should('have.property', 'id', '3');
        cy.get('@toasterSuccess').should('have.been.calledWith', 'Successfully Updated Character Info!');
    });
    
});
