<template>
    <div class="template-container">
        <div class="menu">
            <router-link to="/admin/matches">Događaji</router-link>
            <router-link to="/admin/teams">Timovi</router-link>
            <router-link to="/admin/sports">Sportovi</router-link>
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
        name: 'Admin',
        methods: {
            logout: function () {
                localStorage.removeItem('userId');
            }
        }
    }
</script>