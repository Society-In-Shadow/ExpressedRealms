import Login from "../../../src/components/Login/Login.vue";

describe('<Login />', () => {
  beforeEach(() => {
    // Cypress starts out with a blank slate for each test
    // so we must tell it to visit our website with the `cy.visit()` command.
    // Since we want to visit the same URL at the start of all our tests,
    // we include it in our beforeEach function so that it runs before each test
    cy.mount(Login);
  });
  it('Loading the page doesn\'t validate right away', () => {
    cy.get('#email-help').should('not.be.visible');
    cy.get('#password-help').should('not.be.visible');
  })
  it('Signing In Without Anything Filled In Shows Both Error Messages', () => {
    cy.get('#sign-in-button').click();
    cy.get('#email-help').contains("Email address is a required field")
    cy.get('#password-help').contains("Password is a required field")
  });
  it('Email Permutations', () => {
    cy.get('#email').type("foo");
    cy.get('#email-help').contains("Email address must be a valid email");
    cy.get('#email').clear().type("foo@");
    cy.get('#email-help').contains("Email address must be a valid email");
    cy.get('#email').clear().type("foo@example.com");
    cy.get('#email-help').should('not.be.visible');
  });
  it('Password Permutations', () => {
    cy.get('#password').type("foo");
    cy.get('#password-help').should('not.be.visible');
    cy.get('#password').clear();
    cy.get('#password-help').contains("Password is a required field")
  });
})