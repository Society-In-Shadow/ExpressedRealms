import addUserProfile from "../../../../src/components/characters/character/AddCharacter.vue";

const name = 'name';
const nameHelp = 'name-help';
const expression = 'expression';
const expressionHelp = 'expression-help';
const background = 'background';
const backgroundHelp = 'background-help'

const addCharacterButton = 'add-character-button';
const expressionValues = [
    { id: 1, name: "Foo", shortDescription: "Bar" },
    { id: 2, name: "Boo", shortDescription: "Goo" }
]



describe('<AddCharacter />', () => {
    beforeEach(() => {

        cy.intercept('POST', '/api/characters', {
            statusCode: 200,
        }).as('addProfile');

        cy.intercept('GET', '/api/characters/options', {
            statusCode: 200,
            body: {
                expressions: expressionValues
            }
        }).as('addOptions');
        
        cy.mount(addUserProfile);
    });
    
    it('Loading the page doesn\'t validate right away', () => {
        cy.dataCy(nameHelp).should('not.be.visible');
        cy.dataCy(backgroundHelp).should('not.be.visible');
    });
    
    it('Name Field follows all Schema Validations', () => {
        cy.dataCy(name).clear();
        cy.dataCy(addCharacterButton).click();
        cy.dataCy(nameHelp).contains("Name is a required field");
        cy.dataCy(name).type("1".repeat(151), { delay: 0 });
        cy.dataCy(nameHelp).contains("Name must be at most 150 characters");
        cy.dataCy(name).type("{backspace}");
        cy.dataCy(nameHelp).should('not.be.visible');
    });
    
    it('Expression Field Populates Data', () => {
        cy.dataCy(addCharacterButton).click();
        cy.dataCy(expressionHelp).contains("Expression is a required field");
        cy.dataCy(expression).click();
        cy.get("#expression_list li").each(($ele, i) => {
            expect($ele).to.have.text(expressionValues[i].name)
        });
        cy.get("#expression_0").click();
        cy.dataCy(expressionHelp).should('not.be.visible');
        cy.dataCy("expression-short-description").contains(expressionValues[0].shortDescription);
    })

    it('Passes Data Through Data To API', () => {
        
        cy.dataCy(name).clear();
        cy.dataCy(name).type("John Doe");
        cy.dataCy(expression).click();
        cy.get("#expression_0").click();
        cy.dataCy(background).clear();
        cy.dataCy(background).type("5555555555");
        cy.dataCy(addCharacterButton).click();

        cy.get('@addProfile').its('request.body').should('deep.equal', {
            name: 'John Doe',
            expressionId: expressionValues[0].id,
            background: '5555555555',
        });
    });
    
});