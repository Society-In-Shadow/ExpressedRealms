import { mount } from '@vue/test-utils';
import AddCharacterTile from '../src/components/characters/character/AddCharacter.vue';
import '@testing-library/jest-dom';
import { afterEach, beforeEach, vi, describe, it, expect } from 'vitest';
import axios from 'axios';

const nameHelp = 'name-help';
const factionHelp = 'faction-help';
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

    beforeEach(() => {
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

    afterEach(() => {
        vi.clearAllMocks();
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

});
