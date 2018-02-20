import Vue from 'vue'
import Router from 'vue-router'
import App from './components/App.vue'
import Test from './components/Test.vue'

Vue.use(Router);
const routes = [
    { path: '/test', component: Test }
];


const router = new Router({
    mode: 'history',
    routes // short for `routes: routes`
});

/* eslint-disable no-new */
const app = new Vue({
    el: '#app',
    router,
    render: h => h(App)
});