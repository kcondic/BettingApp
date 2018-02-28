<template>
    <div class="template-container">
        <div class="menu" v-if="isLoggedIn">
            <router-link to="/user/matches">Događaji</router-link>
            <router-link to="/user/wallet">Moj novčanik</router-link>
            <router-link to="/user/tickets">Oklade</router-link>
            <router-link to="/" v-on:click.native="logout()">Odjava</router-link>
        </div>
        <router-view
                     keep-alive
                     transition
                     transition-mode="out-in">
        </router-view>
    </div>
</template>

<script>
    export default {
        name: 'User',
        computed: {
            isLoggedIn: function () {
                if (localStorage.getItem('walletId'))
                    return true;
                return false;
            }
        },
        methods: {
            logout: function () {
                console.log()
                localStorage.removeItem('userId');
                localStorage.removeItem('walletId');
            }
        }
    }
</script>