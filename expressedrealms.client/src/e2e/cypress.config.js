const { defineConfig } = require("cypress");

module.exports = defineConfig({
  projectId: '3wpvob',
  e2e: {
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});
