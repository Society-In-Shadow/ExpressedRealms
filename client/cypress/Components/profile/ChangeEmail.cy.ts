import ResetPassword from "../../../src/components/profile/ChangeEmail.vue";

const email = 'email';
const emailHelp = 'email-help'
const confirmEmail = 'confirm-email';
const confirmEmailHelp = 'confirm-email-help';
const resetEmailButton = 'reset-email-button';

describe('<ChangeEmail />', () => {
    beforeEach(() => {
        
        cy.intercept('POST', '/api/auth/manage/info', {
            statusCode: 200
        }).as('changePassword');

        cy.mount(ResetPassword);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(emailHelp).should('not.be.visible');
        cy.dataCy(confirmEmailHelp).should('not.be.visible');
    });

    it('Resetting Email Without Anything Filled In Shows 2 Error Messages', () => {
        cy.dataCy(resetEmailButton).click();
        cy.dataCy(emailHelp).contains("Email is a required field");
        cy.dataCy(confirmEmailHelp).contains("Email is a required field");
    });

    it('Email Permutations', () => {
        cy.dataCy(email).type("foo");
        cy.dataCy(emailHelp).contains("Email must be a valid email");
        cy.dataCy(email).should('have.class', 'p-invalid');
        cy.dataCy(email).clear();
        cy.dataCy(email).type("foo@");
        cy.dataCy(emailHelp).contains("Email must be a valid email");
        cy.dataCy(email).clear();
        cy.dataCy(email).type("foo@example.com");
        cy.dataCy(emailHelp).should('not.be.visible');
        
    });
    
    it('Confirm Email must match Email', () => {
        
        cy.dataCy(email).type("foo@example.com");
        cy.dataCy(confirmEmail).type("foo@example1.com");
        cy.dataCy(resetEmailButton).click();
        cy.dataCy(confirmEmailHelp).should('be.visible');
        cy.dataCy(confirmEmailHelp).contains('Emails must match');
        cy.dataCy(confirmEmail).should('have.class', 'p-invalid');
        
    });

    describe("Resetting Password Handles API", () => {
        it('Passes Data Through and Display Success Message', () => {
            cy.dataCy(email).type("example@example.com")
            cy.dataCy(confirmEmail).type("example@example.com")
            cy.dataCy(resetEmailButton).click();

            cy.get('@changePassword').its('request.body').should('deep.equal', {
                newEmail: 'example@example.com',
            });
            
            cy.dataCy("successful-change-email").should("be.visible");
        });

    })
})
