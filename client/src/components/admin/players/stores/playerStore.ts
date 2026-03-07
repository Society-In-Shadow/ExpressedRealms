import { defineStore } from 'pinia'
import type { AssignedRoleInfo, PlayerListItem } from '@/components/admin/players/types'
import axios from 'axios'
import { DateTime } from 'luxon'

export const PlayerStore
  = defineStore('playerStore', {
    state: () => {
      return {
        players: [] as Array<PlayerListItem>,
        filteredPlayers: [] as Array<PlayerListItem>,
        player: {} as PlayerListItem,
      }
    },
    actions: {
      async fetchPlayers() {
        await axios.get('/admin/users')
          .then((response) => {
            response.data.users.forEach(function (item: PlayerListItem) {
              item.lockedOutExpires = new Date(item.lockedOutExpires)
              item.roles.forEach(function (item: AssignedRoleInfo) {
                item.expirationDate = item.expirationDate ? DateTime.fromISO(`${item.expirationDate}`) : null
              })
            })
            this.players = response.data.users
            this.filteredPlayers = response.data.users
          })
      },
      async getPlayer(id: string) {
        if (this.players.length === 0)
          await this.fetchPlayers()

        this.player = this.players.find(x => x.id === id)!
      },
      filterPlayers(query: string) {
        const lowercasedQuery = query.toLowerCase().trim()

        if (lowercasedQuery) {
          // Filter players by username or email
          this.filteredPlayers = this.players.filter(player =>
            player.username.toLowerCase().includes(lowercasedQuery)
            || player.email.toLowerCase().includes(lowercasedQuery),
          )
          return
        }
        // Reset showing all players if the query is empty
        this.filteredPlayers = this.players
      },
      getPrivilegedPlayers() {
        return this.filteredPlayers.filter(x => (x.roles.length > 0 || x.legacyRoles.length > 0) && x.isDisabled === false)
      },
      getUnverifiedPlayers() {
        return this.filteredPlayers.filter(x => x.emailConfirmed === false && x.isDisabled === false)
      },
      getPlayers() {
        return this.filteredPlayers.filter(x => x.roles.length === 0 && x.emailConfirmed === true && x.isDisabled === false)
      },
      getDisabledPlayers() {
        return this.filteredPlayers.filter(x => x.isDisabled === true)
      },
    },
  })
