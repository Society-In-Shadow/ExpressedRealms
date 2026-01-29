import { defineStore } from 'pinia'
import axios from 'axios'

import type { ListItem } from '@/types/ListItem'
import toaster from '@/services/Toasters'
import type {
  Contact,
  ContactCharacterSheet,
  ContactFrequency,
  ContactKnowledgeLevels,
  ContactListCharacterSheetResponse,
  ContactListResponse,
  EditContact,
  EditContactResponse,
} from '@/components/characters/character/wizard/contacts/types.ts'
import type { ContactForm } from '@/components/characters/character/wizard/contacts/validators/contactValidations.ts'
import { wizardContentStore } from '@/components/characters/character/wizard/stores/wizardContentStore.ts'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'

const experienceInfo = experienceStore()
const wizardContentInfo = wizardContentStore()

export const contactStore
  = defineStore(`contacts`, {
    state: () => {
      return {
        contacts: [] as Contact[],
        contactsForCharacterSheet: [] as ContactCharacterSheet[],
        knowledgeLevels: [] as ContactKnowledgeLevels[],
        contactFrequency: [] as ContactFrequency[],
        knowledges: [] as ListItem[],
      }
    },
    actions: {
      async getContacts(characterId: number) {
        const response = await axios.get<ContactListResponse>(`/characters/${characterId}/contacts`)
        this.contacts = response.data.contacts
      },
      async getContactsForSheet(characterId: number) {
        const response = await axios.get<ContactListCharacterSheetResponse>(`/characters/${characterId}/contacts/characterSheet`)
        this.contactsForCharacterSheet = response.data.contacts
      },
      async approveContact(characterId: number, contactId: number, approved: boolean) {
        await axios.put(`/characters/${characterId}/contacts/${contactId}/approve`, { isApproved: approved })
        await this.getContactsForSheet(characterId)
        toaster.success(`Successfully Approved Contact!`)
      },
      async getOptions() {
        const response = await axios.get(`/knowledges/summary`)

        this.knowledges = response.data
        this.knowledgeLevels = [
          {
            name: 'Associates Degree (4)',
            levelId: 4,
            cost: 6,
            isDisabled: false,
          },
          {
            name: 'Bachalors Degree (5)',
            levelId: 5,
            cost: 10,
            isDisabled: false,
          },
          {
            name: 'Mastors Degree (6)',
            levelId: 6,
            cost: 16,
            isDisabled: false,
          },
        ]

        this.contactFrequency = [
          {
            frequency: 1,
            cost: 0,
            isDisabled: false,
          },
          {
            frequency: 2,
            cost: 4,
            isDisabled: false,
          },
          {
            frequency: 3,
            cost: 10,
            isDisabled: false,
          },
        ]
      },
      getContact: async function (characterId: number, contactId: number): Promise<EditContact> {
        this.getOptions()
        const response = await axios.get<EditContactResponse>(`/characters/${characterId}/contacts/${contactId}`)

        return {
          id: response.data.id,
          name: response.data.name,
          notes: response.data.notes,
          isApproved: response.data.isApproved,
          knowledge: this.knowledges.find((x: ListItem) => x.id == response.data.knowledgeId) as ListItem,
          knowledgeLevel: this.knowledgeLevels.find((x: ContactKnowledgeLevels) => x.levelId == response.data.knowledgeLevelId) as ContactKnowledgeLevels,
          usesPerWeek: this.contactFrequency.find((x: ContactFrequency) => x.frequency == response.data.usesPerWeek) as ContactFrequency,

        }
      },
      updateContact: async function (values: ContactForm, characterId: number, contactId: number): Promise<void> {
        await axios.put(`/characters/${characterId}/contacts/${contactId}`, {
          contactFrequency: values.frequency.frequency,
          name: values.name,
          notes: values.notes,
          knowledgeLevel: values.knowledgeLevel.levelId,
        })
          .then(async () => {
            await this.getContacts(characterId)
            await experienceInfo.updateExperience(characterId)
            toaster.success('Successfully Updated Knowledge!')
          })
      },
      addContact: async function (characterId: number, values: ContactForm): Promise<void> {
        await axios.post(`/characters/${characterId}/contacts/`, {
          knowledgeId: values.knowledge.id,
          contactFrequency: values.frequency.frequency,
          name: values.name,
          notes: values.notes,
          knowledgeLevel: values.knowledgeLevel.levelId,
        })
          .then(async () => {
            await this.getContacts(characterId)
            await experienceInfo.updateExperience(characterId)
            wizardContentInfo.hideContent()
            toaster.success('Successfully Added Knowledge!')
          })
      },
      deleteContact: async function (characterId: number, contactId: number) {
        let name = 'knowledge'

        const contact = this.contacts.find((x: Contact) => x.id == characterId)
        if (contact)
          name = contact.name

        await axios.delete(`/characters/${characterId}/contacts/${contactId}`)
          .then(async () => {
            wizardContentInfo.hideContent()
            await this.getContacts(characterId)
            await experienceInfo.updateExperience(characterId)

            toaster.success(`Successfully Deleted ${name}!`)
          })
      },
    },
  })
