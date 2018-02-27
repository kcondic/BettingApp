<template>
    <div>
        <form>
            Prijavi se
            <input type="text" v-model="username" placeholder="Korisničko ime">
            <button v-on:click.prevent="signIn(username)">Prijava</button>
        </form>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'Login',
        data() {
            return {
                username: ''
            }
        },
        methods: {
            signIn: function (username) {
                axios.post('/api/login',
                    {
                        username: username
                    }
                ).then(response => {
                    localStorage.setItem("userId", response.data.id);
                    if (response.data.role)
                        this.$router.push('admin');
                    else {
                        localStorage.setItem("walletId", response.data.wallet.id);
                        this.$router.push('user');
                    }
                    }).catch(error => {
                        alert("Neispravni korisnički podaci.");
                    });
            }
        }
    }
</script>