import {defineStore} from "pinia";
import type {ExpressionInfo} from "@/components/public/types";

export const publicExpressionsStore =
    defineStore(`publicExpressions`, {
        state: () => {
            return {
                expressions: [] as ExpressionInfo[]
            }
        },
        actions: {
            async getExpressions(){

                this.expressions = [
                    {
                        id: 1,
                        name: 'Adepts',
                        archetypes: 'Martial artists, medics, mentalists, negotiators, philosophers',
                        description: 'Have you ever wanted to be a master Martial artist, or an enlightened master of the Mind? Ranging from a physical master of their body to the ability to help others with but a touch, the Adept can do a lot, allowing them to fly across the field, dodge bullets, and crush others with their minds. Think 80s Action movie or the wizened old head of a monastery when you imagine what abilities an Adept can utilize.'
                    },
                    {
                        id: 2,
                        name: 'Aeternari',
                        archetypes: ' Bodyguards, curse masters, hex masters, knights reborn, mummies, strangers out of time',
                        description: 'Ancient beings with an immortal soul, Aeternari have refined their abilities to stand the test of time. Ranging from Immortal warriors who have fought in major wars, to the Generals of old able to voice commands from afar, these people can also curse their foes to weaken their very essence. An Aeternari also feeds off the very life essence of others, needing to consume a little every day, though the Society does provide a safe method to do so. Think of these like the eternal warrior fighting to remain on top, or those with commanding voices that speak to the very soul of others.'
                    },
                    {
                        id: 3,
                        name: 'Shammas',
                        archetypes: 'Beast-men, brawlers, healers, mountain men, shamans, mystics, shape shifters, trackers, tribal  warriors ',
                        description: 'Inspired by the Shamans of old, the Shammas conjure the powers of nature and summon the beast within themselves. Able to use the plants and animals around, they can heal others and control the field while in human form, but all have a connection to an animal, and some choose to indulge in the beast within, using it to be faster, tougher, and stronger. Their general feeling is old shamans and werepeople.'
                    },
                    {
                        id: 4,
                        name: 'Sidhe',
                        archetypes: 'Archers, assassins, Eldritch knights, Fey, Illusionists, noble born, seneschals, tricksters',
                        description: 'The very essence of the Fae realms, Sidhe embody both the Tricksters and Warriors of the fae realms. Given the ability to divert attention and fool the senses, the Sidhe make the choice at higher levels to devote towards a more physical manifestation or the more trickery based ones. With the ability to create armor and weaponry from thin air, alongside the capability to turn the eye away from others, Sidhe can find their fit in many groups. Puck from Shakespeare or the Knights of Yore by Arthurian legends are perfect embodiments of their powers.'
                    },
                    {
                        id: 5,
                        name: 'Sorcerer',
                        archetypes: 'Arcane Masters, Conjurers, Elementalists, Seers, Summoners, Wielders of Raw Magick',
                        description: 'Mages of high power, Sorcerers are devoted to an element or two, and develop their skills into those powers to manipulate their chosen elements. Between Air, Earth, Fire and Water; a Sorcerer is spoiled for choice, though they may never take opposing elements. With control, damage, and the unique ability to teleport, Sorcerers have a decent utility to back up their powerhouse of an ability pool. Sorcerers can be seen in the form of certain wizard in Chicago who primarily used fire for their spells.'
                    },
                    {
                        id: 6,
                        name: 'Vampyre',
                        archetypes: 'Blood Mages, Brutes, Chameleons, Mentalists, Puppet Masters',
                        description: 'Giving a penchant for blood and a wide range of powers from twisting forms to be more physically fortified, to the ability to dominate others with their minds, these Vampyres can turn to stone or into the typical bats and dogs. Having to deal with a Bloodthirst that creeps up and needing to feed regularly creates an interesting dynamic between them and others. A typical vampire can be easily seen in most vampiric media, though they do not (inherently) sparkle.'
                    }
                ]
            },
        }
    });
