/**
 * Auto-Generated, Do Not Edit
 */
export const FeatureFlags = {
  ShowArchetypeSelection: 'show-archetype-selection',
  ShowFactionDropdown: 'show-faction-dropdown',
  ShowMarketingContactUsPage: 'show-marketing-contact-us',
  ShowFactions: 'show-factions',
  TestReleaseFlag: 'test-feature-flag',
} as const

export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]
