<script setup lang="ts">

import {nextTick, onMounted} from "vue";

onMounted(async () => {
  if(location.hash){
    await nextTick();
    window.location.replace(location.hash);
  }
})
</script>

<template>
  <h1>Choose from a wide range of Expressions!</h1>
  
  <p />

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="sidhe">
        Sidhe
      </h2>
      <p>
        Eldritch Knights. Carefree Illusionists. Archer's of Legend. Spiritual descendants of a long missing race, the
        Sidhe have been portrayed many ways, including capricious fey or guiding angels. Many Sidhe find themselves
        fascinated by the El’Chee, and much of their lore and traditions are modeled on the scant scraps of information
        available on that ancient race. From the eldritch paths of the ancient knights to the glamour using tricksters,
        from the entropy masters to the beneficial healers; the Sidhe are never predictable and always formidable.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div>
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="adepts">
        Adepts
      </h2>
      <p>
        Incredible martial artists. Masters of the mind. Enlightened healers. Stalwart defenders. The specific discipline
        an Adept chooses to follow develops over time shaping their expression and augmenting their abilities. They wield
        mental and physical powers which range from the subtle to the incredible.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div>
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="aeternari">
        Aeternari
      </h2>
      <p>
        The Immortals. The Ageless. In the prehistory of man, the Immortals were twisted to become a madman's generals.
        No longer truly Immortals, the Aeternari now subsist upon the life force of others. Long after their insane
        creator was gone, the Aeternari were accepted into the then-young Society. The oldest of the Aeternari often
        live there lives detached from others, avoiding personal contact, due to countless years spent watching those
        they love grow old and die. Shuttered windows to the past, the Aeternari bear the weight of forgotten yeas and
        constant feelings of Déjà vu. Ancient eyes watch resignedly as time drifts past them, often treasuring their
        relationships with one another and other long-lived supernaturals as the only constants in which they can find
        comfort.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div>
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="sorcerers">
        Sorcerers
      </h2>
      <p>
        Elementalists. Witches. Magi. Sorcerers have more than a title to define them; they hold power over the raw
        materials of creation. The Sorcerers are masters of the elements, able to activate powerful powers that craft
        complex protective effects and evoke raw destruction, eventually summoning and controlling the ultimate examples
        of their arcane power: Elementals. Sorcerers direct the awesome power of the elements with finesse and subtlety.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div>
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="sorcerers">
        Shammas
      </h2>
      <p>
        Werecreatures. Medicine men. Hybrid of humanity and the wild. The Shammas are gifted with a connection to
        nature and the wild that most could never fathom. While some supernaturals command nature and elements, the
        Shammas call forth the powers of nature like an old and trusted friend. Each Shammas has a totem animal;
        Bruda (bear), Felas (great cat), or Walv (wolf). Old Society tales mention a bond made by the early Shammas,
        allowing them to become one of these three totem predators, but modern occultists are skeptical. Regardless,
        whether taking on the aspects of their totems or channeling nature's spirits, their shamanistic roots are
        readily apparent.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div class="">
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>

  <div class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 id="vampyres">
        Vampyres
      </h2>
      <p>
        Vampyres of legend have been portrayed as everything from vicious monsters to cursed saints. The reality falls
        somewhere in between. Mundanes twisted into gods of war tens of thousands of years ago; Vampyres require blood
        as sustenance, as life, as power. The blood’s power gives Vampyres superhuman physical, mental, and mystical
        abilities. In the millennia since they have been free of their creator’s control, Vampyres have come to be
        viable members of the Society, albeit with a thirst for blood. Many Vampyres enjoy their extended life and
        consider themselves above mundane concerns.
      </p>
      <p>For more background and information, please <a href="/login">login.</a></p>
    </div>
    <div class="">
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>
</template>
