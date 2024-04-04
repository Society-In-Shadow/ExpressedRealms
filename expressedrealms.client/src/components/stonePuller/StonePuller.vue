<script setup lang="ts">

import {ref} from "vue";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import SplitButton from 'primevue/splitbutton';

const stones = ref([]);
const neutralStone = ref("");

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
    
}

function pullNeutralStone() {
  neutralStone.value = stoneTypes[getRandomInt(0, 5)];
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

var table = [
  {
    stone: "Black",
    black: "+5",
    blue: "+4",
    clear: "+3",
    green: "+2",
    red: "+1",
    white: "+0"
  },
  {
    stone: "Blue",
    black: "+4",
    blue: "+5",
    clear: "+1",
    green: "+3",
    red: "+0",
    white: "+2"
  },
  {
    stone: "Clear",
    black: "+3",
    blue: "+1",
    clear: "+5",
    green: "+0",
    red: "+2",
    white: "+4"
  },
  {
    stone: "Green",
    black: "+2",
    blue: "+3",
    clear: "+0",
    green: "+5",
    red: "+4",
    white: "+1"
  },
  {
    stone: "Red",
    black: "+1",
    blue: "+0",
    clear: "+2",
    green: "+4",
    red: "+5",
    white: "+3"
  },
  {
    stone: "White",
    black: "+0",
    blue: "+2",
    clear: "+4",
    green: "+1",
    red: "+3",
    white: "+5"
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

</script>

<template>
  <SplitButton label="Pull Neutral Stone" @click="pullNeutralStone" class="m-2" :model="neutralStones" />
  <SplitButton label="Pull Stone" @click="pullStones(1)" class="m-2" :model="pullStoneList" />
  <Button data-cy="logoff-button" label="Clear Stones" class="m-2" @click="clearStones" />
    
  <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
    <div class="stone leadStone ml-3 mt-3 mb-3 mr-5 text-center align-content-center">{{ neutralStone }}</div>
    <div class="stone leadStone m-3 ml-5 text-center align-content-center">{{stones[0]}}</div>
    <div v-for="stone in stones.slice(1)" class="stone m-3 text-center align-content-center">
      {{stone}}
    </div>
  </div>

  <p>
    In an RBT, a drawn stone’s color is cross-referenced to the neutral stone’s color via the Random Bonus Table, 
    illustrated below. Comparing two colors generates a number between 0 and 5 called the 
    Random Bonus (RB). For example, if a player draws a red stone and the neutral stone is green, a RB of +4 is 
    generated. If a player is entitled to draw multiple stones for the test,  she chooses the stone giving her the 
    higher RB unless.
    
    The bag has 1 stone of each color.
  </p>
  
  <DataTable :value="table" table-style="min-width: 50rem">
    <Column field="stone" header="" />
    <Column field="black" header="Black" />
    <Column field="blue" header="Blue" />
    <Column field="clear" header="Clear" />
    <Column field="green" header="Green" />
    <Column field="red" header="Red" />
    <Column field="white" header="White" />
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