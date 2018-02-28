<template>
    <div>
        Događaji bez ishoda:
        <div>
            <span class="match-general-info"></span>
            <span class="match-odd">1</span>
            <span class="match-odd">X</span>
            <span class="match-odd">2</span>
            <span>Odredi ishod</span>
        </div>
        <div v-for="match in matchesWithoutOutcome">
            <span class="match-general-info">
                {{match.homeTeam.name}} - {{match.awayTeam.name}}
                {{match.timeOfStart | formatDate}}
            </span>
            <span class="match-odd">
                <input type="number" step="0.01" v-model.number="match.homeWinOdd" v-on:change="changeMatchOdds($event, match.id, 0)" />
            </span>         
            <span class="match-odd" v-if="match.homeTeam.sport.isDrawPossible">
                <input type="number" step="0.01" v-model.number="match.drawOdd" v-on:change="changeMatchOdds($event, match.id, 1)" />
            </span>
            <span class="match-odd" v-else>-</span>
            <span class="match-odd">
                <input type="number" step="0.01" v-model.number="match.awayWinOdd" v-on:change="changeMatchOdds($event, match.id, 2)" />
            </span>         
            <button v-on:click.prevent="setOutcome(match.id, 0)" v-if="match.homeWinOdd">1</button>
            <button v-on:click.prevent="setOutcome(match.id, 1)" v-if="match.drawOdd && match.homeTeam.sport.isDrawPossible">X</button>
            <button v-on:click.prevent="setOutcome(match.id, 2)" v-if="match.awayWinOdd">2</button>
            <button v-on:click.prevent="setOutcome(match.id, 3)">Predaja</button>
        </div>
        <form>
            Dodaj novi događaj:
            <select v-model="selectedSport">
                <option disabled value="">Odaberi sport</option>
                <option v-for="sport in sports" v-bind:value="{sport: sport}">
                    {{sport.name}}
                </option>
            </select>
            <div class="add-match-options" v-if="selectedSport">
                <div>
                    <select v-model="selectedHomeTeam">
                        <option disabled value="">Odaberi domaći tim</option>
                        <option v-for="team in selectedSport.sport.teams" v-bind:value="{team: team}" v-bind:disabled="getHomeDisabled(team)">
                            {{team.name}}
                        </option>
                    </select>
                    <select v-model="selectedAwayTeam">
                        <option disabled value="">Odaberi gostujući tim</option>
                        <option v-for="team in selectedSport.sport.teams" v-bind:value="{team: team}" v-bind:disabled="getAwayDisabled(team)">
                            {{team.name}}
                        </option>
                    </select>
                </div>
                <div>
                    <input type="number" step="0.01" v-model="newMatchHomeWinOdd" placeholder="1" />
                    <span v-if="selectedSport.sport.isDrawPossible">
                        <input type="number" step="0.01" v-model="newMatchDrawOdd" placeholder="X" />
                    </span>
                    <input type="number" step="0.01" v-model="newMatchAwayWinOdd" placeholder="2" />
                </div> 
                <div>
                    Vrijeme:
                    <input type="date" v-model="newMatchDate" />
                    <input type="number" v-model="newMatchHour" min="0" max="23" placeholder="HH" /> :
                    <input type="number" v-model="newMatchMinute" min="0" max="59" placeholder="mm" />
                </div>
                <button v-on:click.prevent="addNewMatch()">Dodaj</button>
            </div>
        </form>
    </div>
</template>

<script>
    import debounce from 'lodash/debounce';
    import axios from 'axios';
    export default {
        name: 'AdminMatches',
        data() {
            return {
                matchesWithoutOutcome: [],
                sports: [],
                teams: [],
                selectedSport: '',
                selectedHomeTeam: '',
                selectedAwayTeam: '',
                newMatchHomeWinOdd: null,
                newMatchDrawOdd: null,
                newMatchAwayWinOdd: null,
                newMatchDate: '',
                newMatchHour: null,
                newMatchMinute: null
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
            },
            addNewMatch: function () {
                const newMatch = {
                    HomeTeam: this.selectedHomeTeam.team,
                    AwayTeam: this.selectedAwayTeam.team,
                    HomeWinOdd: this.newMatchHomeWinOdd,
                    DrawOdd: this.newMatchDrawOdd,
                    AwayWinOdd: this.newMatchAwayWinOdd,
                    TimeOfStart: this.newMatchDate + ' ' + this.newMatchHour
                                        + ':' + this.newMatchMinute + ':00'
                };
                axios.post('/api/admin/matches', newMatch)
                    .then(response => {
                        this.$router.go(this.$router.currentRoute);
                    }).catch(error => {
                        alert('Događaj nije dodan.\n'
                            + 'Neispravni podaci ili nemate pristup.');
                    });
            },
            getHomeDisabled: function (team) {
                if (this.selectedAwayTeam)
                    if (team.id === this.selectedAwayTeam.team.id)
                        return true;
                return false;
            },
            getAwayDisabled: function (team) {
                if (this.selectedHomeTeam)
                    if (team.id === this.selectedHomeTeam.team.id)
                        return true;
                return false;
            }
        },
        created() {
            axios.get('/api/admin/matches')
                .then(response => {
                    this.matchesWithoutOutcome = response.data;
                });
            axios.get('/api/matches/sports')
                .then(response => {
                    this.sports = response.data;
                });
        }
    }
</script>