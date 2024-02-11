
const password = 'password';
const createAccountButton = 'create-account-button';
const email = 'email';
const confirmPassword = 'confirm-password';

describe('Create Account Testing', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit('/CreateAccount');

        cy.intercept('POST', '/api/auth/register', {
            statusCode: 200
        }).as('createUser');
    })
    
    it('Back Button Redirects back to Login Page', () => {
        cy.dataCy("back-button").click();
        cy.location('pathname')
            .should('eq', "/login");
    });
    
    it('Redirects back to login page, with specific parameters', () => {
        cy.dataCy(email).type('example@example.com');
        cy.dataCy(password).type('Password1!');
        cy.dataCy(confirmPassword).type('Password1!');
        cy.dataCy(createAccountButton).click();
        
        cy.wait('@createUser');

        cy.location('pathname').should('eq', "/login");
        cy.location('search').should('eq', "?createdUser=1");
    });
})