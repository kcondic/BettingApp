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
                <span v-if="match.homeWinOdd" v-on:click.prevent="tipOnMatch(match, 0, match.homeWinOdd)">{{match.homeWinOdd}}</span>
                <span v-else>-</span>
                <span v-if="match.drawOdd" v-on:click.prevent="tipOnMatch(match, 1, match.drawOdd)">{{match.drawOdd}}</span>
                <span v-else>-</span>
                <span v-if="match.awayWinOdd" v-on:click.prevent="tipOnMatch(match, 2, match.awayWinOdd)">{{match.awayWinOdd}}</span>
                <span v-else>-</span>
            </div>
        </div>
        <form v-if="tips.length">
            Listić
            <div v-for="(matchTip, index) in tips">
                {{index+1}}.
                {{matchTip.match.homeTeamName}} -
                {{matchTip.match.awayTeamName}}
                <span v-if="matchTip.Tip === 0">
                    1
                </span>
                <span v-else-if="matchTip.Tip === 1">
                    X
                </span>
                <span v-else>
                    2
                </span>
                {{matchTip.odd}}
                <span v-on:click.prevent="removeTip(matchTip)">❌</span>
            </div>
            <div>Ukupni koeficijent: {{totalOdd}}</div>
            <div>Ulog: <input type="number" v-model="stake" min="2"/> kn</div>
            <div>Mogući dobitak: {{possibleWin}} kn</div>
            <div><button v-on:click.prevent="placeBet()">Uplati</button></div>
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
                stake: 2,
                wallet: {}
            }
        },
        computed: {
            possibleWin: function () {
                return Math.round(this.stake * this.totalOdd * 100) / 100;
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
            tipOnMatch: function (match, tip, odd) {
                let indexOfExistingTip = this.tips.findIndex(tip => tip.match.id === match.id);              
                if (indexOfExistingTip === -1) {
                    this.tips.push({
                        match: {
                            id: match.id,
                            homeTeamName: match.homeTeam.name,
                            awayTeamName: match.awayTeam.name
                        },
                        tip: tip,
                        odd: odd
                    });
                    this.calculateOdds(odd, 'increase');
                }
                else {
                    this.calculateOdds(this.tips[indexOfExistingTip].odd, 'decrease');
                    this.tips[indexOfExistingTip].tip = tip;
                    this.calculateOdds(odd, 'increase');
                }
            },
            removeTip: function(matchTip) {
                this.tips.splice(this.tips.indexOf(matchTip), 1);
                this.calculateOdds(matchTip.odd, 'decrease');
            },
            calculateOdds: function (odd, operation) {
                if (operation === 'increase')
                    this.totalOdd *= odd;
                else
                    this.totalOdd /= odd;
                this.totalOdd = Math.round(this.totalOdd * 100) / 100;
            },
            placeBet: function () {
                const ticketMatches = [];
                for (let matchTip of this.tips)
                    ticketMatches.push({
                        MatchId: matchTip.match.id,
                        Tip: matchTip.tip,
                        PlacedOdd: matchTip.odd
                    });
                const newTicket = {
                    Wallet: this.wallet,
                    TicketMatches: ticketMatches,
                    Stake: this.stake,
                    TotalOdd: this.totalOdd
                };
                axios.post('/api/user/bet', newTicket)
                    .then(response => {
                        alert('Listić uspješno uplaćen.');
                        this.wallet.funds -= this.stake;
                        this.tips = [];
                }).catch(error => {
                    alert('Listić nije uplaćen. Nemate dovoljno\n' +
                        'sredstava ili Vam je zabranjen pristup.');
                });
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