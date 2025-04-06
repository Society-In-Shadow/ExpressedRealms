import { mount } from '@vue/test-utils';
import AddCharacterTile from '../src/components/characters/character/AddCharacter.vue';
import '@testing-library/jest-dom';
import { afterEach, beforeAll, afterAll, vi, describe, it, expect } from 'vitest';
import axios from 'axios';
import { render, fireEvent } from '@testing-library/vue'; // Import testing utilities
import flushPromises from 'flush-promises';
import { nextTick } from 'vue';



const name = 'name';
const nameHelp = 'name-help';
const expression = 'expression';
const expressionHelp = 'expression-help';
const faction = 'faction';
const factionHelp = 'faction-help';
const background = 'background';
const backgroundHelp = 'background-help'

const expressionValues = [
    { id: 1, name: "Foo", shortDescription: "Bar" },
    { id: 2, name: "Boo", shortDescription: "Goo" }
]

const factionValues = [
    { id: 4, name: "Too", description: "Far" },
    { id: 5, name: "Loo", description: "Yoo" }
]

const factionValues2 = [
    { id: 6, name: "Hoo", description: "Gar" },
    { id: 7, name: "Moo", description: "Boo" }
]

describe('<AddCharacterTile />', () => {

    afterEach(() => {
        vi.spyOn(axios, 'get').mockImplementation((url: string) => {
            if (url === '/characters/options') {
                return Promise.resolve({ data: { expressions: expressionValues} }); // Mock data for specific URL
            }
            if (url === '/characters/factionOptions/1') {
                return Promise.resolve({ data: factionValues });
            }
            if (url === '/characters/factionOptions/2') {
                return Promise.resolve({ data: factionValues2 });
            }
            // Default behavior for other URLs
            return Promise.resolve({ data: [] });
        });

        vi.spyOn(axios, 'post').mockImplementation((url: string) => {
            // Default behavior for other URLs
            return Promise.resolve({ data: [] });
        });
    });
    
    it('renders without errors', () => {
        const wrapper = mount(AddCharacterTile);
        expect(wrapper.exists()).toBe(true);
    });

    it('Loading the component doesn\'t validate right away', async () => {
        const wrapper = mount(AddCharacterTile);

        expect(wrapper.find(`[data-cy="${nameHelp}"]`).element).not.toBeVisible();
        expect(wrapper.find(`[data-cy="${backgroundHelp}"]`).element).not.toBeVisible();
        expect(wrapper.find(`[data-cy="${factionHelp}"]`).exists()).toBe(false);

    });

    it('Name Field follows all Schema Validations', async () => {
        const wrapper = mount(AddCharacterTile);

        // Variables for `data-cy` selectors
        const name = '[data-cy="name"]';
        const addCharacterButton = '[data-cy="add-character-button"]';
        const nameHelp = '[data-cy="name-help"]';

        // Clear Name field
        const nameInput = wrapper.find(name);
        await nameInput.setValue('asdf'); // Simulates `cy.dataCy(name).clear()`

        expect(nameInput.element.value).toBe('asdf');

        expect(wrapper.find('[data-cy="name-help"]').isVisible()).toBe(false);
        
        await wrapper.find(addCharacterButton).trigger('click')
        
        await nextTick();
        // Click Add Character button

        /*await wrapper.vm.$nextTick();
        await flushPromises();*/
        
        //await wrapper.find("form").trigger("submit.prevent"); // Trigger the form submission


        expect(wrapper.find('[data-cy="name-help"]').isVisible()).toBe(true);
        
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
