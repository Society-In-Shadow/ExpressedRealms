export interface StripInfo {
  vitality: number
  health: number
  blood: number
  rwp: number
  psyche: number
  mortis: number
}

export interface TrackedStripInfo {
  vitality: TrackingInfo
  health: TrackingInfo
  blood: TrackingInfo
  rwp: TrackingInfo
  psyche: TrackingInfo
  mortis: TrackingInfo
}

export class TrackingInfo {
  constructor(
    public currentAmount: number,
    public gainedAmount: number,
    public maxAmount: number,
    public reasons: string[],
  ) {}

  get needsMore(): boolean {
    return this.currentAmount + this.gainedAmount < this.maxAmount
  }

  get isFull(): boolean {
    return this.currentAmount + this.gainedAmount == this.maxAmount
  }

  get amountWithTotalGained(): number {
    return this.currentAmount + this.gainedAmount
  }
}
// Adept, Sidhe, Sorcerers
export function handleAdeptSidheSorcerers(userDown: StripInfo, userMax: StripInfo): TrackedStripInfo {
  const trackedStripInfo = {
    vitality: new TrackingInfo(userMax.vitality - userDown.vitality, 0, userMax.vitality, []),
    health: new TrackingInfo(userMax.health - userDown.health, 0, userMax.health, []),
    blood: new TrackingInfo(userMax.blood - userDown.blood, 0, userMax.blood, []),
    rwp: new TrackingInfo(userMax.rwp - userDown.rwp, 0, userMax.rwp, []),
    psyche: new TrackingInfo(userMax.psyche - userDown.psyche, 0, userMax.psyche, []),
    mortis: new TrackingInfo(userMax.mortis - userDown.mortis, 0, userMax.mortis, []),
  }

  if (trackedStripInfo.vitality.needsMore) {
    trackedStripInfo.vitality.gainedAmount += 1
    trackedStripInfo.vitality.reasons.push(`You get 1 free vitality, so it was increased by 1 for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
  }

  if (trackedStripInfo.health.needsMore) {
    if (trackedStripInfo.health.amountWithTotalGained >= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (trackedStripInfo.health.amountWithTotalGained + 1 <= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.gainedAmount += 1
      trackedStripInfo.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${trackedStripInfo.health.amountWithTotalGained}/${trackedStripInfo.health.maxAmount}`)
    }
  }

  if (trackedStripInfo.blood.needsMore) {
    if (trackedStripInfo.blood.amountWithTotalGained >= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.blood.reasons.push('You did not gain blood as your vitality is lower than or equal to your blood')
    }
    else if (trackedStripInfo.blood.amountWithTotalGained + 1 <= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.blood.gainedAmount += 1
      trackedStripInfo.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
    }
  }

  if (trackedStripInfo.psyche.needsMore) {
    trackedStripInfo.psyche.gainedAmount += 1
    trackedStripInfo.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${trackedStripInfo.psyche.amountWithTotalGained}/${trackedStripInfo.psyche.maxAmount}`)
  }

  if (trackedStripInfo.psyche.isFull && trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.gainedAmount += 1
    trackedStripInfo.rwp.reasons.push(`Your psyche is full, so RWP was increased by 1 for total of ${trackedStripInfo.rwp.amountWithTotalGained}/${trackedStripInfo.rwp.maxAmount}`)
  }
  else if (trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.reasons.push(`Your psyche is not full, so no bonus to RWP`)
  }

  if (trackedStripInfo.blood.isFull && trackedStripInfo.vitality.isFull && trackedStripInfo.health.isFull && trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.gainedAmount += 1
    trackedStripInfo.mortis.reasons.push(`Your Health, Blood, and Vitality are maxed, mortis was increased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)
  }
  else if (trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.reasons.push(`Your Health, Blood, and Vitality are not full, so no bonus to Mortis`)
  }

  return trackedStripInfo
}

