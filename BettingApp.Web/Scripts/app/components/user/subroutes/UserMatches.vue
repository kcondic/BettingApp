<template>
    <div>
        <span v-on:click.prevent="getByDay('today')">Današnje oklade</span>
        <span v-on:click.prevent="getByDay('tomorrow')">Sutrašnje oklade</span>
        <select>
            <option>Oklade po sportu</option>
            <option v-for="sport in sports" 
                v-on:click.prevent="getForSport(sport.id)">
                {{sport.name}}</option>
        </select>
        <span>Sredstva za klađenje: {{wallet.funds}} kn</span>
        <div v-for="sport in sportsWithMatches">
            <span v-if="sport[0].homeTeam.sport">{{sport[0].homeTeam.sport.name}}</span>
            <span class="tips">1 X 2</span>
            <div v-for="match in sport">
                {{match.homeTeam.name}} -
                {{match.awayTeam.name}}
                {{match.timeOfStart | formatDate}}
                <span v-if="match.homeWinOdd" v-on:click.prevent="tipOnMatch(match, 0)">{{match.homeWinOdd}}</span>
                <span v-else>-</span>
                <span v-if="match.drawOdd" v-on:click.prevent="tipOnMatch(match, 1)">{{match.drawOdd}}</span>
                <span v-else>-</span>
                <span v-if="match.awayWinOdd" v-on:click.prevent="tipOnMatch(match, 2)">{{match.awayWinOdd}}</span>
                <span v-else>-</span>
            </div>
        </div>
        <form v-if="tips.length">
            Listić
            <div v-for="(matchTip, index) in tips">
                {{index+1}}.
                {{matchTip.match.homeTeam.name}} -
                {{matchTip.match.awayTeam.name}}
                <span v-if="matchTip.tip === 0">
                    1
                    {{matchTip.match.homeWinOdd}}
                </span>
                <span v-else-if="matchTip.tip === 1">
                    X
                    {{matchTip.match.drawOdd}}
                </span>
                <span v-else>
                    2
                    {{matchTip.match.awayWinOdd}}
                </span>
                <span v-on:click.prevent="removeTip(matchTip)">❌</span>
            </div>
            <div>Ukupni koeficijent: {{totalOdd}}</div>
            <div>Ulog: <input type="number" v-model="stake" /></div>
            <div><button v-on:click.prevent>Uplati</button></div>
        </form>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'UserMatches',
        data() {
            return {
                sportsWithMatches: [],
                sports: [],
                tips: [],
                totalOdd: 1,
                stake: 0,
                wallet: {}
            }
        },
        methods: {
            getByDay: function (dayName) {
                axios.get('/api/matches/day', {
                    params: {
                        dayOfMatches: dayName
                    }
                }).then(response => {
                    this.sportsWithMatches = response.data;
                });
            },
            getForSport: function (sportId) {
                axios.get('/api/matches/sport', {
                    params: {
                        sportId: sportId
                    }
                }).then(response => {
                    this.sportsWithMatches = [response.data];
                    });
            },
            tipOnMatch: function (match, tip) {
                let indexOfExistingTip = this.tips.findIndex(tip => tip.match.id === match.id);              
                if (indexOfExistingTip === -1) {
                    this.tips.push({
                        match: match,
                        tip: tip
                    });
                    this.calculateOdds(match, tip, 'increase');
                }
                else {
                    this.calculateOdds(match, this.tips[indexOfExistingTip].tip, 'decrease');
                    this.tips[indexOfExistingTip].tip = tip;
                    this.calculateOdds(match, tip, 'increase');
                }
            },
            removeTip: function(matchTip) {
                this.tips.splice(this.tips.indexOf(matchTip), 1);
                this.calculateOdds(matchTip.match, matchTip.tip, 'decrease');
            },
            calculateOdds: function (match, tip, operation) {
                switch (operation) {
                    case 'increase':
                        switch (tip) {
                            case 0:
                                this.totalOdd *= match.homeWinOdd;
                                break;
                            case 1:
                                this.totalOdd *= match.drawOdd;
                                break;
                            case 2:
                                this.totalOdd *= match.awayWinOdd;
                                break;
                        }
                        break;
                    case 'decrease':
                        switch (tip) {
                            case 0:
                                this.totalOdd /= match.homeWinOdd;
                                break;
                            case 1:
                                this.totalOdd /= match.drawOdd;
                                break;
                            case 2:
                                this.totalOdd /= match.awayWinOdd;
                                break;
                        }
                        break;
                }
                this.totalOdd = Math.round(this.totalOdd * 100) / 100;
            }
        },
        created() {
            this.getByDay('today');
            axios.get('/api/matches/sports').then(response => {
                this.sports = response.data;
            });
            let userId = localStorage.getItem('userId');
            axios.get('/api/user', {
                params: {
                    userId: userId
                }
            }).then(response => {
                this.wallet = response.data;
            });
        }
    }
</script>