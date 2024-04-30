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
const expressionDefaultValue = "Adept";

describe('<EditCharacter />', () => {
    beforeEach(() => {

        cy.intercept('GET', '/api/characters/3', {
            statusCode: 200,
            body: {
                name: nameDefaultValue,
                background: backgroundDefaultValue,
                expression: expressionDefaultValue
            }
        }).as('getCharacter');
        
        cy.intercept('GET', '/api/characters/3/stats', {
            statusCode: 200,
            body: [{}, {}, {}, {}, {}, {}]
        })

        cy.intercept('PUT', '/api/characters', {
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
    });
    
    it('Loading Page Will Grab Data From API and Load It In', () => {
        cy.dataCy(name).should("have.value", nameDefaultValue);
        cy.dataCy(background).should("have.value", backgroundDefaultValue);
        cy.dataCy(expression).should("have.value", expressionDefaultValue);
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
