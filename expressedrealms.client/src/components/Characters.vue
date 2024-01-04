<template>
    <div class="weather-component">
        <h1>Character DB Test</h1>
        <p>This component demonstrates fetching data from the database.</p>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="character in post" :key="character.id">
                        <td>{{ character.id }}</td>
                        <td>{{ character.name }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
      
      
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type Characters = {
        id: number,
        name: string,
    }[];

    interface Data {
        loading: boolean,
        post: null | Characters
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.post = null;
                this.loading = true;

                fetch('/api/characters')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as Characters;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>

<style scoped>
th {
    font-weight: bold;
}
tr:nth-child(even) {
    background: #F2F2F2;
}

tr:nth-child(odd) {
    background: #FFF;
}

th, td {
    padding-left: .5rem;
    padding-right: .5rem;
}

.weather-component {
    text-align: center;
}

table {
    margin-left: auto;
    margin-right: auto;
}
</style>