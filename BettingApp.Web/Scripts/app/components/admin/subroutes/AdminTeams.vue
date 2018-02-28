<template>
    <div>
        <div>
            Timovi:
            <div v-for="sport in sports">
                <span>{{sport.name}}</span>
                <div class="team-list" v-for="team in sport.teams">
                    {{team.name}}
                </div>
            </div>
        </div>
        <form>
            <span class="form-item">Dodaj novi tim:</span>
            <span class="form-item">
                <select v-model="selectedSport">
                    <option disabled value="">Odaberi sport</option>
                    <option v-for="sport in sports" v-bind:value="{sport: sport}">
                        {{sport.name}}
                    </option>
                </select>
            </span>
            <span class="form-item">
                <input type="text" v-model="newTeamName" />
            </span>
            <span class="form-item">
                <button v-on:click.prevent="addNewTeam()">Dodaj</button>
            </span>          
        </form>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'AdminTeams',
        data() {
            return {
                sports: [],
                selectedSport: '',
                newTeamName: ''
            }
        },
        methods: {
            addNewTeam: function () {
                const newTeam = {
                    Name: this.newTeamName,
                    Sport: this.selectedSport.sport
                };
                axios.post('/api/admin/teams', newTeam)
                    .then(response => {
                        const sportIndex = this.sports.findIndex(sport =>
                            sport.id === this.selectedSport.sport.id);
                     this.sports[sportIndex].teams.push(
                        {
                            name: this.newTeamName
                            });
                     this.newTeamName = '';
                    }).catch(error => {
                        alert('Tim nije dodan. Odbijen pristup.');
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