export function handleShammas(userDown: StripInfo, userMax: StripInfo, xpLevel: number): TrackedStripInfo {
  const trackedStripInfo = {
    vitality: new TrackingInfo(userMax.vitality - userDown.vitality, 0, userMax.vitality, []),
    health: new TrackingInfo(userMax.health - userDown.health, 0, userMax.health, []),
    blood: new TrackingInfo(userMax.blood - userDown.blood, 0, userMax.blood, []),
    rwp: new TrackingInfo(userMax.rwp - userDown.rwp, 0, userMax.rwp, []),
    psyche: new TrackingInfo(userMax.psyche - userDown.psyche, 0, userMax.psyche, []),
    mortis: new TrackingInfo(userMax.mortis - userDown.mortis, 0, userMax.mortis, []),
  }

  function handleShammasBonus(targetStat: keyof typeof trackedStripInfo, levelBonus: number, xpLevel: number) {
    let vitalityChange = levelBonus

    const alreadyMaxedOut = trackedStripInfo[targetStat].isFull
    if (alreadyMaxedOut) {
      return // No need to add more
    }

    const bonusGoesOverMax = trackedStripInfo[targetStat].amountWithTotalGained + vitalityChange > trackedStripInfo[targetStat].maxAmount
    if (bonusGoesOverMax) {
      // If so, only apply the amount that does not go over max
      vitalityChange = trackedStripInfo[targetStat].maxAmount - trackedStripInfo[targetStat].amountWithTotalGained
    }
    trackedStripInfo[targetStat].gainedAmount += vitalityChange
    trackedStripInfo[targetStat].reasons.push(`Shammas gain ${levelBonus} additional ${targetStat} at level ${xpLevel}, so it was increased by ${vitalityChange} for total of ${trackedStripInfo[targetStat].amountWithTotalGained}/${trackedStripInfo[targetStat].maxAmount}`)
  }

  if (xpLevel >= 7) {
    handleShammasBonus('vitality', 3, 7)
    handleShammasBonus('health', 3, 7)
    handleShammasBonus('blood', 3, 7)
    handleShammasBonus('psyche', 3, 7)
  }
  else if (xpLevel >= 5) {
    handleShammasBonus('vitality', 2, 5)
    handleShammasBonus('health', 2, 5)
    handleShammasBonus('blood', 2, 5)
    handleShammasBonus('psyche', 2, 5)
  }
  else if (xpLevel >= 3) {
    handleShammasBonus('vitality', 1, 3)
    handleShammasBonus('health', 1, 3)
    handleShammasBonus('blood', 1, 3)
    handleShammasBonus('psyche', 1, 3)
  }

  if (trackedStripInfo.vitality.needsMore) {
    trackedStripInfo.vitality.gainedAmount += 1
    trackedStripInfo.vitality.reasons.push(`You get 1 free vitality, so it was increased by 1 for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
  }

  if (trackedStripInfo.health.needsMore) {
    if (trackedStripInfo.health.amountWithTotalGained >= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (trackedStripInfo.health.amountWithTotalGained + 1 <= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.gainedAmount += 1
      trackedStripInfo.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${trackedStripInfo.health.amountWithTotalGained}/${trackedStripInfo.health.maxAmount}`)
    }
  }

  if (trackedStripInfo.blood.needsMore) {
    if (trackedStripInfo.blood.amountWithTotalGained >= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.blood.reasons.push('You did not gain blood as your vitality is lower than or equal to your blood')
    }
    else if (trackedStripInfo.blood.amountWithTotalGained + 1 <= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.blood.gainedAmount += 1
      trackedStripInfo.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
    }
  }

  if (trackedStripInfo.psyche.needsMore) {
    trackedStripInfo.psyche.gainedAmount += 1
    trackedStripInfo.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${trackedStripInfo.psyche.amountWithTotalGained}/${trackedStripInfo.psyche.maxAmount}`)
  }

  if (trackedStripInfo.psyche.isFull && trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.gainedAmount += 1
    trackedStripInfo.rwp.reasons.push(`Your psyche is full, so RWP was increased by 1 for total of ${trackedStripInfo.rwp.amountWithTotalGained}/${trackedStripInfo.rwp.maxAmount}`)
  }
  else if (trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.reasons.push(`Your psyche is not full, so no bonus to RWP`)
  }

  if (trackedStripInfo.blood.isFull && trackedStripInfo.vitality.isFull && trackedStripInfo.vitality.isFull && trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.gainedAmount += 1
    trackedStripInfo.mortis.reasons.push(`Your Health, Blood, and Vitality are maxed, mortis was increased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)
  }
  else if (trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.reasons.push(`Your Health, Blood, and Vitality are not full, so no bonus to Mortis`)
  }

  return trackedStripInfo
}

