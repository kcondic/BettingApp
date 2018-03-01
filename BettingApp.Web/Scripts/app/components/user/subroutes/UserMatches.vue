<template>
    <div>
        <div class="sub-menu">
            <span v-on:click.prevent="getByDay('today')">Današnje oklade</span>
            <span v-on:click.prevent="getByDay('tomorrow')">Sutrašnje oklade</span>
            <select v-model="selected">
                <option disabled value="">Oklade po sportu</option>
                <option v-for="sport in sports"
                        v-bind:value="{id: sport.id}">
                    {{sport.name}}
                </option>
            </select>
        </div>
        <span v-if="wallet">Sredstva za klađenje: {{wallet.funds}} kn</span>
        <div v-if="!sportsWithMatches.length">
            Trenutno nema događaja.
        </div>
        <div v-for="sport in sportsWithMatches">
            <div v-if="sport.length">
                <span class="match-general-info user">
                    <span v-if="sport[0] && sport[0].homeTeam.sport">{{sport[0].homeTeam.sport.name}}</span>
                </span>
                <span class="match-odd">1</span>
                <span class="match-odd">X</span>
                <span class="match-odd">2</span>
            </div>
            <div v-else>
                Trenutno nema događaja.
            </div>
            <div v-for="match in sport">
                <span class="match-general-info user">
                    {{match.homeTeam.name}} -
                    {{match.awayTeam.name}}
                    {{match.timeOfStart | formatDate}}
                </span>
                <span class="match-odd user" v-bind:class="{ active: isSelected(match.id, 0) }" v-if="match.homeWinOdd" v-on:click.prevent="tipOnMatch(match, 0, match.homeWinOdd)">{{match.homeWinOdd}}</span>
                <span class="match-odd" v-else>-</span>
                <span class="match-odd user" v-bind:class="{ active: isSelected(match.id, 1) }" v-if="match.drawOdd" v-on:click.prevent="tipOnMatch(match, 1, match.drawOdd)">{{match.drawOdd}}</span>
                <span class="match-odd" v-else>-</span>
                <span class="match-odd user" v-bind:class="{ active: isSelected(match.id, 2) }" v-if="match.awayWinOdd" v-on:click.prevent="tipOnMatch(match, 2, match.awayWinOdd)">{{match.awayWinOdd}}</span>
                <span class="match-odd" v-else>-</span>
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
                <span class="remove-button" v-on:click.prevent="removeTip(matchTip)">❌</span>
            </div>
            <div>Ukupni koeficijent: {{totalOddWithBonus}}</div>
            <div>Ulog: <input type="number" v-model.number="stake" min="2"/> kn</div>
            <div>Mogući dobitak: {{possibleWin}} kn</div>
            <div><button v-on:click.prevent="placeBet()">Uplati</button></div>
            <div v-if="bonus.messages" v-for="bonusMessage in bonus.messages">
                {{bonusMessage}}
            </div>
        </form>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'UserMatches',
        data() {
            return {
                sportsWithMatches: [],
                sports: [],
                tips: [],
                selected: '',
                totalOdd: 1,
                bonus: {},
                stake: 2,
                wallet: {}
            }
        },
        computed: {
            totalOddWithBonus: function () {
                return Math.round((this.totalOdd + this.bonus.bonusOdd) * 100) / 100;
            },
            possibleWin: function () {
                return Math.round(this.stake * (this.totalOdd + this.bonus.bonusOdd) * 100) / 100;
            }
        },
        methods: {
            getByDay: function (dayName) {
                this.selected = '';
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
                    this.checkBonuses();
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
                this.checkBonuses();
                this.calculateOdds(matchTip.odd, 'decrease');
            },
            calculateOdds: function (odd, operation) {
                if (operation === 'increase')
                    this.totalOdd *= odd;
                else
                    this.totalOdd /= odd;
                this.totalOdd = Math.round((this.totalOdd) * 100) / 100;
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
                    TotalOdd: this.totalOdd + this.bonus.bonusOdd
                };
                axios.post('/api/user/bet', newTicket)
                    .then(response => {
                        alert('Listić uspješno uplaćen.');
                        this.wallet.funds -= this.stake;
                        this.totalOdd = 1;
                        this.tips = [];
                }).catch(error => {
                    alert('Listić nije uplaćen. Nemate dovoljno\n' +
                        'sredstava ili Vam je zabranjen pristup.');
                });
            },
            checkBonuses: function () {
                const ticketMatches = [];
                for (let matchTip of this.tips)
                    ticketMatches.push({
                        MatchId: matchTip.match.id
                    });                
                axios.post('/api/user/bonus', ticketMatches)
                    .then(response => {
                        this.bonus = response.data;
                });
            },
            isSelected: function (matchId, odd) {
                const indexOfMatchInTips = this.tips.findIndex(tip => tip.match.id === matchId);
                if (indexOfMatchInTips === -1)
                    return false;
                if (this.tips[indexOfMatchInTips].tip === odd)
                    return true;
                return false;
            }
        },
        watch: {
            selected: function(newVal)
            {
                this.getForSport(newVal.id);
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