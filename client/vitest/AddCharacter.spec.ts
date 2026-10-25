import { mount } from '@vue/test-utils'
import AddCharacterTile from '../src/components/characters/wizard/basicInfo/AddCharacter.vue'
import '@testing-library/jest-dom'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import axios from 'axios'

const nameHelp = 'name-help'
const factionHelp = 'faction-help'

const expressionValues = [
  { id: 1, name: 'Foo', shortDescription: 'Bar' },
  { id: 2, name: 'Boo', shortDescription: 'Goo' },
]

describe('<AddCharacterTile />', () => {
  beforeEach(() => {
    vi.spyOn(axios, 'get').mockImplementation((url: string) => {
      if (url === '/characters/options') {
        return Promise.resolve({ data: { expressions: expressionValues } }) // Mock data for specific URL
      }
      // Default behavior for other URLs
      return Promise.resolve({ data: [] })
    })

    vi.spyOn(axios, 'post').mockImplementation((url: string) => {
      // Default behavior for other URLs
      return Promise.resolve({ data: [] })
    })
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders without errors', () => {
    const wrapper = mount(AddCharacterTile)
    expect(wrapper.exists()).toBe(true)
  })

  it('Loading the component doesn\'t validate right away', async () => {
    const wrapper = mount(AddCharacterTile)

    expect(wrapper.find(`[data-cy="${nameHelp}"]`).element).not.toBeVisible()
    expect(wrapper.find(`[data-cy="${factionHelp}"]`).exists()).toBe(false)
  })
})
