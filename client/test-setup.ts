// test/test-setup.ts
if (!global.ResizeObserver) {
    global.ResizeObserver = class ResizeObserver {
        observe() {
            // Mock the observe method (no-op)
        }
        unobserve() {
            // Mock the unobserve method (no-op)
        }
        disconnect() {
            // Mock the disconnect method (no-op)
        }
    };
}
