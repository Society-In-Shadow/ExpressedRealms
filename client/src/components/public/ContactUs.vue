<script setup lang="ts">
import Tabs from 'primevue/tabs';
import TabList from 'primevue/tablist';
import Tab from 'primevue/tab';
import TabPanels from 'primevue/tabpanels';
import TabPanel from 'primevue/tabpanel';
import Card from 'primevue/card';
</script>

<template>
  <Card>
    <template #title>
      Contact Us
    </template>
    <template #content>
      <p>Blurb about reaching out to us in general.  See individual tabs below for more information.</p>
      <Tabs value="0" scrollable>
        <TabList>
          <Tab value="0">
            Parents / Guardians
          </Tab>
          <Tab value="1">
            Players
          </Tab>
          <Tab value="2">
            Event Organizers / Conventions
          </Tab>
          <Tab value="3">
            Other
          </Tab>
        </TabList>
        <TabPanels>
          <TabPanel value="0">
            <p class="m-0">
              Information relevant to parents / guardians will be posted here.
            </p>
            <p>Redirect them to the About Page</p>
            <p>Form:</p>
            <ul>
              <li>Parent Name (required)</li>
              <li>Parent Email (required)</li>
              <li>Child Info</li>
              <li>Title (required)</li>
              <li>Message (required)</li>
            </ul>
          </TabPanel>
          <TabPanel value="1">
            <p class="m-0">
              Information relevant to players will be posted here.
            </p>
            <p>
              Should encourage them to use the discord server for communication, and who they should contact via discord
            </p>
            <p>Form:</p>
            <ul>
              <li>Player Email (required)</li>
              <li>Title (required)</li>
              <li>Messsage (required)</li>
            </ul>
          </TabPanel>
          <TabPanel value="2">
            <p class="m-0">
              Information relevant to conventions will be posted here.
            </p>
            <p>Something about how many people we bring to the table on average per convention</p>
            <p>Form:</p>
            <ul>
              <li>Convention Name (required)</li>
              <li>Point of Contact Name (required)</li>
              <li>Point of Contact Email (required)</li>
              <li>Website</li>
              <li>Title (required)</li>
              <li>Message (required)</li>
            </ul>
          </TabPanel>
          <TabPanel value="3">
            <p class="m-0">
              Information relevant to general interest will be posted here.
            </p>
            <p>Form:</p>
            <ul>
              <li>Name (required)</li>
              <li>Email (required)</li>
              <li>Title (required)</li>
              <li>Message (required)</li>
            </ul>
          </TabPanel>
        </TabPanels>
      </Tabs>
    </template>
  </Card>
</template>
