<template>
    <div class="template-container">
        <router-link to="/login">Prijava</router-link>
        <router-link to="/user/matches">Događaji</router-link>
        <img v-bind:src="coverPhoto"/>
        <span class="home-message">Dobrodošli u Kladaru! Jedini izbor od 1996.</span>
    </div>
</template>

<script>
    export default {
        name: 'Home',
        data() {
            return {
                coverPhoto: require('../../../wwwroot/assets/images/homepage.jpg')
            }
        }
    }
</script>