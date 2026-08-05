import { beforeEach, describe, expect, it } from 'vitest'
import {
  getValidationInstance,
} from '../../../../src/components/expressions/expression/validators/addExpressionValidation'

describe('Add Expression Schema - Field Validations', () => {
  let form: ReturnType<typeof getValidationInstance>

  beforeEach(() => {
    form = getValidationInstance()
    form.setValues()
  })

  describe('Name', () => {
    it('Fails when there are more then 50 characters', async () => {
      form.fields.name.field.value = 'a'.repeat(51)
      await form.handleSubmit(() => {})()
      expect(form.fields.name.error.value).toEqual('Name must be at most 50 characters')
    })

    it('Says it\'s required when not filled in', async () => {
      form.fields.name.field.value = ''
      await form.handleSubmit(() => {})()
      expect(form.fields.name.error.value).toEqual('Name is a required field')
    })

    it('No Errors when it\'s a valid value', async () => {
      form.fields.name.field.value = 'asdf'
      await form.handleSubmit(() => {})()
      expect(form.fields.name.error.value).toBeUndefined()
    })

    it('Label is correct', async () => {
      expect(form.fields.name.label).toEqual('Name')
    })
  })

  describe('Short Description', () => {
    it('Fails when there are more then 125 characters', async () => {
      form.fields.shortDescription.field.value = 'a'.repeat(126)
      await form.handleSubmit(() => {})()
      expect(form.fields.shortDescription.error.value).toEqual('Short Description must be at most 125 characters')
    })

    it('Says it\'s required when not filled in', async () => {
      form.fields.shortDescription.field.value = ''
      await form.handleSubmit(() => {})()
      expect(form.fields.shortDescription.error.value).toEqual('Short Description is a required field')
    })

    it('Label is correct', async () => {
      expect(form.fields.shortDescription.label).toEqual('Short Description')
    })

    it('No Errors when it\'s a valid value', async () => {
      form.fields.shortDescription.field.value = 'asdf'
      await form.handleSubmit(() => {})()
      expect(form.fields.shortDescription.error.value).toBeUndefined()
    })
  })

  describe('Nav Menu Icon', () => {
    it('Default Value is \'emergency_home\'', async () => {
      expect(form.fields.navMenuImage.field.value).toEqual('emergency_home')
    })

    it('Says it\'s required when not filled in', async () => {
      form.fields.navMenuImage.field.value = ''
      await form.handleSubmit(() => {})()
      expect(form.fields.navMenuImage.error.value).toEqual('Nav Menu Icon is a required field')
    })

    it('No Errors when it\'s a valid value', async () => {
      form.fields.navMenuImage.field.value = 'asdf'
      await form.handleSubmit(() => {})()
      expect(form.fields.navMenuImage.error.value).toBeUndefined()
    })

    it('Label is correct', async () => {
      expect(form.fields.navMenuImage.label).toEqual('Nav Menu Icon')
    })
  })
})
