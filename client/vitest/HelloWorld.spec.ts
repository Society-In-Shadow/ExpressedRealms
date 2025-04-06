import { render, fireEvent } from '@testing-library/vue'; // Import testing utilities
import { describe, it, expect } from 'vitest'; // Import Vitest-specific functions
import HelloWorld from './HelloWorld.vue'; // Import the component to test
import '@testing-library/jest-dom';



describe('HelloWorld Component', () => {
    it('renders properly', () => {
        const { getByText } = render(HelloWorld, { props: { msg: 'Hello Vitest!' } });
        expect(getByText('Hello Vitest!')).toBeInTheDocument();
    });

    it('responds to a button click', async () => {
        const { getByText } = render(HelloWorld);
        const button = getByText('Click Me');

        await fireEvent.click(button);
        expect(getByText('You clicked!')).toBeInTheDocument();
    });
});