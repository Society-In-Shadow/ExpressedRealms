<script setup lang="ts">

import {ref} from "vue";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import SplitButton from 'primevue/splitbutton';
import Card from "primevue/card";

const stones = ref([]);
const neutralStone = ref("");
const winningMarble = ref("");
const winningMarbleValue = ref(0);

const stoneTypes = [ "red", "blue", "black", "clear", "green", "white"]
let stoneBag = [ "red", "blue", "black", "clear", "green", "white"]

function removeStone(stoneName:string): string{
  var characterIndex = stoneBag.indexOf(stoneName)
  ~characterIndex && stoneBag.splice(characterIndex, 1);
  return stoneName;
}
function pullStones(numberOfStones:number) {
  
  for(var i = 1; i<=numberOfStones; i++){

    if(stoneBag.length === 0)
      return;
    
    if(stoneBag.length === 1){
      stones.value.push(removeStone(stoneBag[0]));
      return;
    }
    
    const stone = removeStone(stoneBag[getRandomInt(0, stoneBag.length-1)])
    stones.value.push(stone);
  }
  calculateBonus();
}

function pullNeutralStone() {
  neutralStone.value = stoneTypes[getRandomInt(0, 5)];
  calculateBonus();
}

function clearStones() {
  stones.value = [];
  stoneBag = stoneTypes.slice();
  neutralStone.value = "";
}
function getRandomInt(min, max) {
  const minCeiled = Math.ceil(min);
  const maxFloored = Math.floor(max);
  return Math.floor(Math.random() * (maxFloored - minCeiled + 1) + minCeiled); // The maximum is exclusive and the minimum is inclusive
}

function calculateBonus():number {
  
  if(neutralStone.value == "")
    return -1;
  
  if(stones.value.length == 0)
    return -1;
  
  const neutralStoneIndex = table.map(x => x.stone.toLowerCase()).indexOf(neutralStone.value);
  
  let stoneBonus = [];
  
  stones.value.forEach((stoneName) => {
    stoneBonus.push(table[neutralStoneIndex][stoneName]);
  });
  
  var maxValue = Math.max(...stoneBonus);
  
  const stonePosition = stoneBonus.indexOf(maxValue);

  winningMarble.value = stones.value[stonePosition];
  winningMarbleValue.value = maxValue;
  
  return maxValue;
}

function showMarbleValue(marbleName: string): string {
  
  if(marbleName === winningMarble.value)
    return "+" + winningMarbleValue.value;
  
  return ""
}


var table = [
  {
    stone: "Black",
    black: 5,
    blue: 4,
    clear: 3,
    green: 2,
    red: 1,
    white: 0
  },
  {
    stone: "Blue",
    black: 4,
    blue: 5,
    clear: 1,
    green: 3,
    red: 0,
    white: 2
  },
  {
    stone: "Clear",
    black: 3,
    blue: 1,
    clear: 5,
    green: 0,
    red: 2,
    white: 4
  },
  {
    stone: "Green",
    black: 2,
    blue: 3,
    clear: 0,
    green: 5,
    red: 4,
    white: 1
  },
  {
    stone: "Red",
    black: 1,
    blue: 0,
    clear: 2,
    green: 4,
    red: 5,
    white: 3
  },
  {
    stone: "White",
    black: 0,
    blue: 2,
    clear: 4,
    green: 1,
    red: 3,
    white: 5
  }
]

const neutralStones = [
  {
    label: 'Black',
    command: () => neutralStone.value = "black"
  },
  {
    label: 'Blue',
    command: () => neutralStone.value = "blue"
  },
  {
    label: 'Clear',
    command: () => neutralStone.value = "clear"
  },
  {
    label: 'Green',
    command: () => neutralStone.value = "green"
  },
  {
    label: 'Red',
    command: () => neutralStone.value = "red"
  },  
  {
    label: 'White',
    command: () => neutralStone.value = "white"
  },
];

const pullStoneList = [
  {
    label: '2 Stones',
    command: () => pullStones(2)
  },
  {
    label: '3 Stones',
    command: () => pullStones(3)
  },
  {
    label: '4 Stones',
    command: () => pullStones(4)
  },
  {
    label: '5 Stones',
    command: () => pullStones(5)
  },
  {
    label: '6 Stones',
    command: () => pullStones(6)
  }
];

const bonusEffects = [
  { bonus: "+5", success: "Choose and combine two other effects", failure: "Choose and combine two other effects" },
  { bonus: "+4", success: "+1 Damage and Action delayed 6 phases", failure: "2 Damage or Daze" },
  { bonus: "+3", success: "+1 Damage or Daze", failure: "Action delayed 6 phases" },
  { bonus: "+2", success: "Move defender 2 paces in any direction", failure: "1 Damage or Stun" },
  { bonus: "+1", success: "Action delayed 6 phases or Stun", failure: "Held item lands 2 paces away in direction of target" },
  { bonus: "+0", success: "Knockdown or Disarm", failure: "Fall Down, Drop held item, moved 2 paces in direction of defender’s choice" }
];


