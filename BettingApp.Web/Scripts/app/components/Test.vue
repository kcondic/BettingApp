<template>
    <div>
        {{message}}
        {{teams}}
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'Test',
        data() {
            return {
                message: 'Poruka',
                teams: axios.get('/test')
            }
        }
    }
</script>