export interface StripInfo {
  vitality: number
  health: number
  blood: number
  rwp: number
  psyche: number
  mortis: number
}

export interface ChangedStripInfo {
  vitality: ChangedInfo
  health: ChangedInfo
  blood: ChangedInfo
  rwp: ChangedInfo
  psyche: ChangedInfo
  mortis: ChangedInfo
}

export interface ChangedInfo {
  amount: number
  reasons: string[]
}

// Adept, Sidhe, Sorcerers
export function handleAdeptSidheSorcerers(userDown: StripInfo, userMax: StripInfo): ChangedStripInfo {
  const currentUserStats: StripInfo = {
    blood: userMax.blood - userDown.blood,
    health: userMax.health - userDown.health,
    vitality: userMax.vitality - userDown.vitality,
    rwp: userMax.rwp - userDown.rwp,
    psyche: userMax.psyche - userDown.psyche,
    mortis: userMax.mortis - userDown.mortis,
  }

  const changes: ChangedStripInfo = {
    blood: { amount: 0, reasons: [] },
    health: { amount: 0, reasons: [] },
    vitality: { amount: 0, reasons: [] },
    rwp: { amount: 0, reasons: [] },
    psyche: { amount: 0, reasons: [] },
    mortis: { amount: 0, reasons: [] },
  }

  if (userDown.health > 0) {
    if (currentUserStats.health >= currentUserStats.vitality) {
      changes.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (currentUserStats.health + 1 <= currentUserStats.vitality) {
      changes.health.amount += 1
      changes.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${currentUserStats.health + 1}/${userMax.health}`)
    }
  }

  if (userDown.blood > 0) {
    if (currentUserStats.blood >= currentUserStats.vitality) {
      changes.blood.reasons.push('You did not gain blood as your vitality is lower than or equal to your blood')
    }
    else if (currentUserStats.blood + 1 <= currentUserStats.vitality) {
      changes.blood.amount += 1
      changes.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${currentUserStats.blood + 1}/${userMax.blood}`)
    }
  }

  if (userDown.psyche > 0) {
    changes.psyche.amount += 1
    changes.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${currentUserStats.psyche + 1}/${userMax.psyche}`)
  }

  const healthFull = userMax.health == currentUserStats.health
  if (healthFull && userDown.vitality > 0) {
    changes.vitality.amount += 1
    changes.vitality.reasons.push(`Your health is maxed, so vitality was increased by 1 for total of ${currentUserStats.vitality + 1}/${userMax.vitality}`)
  }
  else if (userDown.vitality > 0) {
    changes.vitality.reasons.push(`Your health is not full, so no bonus to vitality`)
  }

  const psycheFull = userMax.psyche == currentUserStats.psyche + changes.psyche.amount
  if (psycheFull && userDown.rwp > 0) {
    changes.rwp.amount += 1
    changes.rwp.reasons.push(`Your psyche is full, so RWP was increased by 1 for total of ${currentUserStats.rwp + 1}/${userMax.rwp}`)
  }
  else if (userDown.rwp > 0) {
    changes.rwp.reasons.push(`Your psyche is not full, so no bonus to RWP`)
  }

  const bloodFull = userMax.blood == currentUserStats.blood + changes.blood.amount
  const vitalityFull = userMax.vitality == currentUserStats.vitality + changes.vitality.amount
  if (bloodFull && vitalityFull && psycheFull && userDown.mortis > 0) {
    changes.mortis.amount += 1
    changes.mortis.reasons.push(`Your Health, Blood, and Vitality are maxed, mortis was increased by 1 for total of ${currentUserStats.mortis + 1}/${userMax.mortis}`)
  }
  else if (userDown.mortis > 0) {
    changes.mortis.reasons.push(`Your Health, Blood, and Vitality are not full, so no bonus to Mortis`)
  }

  return changes
  // End result should be something like
  // They get:
  //  x Vitality
  //    Your health is maxed, vitality was increased by for total of x (Max Value)
  //  x Health
  //    Your health was increased by 1 for total of x (Max Value)
  //    You did not gain health as your vitality is lower than your health
  //  x blood
  //    Your blood was increased by 1 for total of x (Max Value)
  //    You did not gain blood as your vitality is lower than your blood
  //  x RWP
  //    Your blood was increased by 1 for total of x (Max Value)
  //  x Psyche
  //    Your Psyche was increased by 1 for total of x (Max Value)
  //  x Mortis
  //    Your Health, Blood, and Vitality are maxed, mortis was increased by 1 for total of x (Max Value)
}
