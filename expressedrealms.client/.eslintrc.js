/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution')

module.exports = {
  root: true,
  plugins: [
    'vue',
    'cypress',
    'sonarjs',
    '@stylistic/js'
  ],
  extends: [
    'plugin:vue/vue3-recommended',
    'plugin:cypress/recommended',
    'plugin:sonarjs/recommended',
    'eslint:recommended',
    '@vue/eslint-config-typescript',
  ],
  parserOptions: {
    ecmaVersion: 'latest',
  },
  rules: {
    'vue/max-attributes-per-line': ['error', {
      singleline: {
        max: 5
      },
      multiline: {
        max: 5
      }
    }],
    "eol-last": ["error", "always"],
    "no-multiple-empty-lines": [2, {"max": 2, "maxEOF": 1}]
  },
  overrides: [
    {
      files: ['*.cy.ts'],
      rules: {
        'sonarjs/no-duplicate-string': 'off'
      }
    }
  ]
}
