<template>
    <div>
        <router-link to="/user/matches">Događaji</router-link>
        <router-link to="/user/wallet">Moj novčanik</router-link>
        <router-link to="/user/tickets">Oklade</router-link>
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
        name: 'User'
    }
</script>