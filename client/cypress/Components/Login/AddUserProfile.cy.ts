import addUserProfile from "../../../src/components/Login/AddUserProfile.vue";

const name = 'name';
const nameHelp = 'name-help';
const updateProfileButton = 'update-profile-button';

describe('<AddUserProfile />', () => {
    beforeEach(() => {
        
        cy.intercept('POST', '/player', {
            statusCode: 200
        }).as('updateProfile');

        cy.mount(addUserProfile);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
    });

    it('Creating Profile Without Anything Filled In Shows All Error Messages', () => {
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(nameHelp).contains("Name is a required field");
    });
    
    it('Name Field follows all Schema Validations', () => {
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(nameHelp).contains("Name is a required field");
        cy.dataCy(name).type("1".repeat(101), { delay: 0 });
        cy.dataCy(nameHelp).contains("Name must be at most 100 characters");
        cy.dataCy(name).type("{backspace}");
        cy.dataCy(nameHelp).should('not.be.visible');
    });

    it('Passes Data Through Data To API', () => {
        cy.dataCy(name).type("John Doe");
        cy.dataCy(updateProfileButton).click();

        cy.get('@updateProfile').its('request.body').should('deep.equal', {
            name: 'John Doe'
        });
    });
    
});
