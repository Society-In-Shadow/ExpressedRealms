describe('Create Account Testing', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit('/CreateAccount')

        cy.intercept('POST', '/api/auth/resetPassword', {
            statusCode: 200
        }).as('resetPassword');
    })
    
    it('Back Button Redirects back to Login Page', () => {
        cy.dataCy("back-button").click();
        cy.location('pathname')
            .should('eq', "/login");
    });
    
    it('Successfully Submits Reset Password, Then Redirects Back to Login With Flag', () => {
        cy.visit('/resetPassword?resetToken=asdf1234')

        cy.dataCy('email').type('example@example.com');
        cy.dataCy('password').type('Password1!');
        cy.dataCy('confirm-password').type('Password1!');
        cy.dataCy('reset-password-button').click();
        
        cy.get('@resetPassword').its('request.body').should('deep.equal', {
            email: 'example@example.com',
            resetCode: 'asdf1234',
            newPassword: 'Password1!'
        });
        
        // This should be working, but not sure why.  Manually testing seems to work though.
        cy.location('pathname').should('eq', "/login");
        cy.location('search').should('eq', "?resetPassword=1");
    });
})