import CreateAccount from "../../../src/components/Login/CreateAccount.vue";

describe('<CreateAccount />', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.mount(CreateAccount);
    });
    it('Loading the page doesn\'t validate right away', () => {
        cy.get('#email-help').should('not.be.visible');
        cy.get('#password-help').should('not.be.visible');
        cy.get('#confirmPassword-help').should('not.be.visible');
    })

    it('Creating Account Without Anything Filled In Shows 3 Error Messages', () => {
        cy.get('#createAccountButton').click();
        cy.get('#email-help').contains("Email address is a required field")
        cy.get('#password-help').contains("Password is a required field")
        cy.get('#confirmPassword-help').contains("Confirm password is a required field")
    });
    it('Email Permutations', () => {
        cy.get('#email').type("foo");
        cy.get('#email-help').contains("Email address must be a valid email");
        cy.get('#email').clear().type("foo@");
        cy.get('#email-help').contains("Email address must be a valid email");
        cy.get('#email').clear().type("foo@example.com");
        cy.get('#email-help').should('not.be.visible');
    });

    describe("All Passwords", () => {
        it('Password must be at least 8 characters', () => {
            cy.get('#password').clear().type("foo");
            cy.get('#password-help').contains('Password must be at least 8 characters');
        });
        it('Password is a required field', () => {
            cy.get('#password').type("foo").clear();
            cy.get('#password-help').contains("Password is a required field")
        });
        it('Password Requires a Number', () => {
            cy.get('#password').clear().type("asdfjkla!A");
            cy.get('#password-help').contains('Password requires a number');
        });
        it('Password requires a upper case letter', () => {
            cy.get('#password').clear().type("asdfjkla!1");
            cy.get('#password-help').contains('Password requires an uppercase letter');
        });
        it('Password requires a lower case letter', () => {
            cy.get('#password').clear().type("AAAAAA!1A");
            cy.get('#password-help').contains('Password requires a lowercase letter');
        });
        it('Password requires a special character', () => {
            cy.get('#password').clear().type("AAAAAAAA1aA");
            cy.get('#password-help').contains('Password requires a symbol');
        });
    })
    describe("All Confirm Passwords", () => {

        beforeEach(() => {
            cy.get('#password').type("Asdf1234@");
        })
        it('Password must be at least 8 characters', () => {
            cy.get('#confirmPassword').type("foo").clear();
            cy.get('#confirmPassword-help').contains('Passwords must match');
        });
        it('Password is a required field', () => {
            cy.get('#confirmPassword').type("foo").clear();
            cy.get('#password').clear();
            cy.get('#confirmPassword-help').contains("Confirm password is a required field")
        });
    })
})