export function handleAeternari(userDown: StripInfo, userMax: StripInfo, xpLevel: number): TrackedStripInfo {
  const trackedStripInfo = {
    vitality: new TrackingInfo(userMax.vitality - userDown.vitality, 0, userMax.vitality, []),
    health: new TrackingInfo(userMax.health - userDown.health, 0, userMax.health, []),
    blood: new TrackingInfo(userMax.blood - userDown.blood, 0, userMax.blood, []),
    rwp: new TrackingInfo(userMax.rwp - userDown.rwp, 0, userMax.rwp, []),
    psyche: new TrackingInfo(userMax.psyche - userDown.psyche, 0, userMax.psyche, []),
    mortis: new TrackingInfo(userMax.mortis - userDown.mortis, 0, userMax.mortis, []),
  }

  if (trackedStripInfo.vitality.amountWithTotalGained > xpLevel) {
    trackedStripInfo.vitality.gainedAmount -= xpLevel
    trackedStripInfo.vitality.reasons.push(`You lost ${xpLevel} vitality due to Vitality Burn, so it was decreased by ${xpLevel} for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
  }
  else {
    // Not enough vitality to burn, so burn additional resources
    while (trackedStripInfo.vitality.amountWithTotalGained <= xpLevel) {
      trackedStripInfo.rwp.gainedAmount -= 1
      trackedStripInfo.rwp.reasons.push(`You burned 1 RWP and 1 mortis to fulfill Vitality Burn, so RWP was decreased by 1 for total of ${trackedStripInfo.rwp.amountWithTotalGained}/${trackedStripInfo.rwp.maxAmount}`)
      trackedStripInfo.mortis.gainedAmount -= 1
      trackedStripInfo.mortis.reasons.push(`You burned 1 RWP and 1 mortis to fulfill Vitality Burn, so mortis was decreased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)

      if (trackedStripInfo.rwp.amountWithTotalGained <= 0) {
        trackedStripInfo.rwp.reasons.push('You burned all RWP to fulfill Vitality Burn, you have entered "Death Sleep".  GO Required')
      }

      if (trackedStripInfo.mortis.amountWithTotalGained <= 0) {
        trackedStripInfo.mortis.reasons.push('You burned all mortis to fulfill Vitality Burn, you have entered "Death Sleep".  GO Required')
      }

      if (trackedStripInfo.rwp.amountWithTotalGained <= 0 || trackedStripInfo.mortis.amountWithTotalGained <= 0) {
        return trackedStripInfo
      }

      let vitalityChange = 6
      const bonusGoesOverMax = trackedStripInfo.vitality.amountWithTotalGained + vitalityChange > trackedStripInfo.vitality.maxAmount
      if (bonusGoesOverMax) {
        // If so, only apply the amount that does not go over max
        vitalityChange = trackedStripInfo.vitality.maxAmount - trackedStripInfo.vitality.amountWithTotalGained
      }
      trackedStripInfo.vitality.gainedAmount += vitalityChange
      trackedStripInfo.vitality.reasons.push(`You burned 1 RWP and 1 mortis to fulfill Vitality Burn, so vitality was increased by ${vitalityChange} for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
    }
    trackedStripInfo.vitality.gainedAmount -= xpLevel
    trackedStripInfo.vitality.reasons.push(`You lost ${xpLevel} vitality due to Vitality Burn, so it was decreased by ${xpLevel} for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
  }

  if (trackedStripInfo.health.needsMore) {
    trackedStripInfo.health.gainedAmount += 1
    trackedStripInfo.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${trackedStripInfo.health.amountWithTotalGained}/${trackedStripInfo.health.maxAmount}`)
  }

  if (trackedStripInfo.blood.needsMore) {
    trackedStripInfo.blood.gainedAmount += 1
    trackedStripInfo.blood.reasons.push(`You get 1 free blood, so it was increased by 1 for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
  }

  if (trackedStripInfo.psyche.needsMore) {
    trackedStripInfo.psyche.gainedAmount += 1
    trackedStripInfo.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${trackedStripInfo.psyche.amountWithTotalGained}/${trackedStripInfo.psyche.maxAmount}`)
  }

  if (trackedStripInfo.psyche.isFull && trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.gainedAmount += 1
    trackedStripInfo.rwp.reasons.push(`Your psyche is full, so RWP was increased by 1 for total of ${trackedStripInfo.rwp.amountWithTotalGained}/${trackedStripInfo.rwp.maxAmount}`)
  }
  else if (trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.reasons.push(`Your psyche is not full, so no bonus to RWP`)
  }

  if (trackedStripInfo.health.isFull && trackedStripInfo.blood.isFull && trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.gainedAmount += 1
    trackedStripInfo.mortis.reasons.push(`Your Blood and Health are maxed, mortis was increased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)
  }
  else if (trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.reasons.push(`Your Blood and Health are not full, so no bonus to Mortis`)
  }

  return trackedStripInfo
}

