import * as js from '@eslint/js'
import * as globals from 'globals'
import * as tseslint from 'typescript-eslint'
import * as pluginVue from 'eslint-plugin-vue'
import * as tsPlugin from '@stylistic/eslint-plugin'
import pluginCypress from 'eslint-plugin-cypress'
import pluginSonarjs from 'eslint-plugin-sonarjs'
import { defineConfig } from 'eslint/config'

export default defineConfig([
  {
    ignores: [
      'dist/',
      'node_modules/',
      'coverage/',
      '*.config.js',
    ],
  },

  {
    files: ['**/*.{js,mjs,cjs,ts,mts,cts,vue}'],
    plugins: {
      js,
    },
    extends: [
      'js/recommended',
    ],
    languageOptions: {
      globals: globals.browser,
    },
    rules: {
      'vue/max-attributes-per-line': [
        'error',
        {
          singleline: { max: 5 },
          multiline: { max: 5 },
        },
      ],
      'no-console': 'error',
      'vue/no-empty-component-block': 'error',
      'no-multiple-empty-lines': ['error', { max: 1, maxEOF: 1 }],
    },
  },

  tseslint.configs.recommended,
  pluginCypress.configs.recommended,
  pluginSonarjs.configs.recommended,
  tsPlugin.default.configs.recommended,
  pluginVue.configs['flat/recommended'],

  {
    files: ['**/*.vue'],
    languageOptions: {
      parserOptions: {
        parser: tseslint.parser,
      },
    },
  },
])
