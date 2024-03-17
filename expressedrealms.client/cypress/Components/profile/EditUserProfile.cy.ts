import addUserProfile from "../../../src/components/profile/EditUserProfile.vue";

const name = 'name';
const nameHelp = 'name-help';
const phoneNumber = 'phone-number';
const phoneNumberHelp = 'phone-number-help'
const city = 'city';
const cityHelp = 'city-help';
const state = 'state';
const stateHelp = 'state-help';
const updateProfileButton = 'update-profile-button';

describe('<EditUserProfile />', () => {
    beforeEach(() => {

        cy.intercept('GET', '/api/player', {
            statusCode: 200,
            body: {
                name: "Jane Doe",
                phone: "(777) 777-7777",
                city: "Chicago",
                state: "IL"
            }
        }).as('getProfile');
        
        cy.intercept('PUT', '/api/player', {
            statusCode: 200
        }).as('updateProfile');
        
        cy.mount(addUserProfile);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
        cy.dataCy(cityHelp).should('not.be.visible');
        cy.dataCy(stateHelp).should('not.be.visible');
    });

    it('Loading the page should fill in values', () => {
        cy.dataCy(name).should('have.value', 'Jane Doe')
        cy.dataCy(phoneNumber).should('have.value', '(777) 777-7777')
        cy.dataCy(city).should('have.value', 'Chicago');
        cy.dataCy(state).should('have.value', 'IL');
    });
    
    it('Name Field follows all Schema Validations', () => {
        cy.dataCy(name).clear();
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(nameHelp).contains("Name is a required field");
        cy.dataCy(name).type("1".repeat(101), { delay: 0 });
        cy.dataCy(nameHelp).contains("Name must be at most 100 characters");
        cy.dataCy(name).type("{backspace}");
        cy.dataCy(nameHelp).should('not.be.visible');
    });

    it('Phone Number Field follows all Schema Validations', () => {
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(phoneNumberHelp).contains("Phone Number is a required field");
        cy.dataCy(phoneNumber).type("555");
        cy.dataCy(phoneNumberHelp).contains("Format must be (555) 555-5555");
        cy.dataCy(phoneNumber).type("555-5555");
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(phoneNumber).type("555-555-5555");
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(phoneNumber).type("(555) 555-5555");
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(phoneNumber).type("5555555555");
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(phoneNumber).type("555555555555");
        cy.dataCy(phoneNumberHelp).should('not.be.visible');
    });

    it('City Field follows all Schema Validations', () => {
        cy.dataCy(city).clear();
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(cityHelp).contains("City is a required field");
        cy.dataCy(city).type("1".repeat(101), { delay: 0 })
        cy.dataCy(cityHelp).contains("City must be at most 100 characters");
        cy.dataCy(city).type("{backspace}");
        cy.dataCy(cityHelp).should('not.be.visible');
    });

    it('State Field follows all Schema Validations', () => {
        cy.dataCy(state).clear();
        cy.dataCy(updateProfileButton).click();
        cy.dataCy(stateHelp).contains("State is a required field");
        
        cy.dataCy(state).type("SDD", { delay: 0 });
        cy.dataCy(stateHelp).should('not.be.visible');
        
        cy.dataCy(state).clear();
        cy.dataCy(state).type("q");
        cy.dataCy(stateHelp).contains("State must be at least 2 characters");

        cy.dataCy(state).clear();
        cy.dataCy(state).type("qt");
        cy.dataCy(stateHelp).contains("Not a valid state")
        
        const states = ["AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", 
            "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", 
            "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "NE"]
        
        states.forEach((x) => {
            cy.dataCy(state).clear();
            cy.dataCy(state).type(x, {delay: 0});
            cy.dataCy(stateHelp).should('not.be.visible');
        })

    });

    it('Passes Data Through Data To API', () => {
        
        cy.dataCy(name).clear();
        cy.dataCy(name).type("John Doe");
        cy.dataCy(phoneNumber).clear();
        cy.dataCy(phoneNumber).type("5555555555");
        cy.dataCy(city).clear();
        cy.dataCy(city).type("Dallas");
        cy.dataCy(state).clear();
        cy.dataCy(state).type("TX");
        cy.dataCy(updateProfileButton).click();

        cy.get('@updateProfile').its('request.body').should('deep.equal', {
            name: 'John Doe',
            phoneNumber: '(555) 555-5555',
            city: 'Dallas',
            state: 'TX'
        });
    });
    
});