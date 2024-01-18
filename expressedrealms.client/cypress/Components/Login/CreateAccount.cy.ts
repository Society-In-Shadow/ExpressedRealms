import CreateAccount from "../../../src/components/Login/CreateAccount.vue";

describe('<CreateAccount />', () => {
    beforeEach(() => {
        
        cy.intercept('GET', '/api/auth/isLoggedIn', {
            body: ['false'],
        });

        cy.intercept('POST', '/api/auth/createaccount', {
            statusCode: 200
        }).as('createUser');

        cy.mount(CreateAccount);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy('email-help').should('not.be.visible');
        cy.dataCy('password-help').should('not.be.visible');
        cy.dataCy('confirm-password-help').should('not.be.visible');
    });

    it('Creating Account Without Anything Filled In Shows 3 Error Messages', () => {
        cy.dataCy('create-account-button').click();
        cy.dataCy('email-help').contains("Email address is a required field")
        cy.dataCy('password-help').contains("Password is a required field")
        cy.dataCy('confirm-password-help').contains("Confirm password is a required field");
    });

    it('Submits A new User successfully', () => {
        cy.dataCy('email').type('example@example.com');
        cy.dataCy('password').type('Password1!');
        cy.dataCy('confirm-password').type('Password1!');
        cy.dataCy('create-account-button').click();

        cy.get('@createUser').its('request.body').should('deep.equal', {
            email: 'example@example.com',
            password: 'Password1!'
        })
    });
    
    it('Email Permutations', () => {
        cy.dataCy('email').type("foo");
        cy.dataCy('email-help').contains("Email address must be a valid email");
        cy.dataCy('email').clear().type("foo@");
        cy.dataCy('email-help').contains("Email address must be a valid email");
        cy.dataCy('email').clear().type("foo@example.com");
        cy.dataCy('email-help').should('not.be.visible');
    });

    describe("All Passwords", () => {
        it('Password must be at least 8 characters', () => {
            cy.dataCy('password').clear().type("foo");
            cy.dataCy('password-help').contains('Password must be at least 8 characters');
        });
        it('Password is a required field', () => {
            cy.dataCy('password').type("foo").clear();
            cy.dataCy('password-help').contains("Password is a required field")
        });
        it('Password Requires a Number', () => {
            cy.dataCy('password').clear().type("asdfjkla!A");
            cy.dataCy('password-help').contains('Password requires a number');
        });
        it('Password requires a upper case letter', () => {
            cy.dataCy('password').clear().type("asdfjkla!1");
            cy.dataCy('password-help').contains('Password requires an uppercase letter');
        });
        it('Password requires a lower case letter', () => {
            cy.dataCy('password').clear().type("AAAAAA!1A");
            cy.dataCy('password-help').contains('Password requires a lowercase letter');
        });
        it('Password requires a special character', () => {
            cy.dataCy('password').clear().type("AAAAAAAA1aA");
            cy.dataCy('password-help').contains('Password requires a symbol');
        });
    });
    
    describe("All Confirm Passwords", () => {

        beforeEach(() => {
            cy.dataCy('password').type("Asdf1234@");
        })
        it('Password must be at least 8 characters', () => {
            cy.dataCy('confirm-password').type("foo").clear();
            cy.dataCy('confirm-password-help').contains('Passwords must match');
        });
        it('Password is a required field', () => {
            cy.dataCy('confirm-password').type("foo").clear();
            cy.dataCy('password').clear();
            cy.dataCy('confirm-password-help').contains("Confirm password is a required field")
        });
    });
})