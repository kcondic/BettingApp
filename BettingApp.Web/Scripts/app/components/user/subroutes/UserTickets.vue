<template>
    <div>
        Pregled oklada
        <div v-for="ticket in tickets">
            <div v-for="(ticketMatch, index) in ticket.ticketMatches">
                <div>
                    {{index+1}}.
                    {{ticketMatch.match.homeTeam.name}} -
                    {{ticketMatch.match.awayTeam.name}}
                </div>
                <div>{{ticketMatch.match.timeOfStart | formatDate}}</div>
                <div>Tip: <span v-if="ticketMatch.tip === 0">1</span>
                          <span v-else-if="ticketMatch.tip === 1">X</span>
                          <span v-else>2</span>
                </div>
                <div>Koeficijent: {{ticketMatch.placedOdd}}</div>
                <div>Status: <span v-if="ticketMatch.match.outcome === null">Aktivan</span>
                             <span v-else-if="ticketMatch.match.outcome === ticketMatch.tip">Prošao</span>
                             <span v-else-if="ticketMatch.match.outcome === 3">Predaja</span>
                             <span v-else>Pad</span>
                </div>
            </div>
            <div>Ulog: {{ticket.stake}} kn</div>
            <div>Ukupni koeficijent: {{ticket.totalOdd}}</div>
            <div>Mogući dobitak: {{calculatePossibleWin(ticket.stake, ticket.totalOdd)}} kn</div>
        ---------------------------------------------
        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'UserTickets',
        data() {
            return {
                tickets: []
            }
        },
        methods: {
            calculatePossibleWin: function(stake, totalOdd) {
                return Math.round(stake * totalOdd * 100) / 100;
            }
        },
        created() {
            let walletId = localStorage.getItem('walletId');
            axios.get('/api/user/tickets', {
                params: {
                    walletId: walletId
                }
            }).then(response => {
                this.tickets = response.data;
            });
        }
    }
</script>