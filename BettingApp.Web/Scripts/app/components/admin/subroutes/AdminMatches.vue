<template>
    <div>
        Događaji bez ishoda:
        1 X 2 Odredi ishod
        <div v-for="match in matchesWithoutOutcome">
            {{match.homeTeam.name}} - {{match.awayTeam.name}}
            {{match.timeOfStart | formatDate}}
            <input type="number" step="0.01" v-model.number="match.homeWinOdd" v-on:change="changeMatchOdds($event, match.id, 0)"/>
            <span v-if="match.drawOdd !== null">
                <input type="number" step="0.01"  v-model.number="match.drawOdd" v-on:change="changeMatchOdds($event, match.id, 1)" />
            </span>
            <span v-else>-</span>
            <input type="number" step="0.01" v-model.number="match.awayWinOdd" v-on:change="changeMatchOdds($event, match.id, 2)"/>
            <button v-on:click.prevent="setOutcome(match.id, 0)">1</button>
            <button v-on:click.prevent="setOutcome(match.id, 1)">X</button>
            <button v-on:click.prevent="setOutcome(match.id, 2)">2</button>
            <button v-on:click.prevent="setOutcome(match.id, 3)">Predaja</button>
        </div>
    </div>
</template>

<script>
    import debounce from 'lodash/debounce'
    import axios from 'axios'
    export default {
        name: 'AdminMatches',
        data() {
            return {
                matchesWithoutOutcome: []
            }
        },
        methods: {
            changeMatchOdds: debounce(function (event, matchId, oddToChange) {
                axios.post('/api/admin/matches/odds',
                    {
                        matchId: matchId,
                        oddToChange: oddToChange,
                        newOdd: event.target.value
                    }).catch(error => {
                        alert('Promjena kvote nije prihvaćena.\n'
                            + 'Događaj je već počeo ili nemate pristup.');
                        this.$router.go(this.$router.currentRoute);
                });
            }, 1000),
            setOutcome: function (matchId, outcome) {
                axios.post('/api/admin/matches/outcome',
                    {
                        matchId: matchId,
                        outcome: outcome
                    }).then(response => {
                        const indexOfMatch = this.matchesWithoutOutcome
                            .findIndex(match => match.id === matchId);
                        this.matchesWithoutOutcome.splice(indexOfMatch, 1);
                    }).catch(error => {
                        alert('Postavljeni ishod nije prihvaćen.\n'
                            + 'Remi nije moguć ili nemate pristup.');
                    });
            }
        },
        created() {
            axios.get('/api/admin/matches')
                .then(response => {
                    this.matchesWithoutOutcome = response.data;
                }).catch(function (error) {
                    console.log(error);
                });
        }
    }
</script>