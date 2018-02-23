<template>
    <div>
        <router-link to="/admin/matches">Događaji</router-link>
        <router-link to="/admin/teams">Timovi</router-link>
        <router-link to="/admin/sports">Sportovi</router-link>
        <router-link to="/">Odjava</router-link>
        <router-view class="view"
                     keep-alive
                     transition
                     transition-mode="out-in">
        </router-view>
    </div>
</template>

<script>
    export default {
        name: 'Admin'
    }
</script>