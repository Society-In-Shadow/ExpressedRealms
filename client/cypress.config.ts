import { defineConfig } from 'cypress'

export default defineConfig({
  projectId: '3wpvob',
  retries: {
    runMode: 3, // Retries for `cypress run`
    openMode: 0, // Retries for `cypress open`
  },
  e2e: {
    baseUrl: 'https://172.19.0.6',
  },

  component: {
    devServer: {
      framework: 'vue',
      bundler: 'vite',
    },
    reporter: 'cypress-multi-reporters',
    reporterOptions: {
      reporterEnabled: 'spec, mocha-junit-reporter',
      mochaJunitReporterReporterOptions: {
        mochaFile: 'cypress/results/test-results-[hash].xml', // Path to store XML files
        toConsole: false, // Set to true to log JUnit output in console
      },
    },

  },
})
