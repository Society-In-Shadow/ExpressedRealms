import Login from "../../../src/components/Login/UserLogin.vue";

describe('<Login />', () => {
  beforeEach(() => {
    // Cypress starts out with a blank slate for each test
    // so we must tell it to visit our website with the `cy.visit()` command.
    // Since we want to visit the same URL at the start of all our tests,
    // we include it in our beforeEach function so that it runs before each test

    cy.intercept('GET', '/auth/antiforgeryToken', {
      statusCode: 200
    }).as('antiforgeryToken');

    cy.intercept('POST', '/auth/login', {
      statusCode: 200
    }).as('login');
    
    cy.intercept('GET', '/auth/getInitialLoginInfo', {
      statusCode: 200
    });

    // needed for the axios interceptor
    cy.window().then((win) => {
      win.history.pushState({}, '', '/login');
    });

    cy.mount(Login);
  });
  it('Loading the page doesn\'t validate right away', () => {
    cy.dataCy('email-help').should('not.be.visible');
    cy.dataCy('password-help').should('not.be.visible');
  })
  it('Signing In Without Anything Filled In Shows Both Error Messages', () => {
    cy.dataCy('sign-in-button').click();
    cy.dataCy('email-help').contains("Email address is a required field")
    cy.dataCy('password-help').contains("Password is a required field")
  });
  it('Email Permutations', () => {
    cy.dataCy('email').type("foo");
    cy.dataCy('email-help').contains("Email address must be a valid email");
    cy.dataCy('email').clear();
    cy.dataCy('email').type("foo@");
    cy.dataCy('email-help').contains("Email address must be a valid email");
    cy.dataCy('email').clear();
    cy.dataCy('email').type("foo@example.com");
    cy.dataCy('email-help').should('not.be.visible');
  });
  it('Password Permutations', () => {
    cy.dataCy('password').type("foo");
    cy.dataCy('password-help').should('not.be.visible');
    cy.dataCy('password').clear();
    cy.dataCy('password-help').contains("Password is a required field")
  });
  it('Logs In Successfully', () => {
    cy.dataCy('email').type("example@example.com")
    cy.dataCy('password').type('Password1!');
    cy.dataCy('sign-in-button').click();

    cy.get('@login').its('request.body').should('deep.equal', {
      email: 'example@example.com',
      password: 'Password1!'
    });
    
    // Make sure it resets the antiforgery token
    cy.wait('@antiforgeryToken');
    
  });
  it('Shows Email or Password Incorrect Message Upon Login Failure', () => {

    cy.dataCy("error-invalid-login").should('not.exist');

    cy.intercept('POST', '/auth/login', {
      statusCode: 401,
      body: {
        detail: "Failed"
      }
    }).as('login');
    
    cy.dataCy('email').type("example@example.com")
    cy.dataCy('password').type('Password1!');
    cy.dataCy('sign-in-button').click();    
    cy.wait('@login');
    
    cy.dataCy("error-invalid-login").should('be.visible');
        
  });
  it('Shows timeout if too many attempts at logging in', () => {

    cy.dataCy("error-invalid-login").should('not.exist');

    cy.intercept('POST', '/auth/login', {
      statusCode: 401,
      body: {
        detail: "LockedOut"
      }
    }).as('login');

    cy.dataCy('email').type("example@example.com")
    cy.dataCy('password').type('Password1!');
    cy.dataCy('sign-in-button').click();
    cy.wait('@login');

    cy.dataCy("locked-out").should('be.visible');
    cy.dataCy("locked-out").invoke('text')
        .then(t => t.trim())
        .should('contain', "You've been locked out of your account. Please wait 5 minutes before trying again.");

  });
})
