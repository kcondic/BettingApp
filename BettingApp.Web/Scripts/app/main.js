import Vue from 'vue';
import Router from 'vue-router';
import axios from 'axios';
import moment from 'moment';
import App from './components/App.vue';
import Home from './components/Home.vue'
import Login from './components/auth/Login.vue';
import User from './components/user/layout/User.vue';
import UserMatches from './components/user/subroutes/UserMatches.vue';
import UserWallet from './components/user/subroutes/UserWallet.vue';
import UserTickets from './components/user/subroutes/UserTickets.vue';
import Admin from './components/admin/layout/Admin.vue';
import AdminMatches from './components/admin/subroutes/AdminMatches.vue';
import AdminTeams from './components/admin/subroutes/AdminTeams.vue';
import AdminSports from './components/admin/subroutes/AdminSports.vue';

Vue.use(Router);
Vue.use(axios);

Vue.filter('formatDate', function (value) {
    if (value) {
        return moment(String(value)).format('D.M. H:mm');
    }
});

const routes = [
    {
        path: '/', component: Home
    },
    {
        path: '/login', component: Login
    },
    {
        path: '/user', component: User, redirect: 'user/matches',
        children: [
            {
                 path: 'matches', component: UserMatches
            },
            {
                path: 'wallet', component: UserWallet
            },
            {
                path: 'tickets', component: UserTickets
            }]
    },
    {
        path: '/admin', component: Admin, redirect: 'admin/matches',
        children: [
            {
                path: 'matches', component: AdminMatches
            },
            {
                path: 'teams', component: AdminTeams
            },
            {
                path: 'sports', component: AdminSports
            }]
    }
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