export function handleVampyre(userDown: StripInfo, userMax: StripInfo, xpLevel: number): TrackedStripInfo {
  const trackedStripInfo = {
    vitality: new TrackingInfo(userMax.vitality - userDown.vitality, 0, userMax.vitality, []),
    health: new TrackingInfo(userMax.health - userDown.health, 0, userMax.health, []),
    blood: new TrackingInfo(userMax.blood - userDown.blood, 0, userMax.blood, []),
    rwp: new TrackingInfo(userMax.rwp - userDown.rwp, 0, userMax.rwp, []),
    psyche: new TrackingInfo(userMax.psyche - userDown.psyche, 0, userMax.psyche, []),
    mortis: new TrackingInfo(userMax.mortis - userDown.mortis, 0, userMax.mortis, []),
  }

  if (trackedStripInfo.blood.amountWithTotalGained > xpLevel) {
    trackedStripInfo.blood.gainedAmount -= xpLevel
    trackedStripInfo.blood.reasons.push(`You lost ${xpLevel} blood due to Blood Thirst, so it was decreased by ${xpLevel} for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
  }
  else {
    // Not enough blood to burn, so burn additional resources
    while (trackedStripInfo.blood.amountWithTotalGained <= xpLevel) {
      trackedStripInfo.health.gainedAmount -= 1
      trackedStripInfo.health.reasons.push(`You burned 1 health, 1 vitality, and 1 mortis to fulfill Blood Thirst, so health was decreased by 1 for total of ${trackedStripInfo.health.amountWithTotalGained}/${trackedStripInfo.health.maxAmount}`)
      trackedStripInfo.vitality.gainedAmount -= 1
      trackedStripInfo.vitality.reasons.push(`You burned 1 health, 1 vitality, and 1 mortis to fulfill Blood Thirst, so vitality was decreased by 1 for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
      trackedStripInfo.mortis.gainedAmount -= 1
      trackedStripInfo.mortis.reasons.push(`You burned 1 health, 1 vitality, and 1 mortis to fulfill Blood Thirst, so mortis was decreased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)

      if (trackedStripInfo.health.amountWithTotalGained <= 0) {
        trackedStripInfo.health.reasons.push('You burned all health to fulfill Blood Thirst, you have entered "Sleep of Ages".  GO Required')
      }

      if (trackedStripInfo.vitality.amountWithTotalGained <= 0) {
        trackedStripInfo.vitality.reasons.push('You burned all vitality to fulfill Blood Thirst, you have entered "Sleep of Ages".  GO Required')
      }

      if (trackedStripInfo.mortis.amountWithTotalGained <= 0) {
        trackedStripInfo.mortis.reasons.push('You burned all mortis to fulfill Blood Thirst, you have entered "Sleep of Ages".  GO Required')
      }

      if (trackedStripInfo.rwp.amountWithTotalGained <= 0 || trackedStripInfo.mortis.amountWithTotalGained <= 0 || trackedStripInfo.health.amountWithTotalGained <= 0) {
        return trackedStripInfo
      }

      let bloodChange = 6
      const bonusGoesOverMax = trackedStripInfo.blood.amountWithTotalGained + bloodChange > trackedStripInfo.blood.maxAmount
      if (bonusGoesOverMax) {
        // If so, only apply the amount that does not go over max
        bloodChange = trackedStripInfo.blood.maxAmount - trackedStripInfo.blood.amountWithTotalGained
      }
      trackedStripInfo.blood.gainedAmount += bloodChange
      trackedStripInfo.blood.reasons.push(`You burned 1 RWP and 1 mortis to fulfill Blood Thirst, so blood was increased by ${bloodChange} for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
    }
    trackedStripInfo.blood.gainedAmount -= xpLevel
    trackedStripInfo.blood.reasons.push(`You lost ${xpLevel} blood due to Blood Thirst, so it was decreased by ${xpLevel} for total of ${trackedStripInfo.blood.amountWithTotalGained}/${trackedStripInfo.blood.maxAmount}`)
  }

  if (trackedStripInfo.vitality.needsMore) {
    trackedStripInfo.vitality.gainedAmount += 1
    trackedStripInfo.vitality.reasons.push(`You get 1 free vitality, so it was increased by 1 for total of ${trackedStripInfo.vitality.amountWithTotalGained}/${trackedStripInfo.vitality.maxAmount}`)
  }

  if (trackedStripInfo.health.needsMore) {
    if (trackedStripInfo.health.amountWithTotalGained >= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.reasons.push('You did not gain health as your vitality is lower than or equal to your health')
    }
    else if (trackedStripInfo.health.amountWithTotalGained + 1 <= trackedStripInfo.vitality.amountWithTotalGained) {
      trackedStripInfo.health.gainedAmount += 1
      trackedStripInfo.health.reasons.push(`You get 1 free health, so it was increased by 1 for total of ${trackedStripInfo.health.amountWithTotalGained}/${trackedStripInfo.health.maxAmount}`)
    }
  }

  if (trackedStripInfo.psyche.needsMore) {
    trackedStripInfo.psyche.gainedAmount += 1
    trackedStripInfo.psyche.reasons.push(`You get a free Psyche, so it was increased by 1 for total of ${trackedStripInfo.psyche.amountWithTotalGained}/${trackedStripInfo.psyche.maxAmount}`)
  }

  if (trackedStripInfo.psyche.isFull && trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.gainedAmount += 1
    trackedStripInfo.rwp.reasons.push(`Your psyche is full, so RWP was increased by 1 for total of ${trackedStripInfo.rwp.amountWithTotalGained}/${trackedStripInfo.rwp.maxAmount}`)
  }
  else if (trackedStripInfo.rwp.needsMore) {
    trackedStripInfo.rwp.reasons.push(`Your psyche is not full, so no bonus to RWP`)
  }

  if (trackedStripInfo.health.isFull && trackedStripInfo.vitality.isFull && trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.gainedAmount += 1
    trackedStripInfo.mortis.reasons.push(`Your Health and Vitality are maxed, mortis was increased by 1 for total of ${trackedStripInfo.mortis.amountWithTotalGained}/${trackedStripInfo.mortis.maxAmount}`)
  }
  else if (trackedStripInfo.mortis.needsMore) {
    trackedStripInfo.mortis.reasons.push(`Your Health and Vitality are not full, so no bonus to Mortis`)
  }

  return trackedStripInfo
}
