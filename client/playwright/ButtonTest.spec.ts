import { test, expect } from '@playwright/experimental-ct-vue';
import AddCharacter from './ButtonTest.vue';

test('should render the button with the correct label and emit event on click', async ({ mount }) => {
    // Mount the Vue component with props
    const component = await mount(AddCharacter, {
        props: {
            buttonLabel: 'Add Character',
        },
    });

    // Verify the button has the correct text
    const button = component.locator('button');
    await expect(button).toHaveText('Add Character');

    // Simulate button click
    await button.click();

    // Verify the emitted event
    const emittedEvents = await component.evaluate((node: any) => node.emitted());
    expect(emittedEvents.onCharacterAdded).toBeDefined(); // Check the event exists
    expect(emittedEvents.onCharacterAdded[0]).toEqual(['New Character']); // Check the event payload
});