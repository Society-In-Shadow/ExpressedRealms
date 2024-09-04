import ResetPassword from "../../../src/components/profile/ChangePassword.vue";

const currentPassword = 'current-password';
const currentPasswordHelp = 'current-password-help';
const password = 'new-password';
const passwordHelp = 'new-password-help'
const confirmPassword = 'confirm-password';
const confirmPasswordHelp = 'confirm-password-help';
const resetPasswordButton = 'reset-password-button';

describe('<ChangePassword />', () => {
    beforeEach(() => {
        
        cy.intercept('POST', '/auth/manage/info', {
            statusCode: 200
        }).as('changePassword');

        cy.mount(ResetPassword);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(currentPasswordHelp).should('not.be.visible');
        cy.dataCy(passwordHelp).should('not.be.visible');
        cy.dataCy(confirmPasswordHelp).should('not.be.visible');
    });

    it('Creating Account Without Anything Filled In Shows 3 Error Messages', () => {
        cy.dataCy(resetPasswordButton).click();
        cy.dataCy(currentPasswordHelp).contains("Current Password is a required field")
        cy.dataCy(passwordHelp).contains("Password is a required field")
        cy.dataCy(confirmPasswordHelp).contains("Confirm password is a required field");
    });
    
    it('Current Password is Required', () => {
        cy.dataCy(resetPasswordButton).click();
        cy.dataCy(currentPassword).type("foo@example.com");
        cy.dataCy(currentPasswordHelp).should('not.be.visible');
    });

    it('Fields Correctly Highlight Bad Fields', () => {

        cy.dataCy(currentPassword).type("Password1!");
        cy.dataCy(password).type("Password1!");
        cy.dataCy(resetPasswordButton).click();
        cy.dataCy(confirmPassword).should('have.class', 'p-invalid');

        cy.dataCy(confirmPassword).type("Password1!");
        cy.dataCy(currentPassword).clear();
        cy.dataCy(resetPasswordButton).click();
        cy.dataCy(currentPassword).should('have.class', 'p-invalid');

        cy.dataCy(currentPassword).type("Password1!");
        cy.dataCy(confirmPassword).clear();
        cy.dataCy(resetPasswordButton).click();
        cy.dataCy(confirmPassword).should('have.class', 'p-invalid');

        cy.dataCy(currentPasswordHelp).should('not.be.visible');
    });

    describe("All Passwords", () => {
        it('Password must be at least 8 characters', () => {
            cy.dataCy(password).clear();
            cy.dataCy(password).type("foo");
            cy.dataCy(passwordHelp).contains('Password must be at least 8 characters');
        });
        it('Password is a required field', () => {
            cy.dataCy(password).type("foo");
            cy.dataCy(password).clear();
            cy.dataCy(passwordHelp).contains("Password is a required field")
        });
        it('Password Requires a Number', () => {
            cy.dataCy(password).clear();
            cy.dataCy(password).type("asdfjkla!A");
            cy.dataCy(passwordHelp).contains('Password requires a number');
        });
        it('Password requires a upper case letter', () => {
            cy.dataCy(password).clear();
            cy.dataCy(password).type("asdfjkla!1");
            cy.dataCy(passwordHelp).contains('Password requires an uppercase letter');
        });
        it('Password requires a lower case letter', () => {
            cy.dataCy(password).clear();
            cy.dataCy(password).type("AAAAAA!1A");
            cy.dataCy(passwordHelp).contains('Password requires a lowercase letter');
        });
        it('Password requires a special character', () => {
            cy.dataCy(password).clear();
            cy.dataCy(password).type("AAAAAAAA1aA");
            cy.dataCy(passwordHelp).contains('Password requires a symbol');
        });
    });
    
    describe("All Confirm Passwords", () => {

        beforeEach(() => {
            cy.dataCy(password).type("Asdf1234@");
        })
        it('Password must be at least 8 characters', () => {
            cy.dataCy(confirmPassword).type("foo");
            cy.dataCy(confirmPassword).clear();
            cy.dataCy(confirmPasswordHelp).contains('Passwords must match');
        });
        it('Password is a required field', () => {
            cy.dataCy(confirmPassword).type("foo");
            cy.dataCy(confirmPassword).clear();
            cy.dataCy(password).clear();
            cy.dataCy(confirmPasswordHelp).contains("Confirm password is a required field");
        });
    });
    
    describe("Resetting Password Handles API", () => {
        it('Passes Data Through and Display Success Message', () => {
            cy.dataCy(currentPassword).type("Password1!")
            cy.dataCy(password).type("Password2@")
            cy.dataCy(confirmPassword).type("Password2@")
            cy.dataCy(resetPasswordButton).click();

            cy.get('@changePassword').its('request.body').should('deep.equal', {
                oldPassword: 'Password1!',
                newPassword: 'Password2@'
            });
            
            cy.dataCy("successful-change-password").should("be.visible");
        });

        it('Passes Data Through and Shows That Current Password Is Incorrect', () => {

            const invalidPasswordMessage = "Invalid Password";
            cy.intercept('POST', '/auth/manage/info', {
                statusCode: 400,
                body: {
                    errors: {
                        PasswordMismatch: [invalidPasswordMessage]
                    }
                }
            }).as('changePassword');
            
            cy.dataCy(currentPassword).type("Password1!")
            cy.dataCy(password).type("Password2@")
            cy.dataCy(confirmPassword).type("Password2@")
            cy.dataCy(resetPasswordButton).click();
            
            cy.wait("@changePassword");

            cy.dataCy("successful-change-password").should("not.exist");
            cy.dataCy("current-password-help").should("be.visible");
            cy.dataCy("current-password-help").contains(invalidPasswordMessage);
            
        });
    })
})
