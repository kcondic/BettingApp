<template>
    <div>
        <form>Uplati sredstva: 
        <input type="number" v-model="payment" min="10"/>
        <button v-on:click.prevent="fundsPayment()">Uplati</button>
        </form>
        <div v-for="transaction in transactions">
            <span v-if="transaction.transactionType === 0">OKLADA -</span>
            <span v-else-if="transaction.transactionType === 1">ISPLATA </span>
            <span v-else>UPLATA </span>
            <span>{{transaction.transactionAmount}} kn</span>
            <div>{{transaction.timeOfTransaction | formatDate}}</div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'UserWallet',
        data() {
            return {
                payment: 10,
                transactions: [],
                walletId: localStorage.getItem('walletId')
            }
        },
        methods: {
            fundsPayment: function () {
                axios.post('/api/user/wallet', {
                    walletId: this.walletId,
                    fundsToGrant: this.payment
                })
                .then(response => {
                    alert('Sredstva uspješno uplaćena.');
                    this.transactions.unshift({
                        transactionType: 2,
                        transactionAmount: this.payment,
                        timeOfTransaction: new Date()
                    });
                }).catch(error => {
                    alert('Uplata nije odobrena!');
                });
            }
        },
        created() {
            axios.get('/api/user/wallet', {
                params: {
                    walletId: this.walletId
                }
            }).then(response => {
                this.transactions = response.data;
            });
        }
    }
</script>