import addUserProfile from "../../../../src/components/characters/character/AddCharacter.vue";

const name = 'name';
const nameHelp = 'name-help';
const background = 'background';
const backgroundHelp = 'background-help'
const addCharacterButton = 'add-character-button';

describe('<AddCharacter />', () => {
    beforeEach(() => {

        cy.intercept('POST', '/api/characters', {
            statusCode: 200,
        }).as('addProfile');
        
        cy.mount(addUserProfile);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(backgroundHelp).should('not.be.visible');
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

    it('Passes Data Through Data To API', () => {
        
        cy.dataCy(name).clear();
        cy.dataCy(name).type("John Doe");
        cy.dataCy(background).clear();
        cy.dataCy(background).type("5555555555");
        cy.dataCy(addCharacterButton).click();

        cy.get('@addProfile').its('request.body').should('deep.equal', {
            name: 'John Doe',
            background: '5555555555',
        });
    });
    
});