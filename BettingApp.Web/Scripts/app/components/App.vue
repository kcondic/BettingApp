<template>
    <div>
        <router-link to="test">TEST</router-link>
        <router-view class="view"
                     keep-alive
                     transition
                     transition-mode="out-in">
        </router-view>
    </div>
</template>