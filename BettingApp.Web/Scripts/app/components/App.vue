<template>
    <div>
        <router-link to="test">TESTT</router-link>
        <router-view class="view"
                     keep-alive
                     transition
                     transition-mode="out-in">
        </router-view>
    </div>
</template>