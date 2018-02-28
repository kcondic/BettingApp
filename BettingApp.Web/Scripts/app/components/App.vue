<template>
    <div>
        <router-view class="view"
                     keep-alive
                     transition
                     transition-mode="out-in">
        </router-view>
    </div>
</template>
<style>
    .view
    {
        margin-top: 50px;
    }

    body 
    {
        margin: 0;
        font-size: 16px;
    }

    .template-container 
    {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }

    a
    {
        height: 30px;
        color: black;
        padding: 3px 5px;
        text-decoration: none;
        text-transform: uppercase;
        box-sizing: border-box;
    }

    a:hover 
    {
        border-bottom: 3px solid #99b6e8;
    }

    .router-link-exact-active 
    {
        border-bottom: 3px solid #99b6e8;
    }

    form
    {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .form-item
    {
        margin-top: 5px;
    }

    span
    {
        display: inline-block;
    }

    button 
    {
        color: white;
        font-weight: 600;
        padding: 4px 10px;
        background-color: #afcdff;
        border-radius: 9px 0;
    }

    button:hover
    {
        cursor: pointer;
    }

    .home-message
    {
        font-size: 50px;
        opacity: 0.7;
        font-family: 'Brush Script MT';
    }

    .menu 
    {
        padding: 6px 20px;
        background-color: #afcdff;
        margin-bottom: 30px;
        border-radius: 10px;
    }

    .menu a 
    {
        margin-right: 15px;
        font-size: 20px;
        color: #f4f8ff;
    }

    .sub-menu 
    {
        padding: 6px 20px;
        background-color: #afcdff;
        margin-bottom: 30px;
        border-radius: 10px;
    }

    .sub-menu span, .sub-menu select
    {
        color: white;
        text-transform: uppercase;
        margin-right: 15px;
    }

    .sub-menu span:hover
    {
        cursor: pointer;
        transform: scale(1.05);
    }

    .sub-menu select 
    {
        font-family: 'Times New Roman';
        font-size: 16px;
        outline: none;
        border: none;
        background-color: #afcdff;
    }

    input[type="number"]
    {
        width: 50px;
    }

    .match-general-info
    {
        width: 40%;
    }

    .match-general-info.user {
        width: 60%;
    }

    .match-odd
    {
        text-align: center;
        width: 8%;
    }

    .match-odd.user
    {
        cursor: pointer;
    }

    .match-odd.user:hover
    {
        background-color: #afcdff;
    }

    .match-odd.user.active 
    {
        background-color: #afcdff;
    }

    .remove-button
    {
        cursor: pointer;
    }

    .transactions
    {
        margin-top: 10px;
    }
    
    .transaction-positive
    {
        color: green;
    }

    .transaction-negative
    {
        color: red;
    }

    .ticket 
    {
        padding-bottom: 10px;
        border-bottom: 3px solid #99b6e8;
    }

    .ticket-icon
    {
        position: relative;
        top: -30px;
        float: right;
        height: 20px;
        width: 20px;
    }

    .add-match-options
    {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .add-match-options div, .add-match-options button
    {
        margin-top: 10px;
    }

    .team-list
    {
        padding-left: 20px;
    }

</style>