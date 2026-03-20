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

  if (userDown.vitality > 0) {
    changes.vitality.amount += 1
    changes.vitality.reasons.push(`You get 1 free vitality, so it was increased by 1 for total of ${currentUserStats.vitality + 1}/${userMax.vitality}`)
  }

  if (userDown.health > 0) {
    if (currentUserStats.health >= currentUserStats.vitality + changes.vitality.amount) {
      changes.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (currentUserStats.health + 1 <= currentUserStats.vitality + changes.vitality.amount) {
      changes.health.amount += 1
      changes.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${currentUserStats.health + 1}/${userMax.health}`)
    }
  }

  if (userDown.blood > 0) {
    if (currentUserStats.blood >= currentUserStats.vitality + changes.vitality.amount) {
      changes.blood.reasons.push('You did not gain blood as your vitality is lower than or equal to your blood')
    }
    else if (currentUserStats.blood + 1 <= currentUserStats.vitality + changes.vitality.amount) {
      changes.blood.amount += 1
      changes.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${currentUserStats.blood + 1}/${userMax.blood}`)
    }
  }

  if (userDown.psyche > 0) {
    changes.psyche.amount += 1
    changes.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${currentUserStats.psyche + 1}/${userMax.psyche}`)
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
}

export function handleShammas(userDown: StripInfo, userMax: StripInfo, xpLevel: number): ChangedStripInfo {
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

  if (userDown.vitality > 0) {
    changes.vitality.amount += 1
    changes.vitality.reasons.push(`You get 1 free vitality, so it was increased by 1 for total of ${currentUserStats.vitality + 1}/${userMax.vitality}`)
  }

  function HandleShammasFreeLevelVitality(levelBonus: number, xpLevel: number) {
    let vitalityChange = levelBonus

    const alreadyMaxedOut = changes.vitality.amount + currentUserStats.vitality == userMax.vitality
    if (alreadyMaxedOut) {
      return // No need to add more
    }

    const bonusGoesOverMax = changes.vitality.amount + vitalityChange + currentUserStats.vitality > userMax.vitality
    if (bonusGoesOverMax) {
      // If so, only apply the amount that does not go over max
      vitalityChange = userMax.vitality - currentUserStats.vitality - changes.vitality.amount
    }
    changes.vitality.amount += vitalityChange
    changes.vitality.reasons.push(`Shammas gain ${levelBonus} additional vitality at level ${xpLevel}, so it was increased by ${vitalityChange} for total of ${currentUserStats.vitality + changes.vitality.amount}/${userMax.vitality}`)
  }

  if (xpLevel >= 7) {
    HandleShammasFreeLevelVitality(3, 7)
  }
  else if (xpLevel >= 5) {
    HandleShammasFreeLevelVitality(2, 5)
  }
  else if (xpLevel >= 3) {
    HandleShammasFreeLevelVitality(1, 3)
  }

  if (userDown.health > 0) {
    if (currentUserStats.health >= currentUserStats.vitality + changes.vitality.amount) {
      changes.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (currentUserStats.health + 1 <= currentUserStats.vitality + changes.vitality.amount) {
      changes.health.amount += 1
      changes.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${currentUserStats.health + 1}/${userMax.health}`)
    }
  }

  if (userDown.blood > 0) {
    if (currentUserStats.blood >= currentUserStats.vitality + changes.vitality.amount) {
      changes.blood.reasons.push('You did not gain blood as your vitality is lower than or equal to your blood')
    }
    else if (currentUserStats.blood + 1 <= currentUserStats.vitality + changes.vitality.amount) {
      changes.blood.amount += 1
      changes.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${currentUserStats.blood + 1}/${userMax.blood}`)
    }
  }

  if (userDown.psyche > 0) {
    changes.psyche.amount += 1
    changes.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${currentUserStats.psyche + 1}/${userMax.psyche}`)
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
}
