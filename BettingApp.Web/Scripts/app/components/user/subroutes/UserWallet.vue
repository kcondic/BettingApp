<template>
    <div>
        <form>
            <span class="form-item">Uplati sredstva:</span>  
            <span class="form-item"><input type="number" v-model.number="payment" min="10" /></span>
            <span class="form-item"><button v-on:click.prevent="fundsPayment()">Uplati</button></span>      
        </form>
        <div class="transactions" v-for="transaction in transactions">
            <span class="transaction-negative" v-if="transaction.transactionType === 0">OKLADA</span>
            <span class="transaction-positive" v-else-if="transaction.transactionType === 1">DOBIVENA OKLADA</span>
            <span class="transaction-positive" v-else>UPLATA</span> 
            <span>{{transaction.timeOfTransaction | formatDate}} {{transaction.transactionAmount}} kn</span>
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