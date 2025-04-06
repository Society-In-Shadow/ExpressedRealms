import { mount } from '@vue/test-utils'; // Helper to mount the component
import PrimeVue from 'primevue/config'; // PrimeVue plugin
import AddCharacterTile from '../src/components/characters/character/AddCharacter.vue'; // Your component
import { createRouter, createWebHistory } from 'vue-router'; // Vue Router
import '@testing-library/jest-dom';

const name = 'name';
const nameHelp = 'name-help';
const expression = 'expression';
const expressionHelp = 'expression-help';
const faction = 'faction';
const factionHelp = 'faction-help';
const background = 'background';
const backgroundHelp = 'background-help'

// Create a mock router
const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/', // Add a route for `/`
            name: 'Home',
            component: { template: '<div>Home</div>' }, // Temporary placeholder component
        },

        { path: '/characters/add', name: 'AddCharacter' }, // Example route
    ],
});

describe('<AddCharacterTile />', () => {
    beforeEach(async () => {
        // Ensure router is ready before each test
        router.push('/');
        await router.isReady();
    });

    it('renders without errors', () => {
        // Mount the component and inject PrimeVue and the router
        const wrapper = mount(AddCharacterTile, {
            global: {
                plugins: [PrimeVue, router],
            },
        });

        // Assert that the component exists in the DOM
        expect(wrapper.exists()).toBe(true);
    });

    it('Loading the component doesn\'t validate right away', async () => {
        const wrapper = mount(AddCharacterTile, {
            global: {
                plugins: [PrimeVue, router],
            },
        });

        expect(wrapper.find(`[data-cy="${nameHelp}"]`).element).not.toBeVisible();
        expect(wrapper.find(`[data-cy="${backgroundHelp}"]`).element).not.toBeVisible();
        expect(wrapper.find(`[data-cy="${factionHelp}"]`).exists()).toBe(false);

    });

    it('Name Field follows all Schema Validations', async () => {
        const wrapper = mount(AddCharacterTile, {
            global: {
                plugins: [PrimeVue, router],
            },
        });

        // Variables for `data-cy` selectors
        const name = '[data-cy="name"]';
        const addCharacterButton = '[data-cy="add-character-button"]';
        const nameHelp = '[data-cy="name-help"]';

        // Clear Name field
        const nameInput = wrapper.find(name);
        await nameInput.setValue(''); // Simulates `cy.dataCy(name).clear()`

        // Click Add Character button
        const button = wrapper.find(addCharacterButton);
        await button.trigger('click'); // Simulates `cy.dataCy(addCharacterButton).click()`

        // Assert validation message for required Name
        const nameHelpEl = wrapper.find(nameHelp);
        expect(nameHelpEl.text()).toBe('Name is a required field'); // Simulates `cy.dataCy(nameHelp).contains(...)`

        // Enter 151 characters in the Name field
        const longName = '1'.repeat(151);
        await nameInput.setValue(longName); // Simulates `cy.dataCy(name).type(...)`

        // Assert validation message for maximum character limit
        expect(nameHelpEl.text()).toBe('Name must be at most 150 characters');

        // Simulate backspace (shorten the name to 150 characters)
        const validName = '1'.repeat(150);
        await nameInput.setValue(validName); // Simulates removing one character with backspace

        // Assert that the validation message is no longer visible
        expect(nameHelpEl.exists()).toBe(true); // Name Help element still exists in the DOM
        expect(nameHelpEl.element).not.toBeVisible(); // Validation message should not be visible


    });
});
