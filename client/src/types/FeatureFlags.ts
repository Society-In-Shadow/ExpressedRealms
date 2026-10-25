/**
 * Auto-Generated, Do Not Edit
 */
export const FeatureFlags = {
  ShowArchetypeSelection: 'show-archetype-selection',
  ShowMarketingContactUsPage: 'show-marketing-contact-us',
  ShowPreCheckinFunctionality: 'show-pre-checkin',
  ShowFactions: 'show-factions',
  TestReleaseFlag: 'test-feature-flag',
} as const

export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]
