describe('Login Testing', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit('/')
    })

    it('After Password Reset, Show Success and Have Them Login', () => {
        cy.visit('/login?resetPassword=1')
        cy.dataCy('success-password-reset-message').should('be.visible')        
        
        cy.visit('/login')
        cy.dataCy('success-password-reset-message').should('not.exist');
    });

    it('After Confirmed Email, Show Success and Have Them Login', () => {
        cy.visit('/login?confirmedEmail=1')
        cy.dataCy('success-confirmed-email-message').should('be.visible')

        cy.visit('/login')
        cy.dataCy('success-confirmed-email-message').should('not.exist');
    });

    it('After Create Account, Show Success and Have Them Login', () => {
        cy.visit('/login?createdUser=1')
        cy.dataCy('success-created-user-message').should('be.visible')

        cy.visit('/login')
        cy.dataCy('success-created-user-message').should('not.exist');
    });

    it('After Sending Reset Password Email, Show Success and Have Them Login', () => {
        cy.visit('/login?forgotPassword=1')
        cy.dataCy('success-forgot-password-message').should('be.visible')

        cy.visit('/login')
        cy.dataCy('success-forgot-password-message').should('not.exist');
    });
    
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