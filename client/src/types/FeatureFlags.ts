/**
 * Auto-Generated, Do Not Edit
 */
export const FeatureFlags = {
  ShowMarketingContactUsPage: 'show-marketing-contact-us',
  ShowPreCheckinFunctionality: 'show-pre-checkin',
  TestReleaseFlag: 'test-feature-flag',
} as const

export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]
