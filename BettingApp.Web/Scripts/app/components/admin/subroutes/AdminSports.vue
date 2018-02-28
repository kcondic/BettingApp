<template>
    <div>
        <div>
            Trenutni sportovi:
            <div v-for="sport in sports">
                {{sport.name}}
                {{sport.teams.length}} timova
            </div>
        </div>
        <form>
            <span class="form-item">Dodaj novi sport</span>
            <span class="form-item">
                <input type="text" v-model="newSportName" />
            </span>
            <span class="form-item">
                <input type="checkbox" v-model="isDrawPossible" />
                <label for="checkbox">Remi moguć?</label>
            </span>
            <span class="form-item">
                <button v-on:click.prevent="addNewSport()">Dodaj</button>
            </span>       
        </form>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'AdminSports',
        data() {
            return {
                sports: [],
                newSportName: '',
                isDrawPossible: true
            }
        },
        methods: {
            addNewSport: function () {
                const newSport = {
                    Name: this.newSportName,
                    IsDrawPossible: this.isDrawPossible
                };
                axios.post('/api/admin/sports', newSport)
                    .then(response => {
                        this.sports.push(
                            {
                                name: this.newSportName,
                                teams: []
                            });
                        this.newSportName = '';
                    }).catch(error => {
                        alert('Sport nije dodan. Odbijen pristup.');
                    });
            }
        },
        created() {
            axios.get('/api/matches/sports').then(response => {
                this.sports = response.data;
            });
        }
    }
</script>