</script>

<template>
  <Card class="m-3">
    <template #content>
      <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
        <SplitButton label="Pull Stone" @click="pullStones(1)" class="m-2" :model="pullStoneList" />
        <SplitButton label="Pull Neutral Stone" @click="pullNeutralStone" class="m-2"  :model="neutralStones" />
        <Button data-cy="logoff-button" label="Clear Stones" class="m-2" @click="clearStones" />
      </div>
      <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
        <div class="stone ml-3 mt-3 mb-3 mr-5 text-center align-content-center">
          <div>{{ neutralStone }}</div>
        </div>
      </div>
      <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
        <div v-for="stone in stones" class="stone m-3 text-center align-content-center">
          <div>{{stone}}</div>
          <div>{{ showMarbleValue(stone) }}</div>
        </div>
      </div>
    
      <h1>Stone Pulling</h1>
      <p>
        In a RBT, a drawn stone’s color is cross-referenced to the neutral stone’s color via the Random Bonus Table, 
        illustrated below. Comparing two colors generates a number between 0 and 5 called the 
        Random Bonus. For example, if a player draws a red stone and the neutral stone is green, a RB of +4 is 
        generated. If a player is entitled to draw multiple stones for the test, they choose the stone giving them the 
        highest bonus value.
        
        The bag has 1 stone of each color.
      </p>
      
      <DataTable :value="table" table-style="min-width: 50rem">
        <Column field="stone" header="" />
        <Column field="black" header="Black">
          <template #body="slotProps">
            +{{ slotProps.data.black }}
          </template>
        </Column>
        <Column field="blue" header="Blue" >
          <template #body="slotProps">
            +{{ slotProps.data.blue }}
          </template>
        </Column>
        <Column field="clear" header="Clear" >
          <template #body="slotProps">
            +{{ slotProps.data.clear }}
          </template>
        </Column>
        <Column field="green" header="Green" >
          <template #body="slotProps">
            +{{ slotProps.data.green }}
          </template>
        </Column>
        <Column field="red" header="Red" >
          <template #body="slotProps">
            +{{ slotProps.data.red }}
          </template>
        </Column>
        <Column field="white" header="White" >
          <template #body="slotProps">
            +{{ slotProps.data.white }}
          </template>
        </Column>
      </DataTable>
      
    </template>
  </Card>
  
  <h1>Lead Stone</h1>
  <p>
    In some instances, players are allowed to draw more than one stone. When drawing multiple stones, it is important
    for you to specify a lead stone. To do so, you should somehow separate and designate your lead stone prior to
    revealing. A common method for doing this is to shift one of your drawn stones into the other hand and place this
    hand above the other to clearly designate it as the lead stone. Some tests rely on the lead stone to determine the
    quality of the test’s result, however, the other stone(s) are typically still used for determining success.
  </p>
  <h1>Success and Failure</h1>
  <p>
    All things considered, it is assumed that if you fail, you fail completely and if you succeed, that you succeed
    completely. On some rare occasions, a successful test may not have the intended effect, and a failure may have some
    benefits to the failing character.
  </p>
  <h1>Critical Success and Critical Failure</h1>
  <p>
    There are two rare (1-in-36 chance) circumstances that can occur when an attack is made, changing the result
    drastically.
  </p>
  <p>
    The first is a critical success and happens when the attacker’s lead stone random bonus result is +5 and the
    defender’s is +0. The other is a critical failure and happens in the reverse circumstance when the defender’s lead
    stone random bonus result is +5 and the attacker’s is +0.
  </p>
  <p>In either case, the successful party may either allow the GO a free hand to describe the critical success/failure
    with cinematic and mechanics or draw a new random bonus test and look up the result on the appropriate critical
    table. This is called making a critical test. Some powers have a unique effect listed in the event of a critical
    result, in these instances, a critical test is not necessary. When a critical success occurs there is no need to
    draw for an extra damage test as this test is considered to be successful. The game official always has the final
    say on which critical effect occurs, including making up critical effects on the fly, allowing the game official
    to craft the cinematic of the scene.
  </p>

  <DataTable :value="bonusEffects" table-style="min-width: 50rem">
    <Column field="bonus" header="Bonus" />
    <Column field="success" header="Success (Effect on Defender)" />
    <Column field="failure" header="Failure (Effect on Attacker)" />
  </DataTable>
</template>

<style scoped>
.stone {
  width: 75px;
  height: 75px;
  border: white 5px solid;
}

.leadStone{
  border: greenyellow 5px solid;
}
</style>