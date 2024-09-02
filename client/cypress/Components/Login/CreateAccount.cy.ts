import CreateAccount from "../../../src/components/Login/CreateAccount.vue";

const emailHelp = 'email-help';
const passwordHelp = 'password-help'
const password = 'password';
const confirmPasswordHelp = 'confirm-password-help';
const createAccountButton = 'create-account-button';
const email = 'email';
const confirmPassword = 'confirm-password';

describe('<CreateAccount />', () => {
    beforeEach(() => {
        
        cy.intercept('POST', '/auth/register', {
            statusCode: 200
        }).as('createUser');

        cy.mount(CreateAccount);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(emailHelp).should('not.be.visible');
        cy.dataCy(passwordHelp).should('not.be.visible');
        cy.dataCy(confirmPasswordHelp).should('not.be.visible');
    });

    it('Creating Account Without Anything Filled In Shows 3 Error Messages', () => {
        cy.dataCy(createAccountButton).click();
        cy.dataCy(emailHelp).contains("Email address is a required field")
        cy.dataCy(passwordHelp).contains("Password is a required field")
        cy.dataCy(confirmPasswordHelp).contains("Confirm password is a required field");
    });

    it('Submits A new User successfully', () => {
        cy.dataCy(email).type('example@example.com');
        cy.dataCy(password).type('Password1!');
        cy.dataCy(confirmPassword).type('Password1!');
        cy.dataCy(createAccountButton).click();

        cy.get('@createUser').its('request.body').should('deep.equal', {
            email: 'example@example.com',
            password: 'Password1!'
        })
    });
    
    it('Email Permutations', () => {
        cy.dataCy(email).type("foo");
        cy.dataCy(emailHelp).contains("Email address must be a valid email");
        cy.dataCy(email).clear();
        cy.dataCy(email).type("foo@");
        cy.dataCy(emailHelp).contains("Email address must be a valid email");
        cy.dataCy(email).clear();
        cy.dataCy(email).type("foo@example.com");
        cy.dataCy(emailHelp).should('not.be.visible');
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
})
