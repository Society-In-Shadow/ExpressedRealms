import { defineStore } from 'pinia'
import type {PlayerListItem} from "@/components/players/Objects/Player";
import axios from "axios";

export const playerList =
    defineStore('playerList', {
        state: () => {
            return {
                players: [] as Array<PlayerListItem>,
                filteredPlayers: [] as Array<PlayerListItem>
            }
        },
        actions: {
            async fetchPlayers() {
                await axios.get('/admin/users')
                    .then((response) => {
                        response.data.users.forEach(function(item:PlayerListItem) {
                            item.lockedOutExpires = new Date(item.lockedOutExpires);
                        });
                        this.players = response.data.users;
                        this.filteredPlayers = response.data.users
                    });
            },
            filterPlayers(query: string) {
                const lowercasedQuery = query.toLowerCase().trim();
            
                if (!lowercasedQuery) {
                    // Reset showing all players if the query is empty
                    this.filteredPlayers = this.players;
                } else {
                    // Filter players by username or email
                    this.filteredPlayers = this.players.filter((player) =>
                        player.username.toLowerCase().includes(lowercasedQuery) ||
                        player.email.toLowerCase().includes(lowercasedQuery)
                    );
                }
            },
            getPrivilegedPlayers(){
                return this.filteredPlayers.filter(x => x.roles.length > 0 && x.isDisabled === false)
            },
            getUnverifiedPlayers(){
                return this.filteredPlayers.filter(x => x.emailConfirmed === false && x.isDisabled === false)
            },
            getPlayers(){
                return this.filteredPlayers.filter(x => x.roles.length === 0 && x.emailConfirmed === true && x.isDisabled === false)
            },
            getDisabledPlayers(){
                return this.filteredPlayers.filter(x => x.isDisabled === true)
            }
        }
    });
