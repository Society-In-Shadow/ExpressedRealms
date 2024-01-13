describe('Login Testing', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit('/')
    })
    
    it('Base URL redirects to login', () => {
        cy.visit('/');
        cy.location('pathname')
            .should('eq', "/login");
    });
    it('Protected Endpoint redirects to login', () => {
        cy.visit('/characters');
        cy.location('pathname')
            .should('include', "login");
    });
    it('Redirects to Create Account When Button is Clicked', () => {
        cy.dataCy("create-account").click();
        cy.location('pathname')
            .should('eq', "/CreateAccount");
    })
    it('Redirects to Forgot Password When Button is Clicked', () => {
        cy.dataCy("forgot-password").click();
        cy.location('pathname')
            .should('eq', "/ForgotPassword");
    })
})