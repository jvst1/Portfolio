<template>
    <v-container>
        <v-row>
            <v-col cols="12" md="4" lg="3">
                <v-tabs vertical v-model="activeTab">
                    <v-tab v-for="tab in tabs" :key="tab.name">{{ tab.title }}</v-tab>
                </v-tabs>
            </v-col>
            <v-col cols="12" md="8" lg="9">
                <v-tabs-items v-model="activeTab">
                    <v-tab-item v-for="tab in tabs" :key="tab.name">
                        <component :is="tab.component" />
                    </v-tab-item>
                </v-tabs-items>
            </v-col>
        </v-row>
    </v-container>
</template>
  
<script>
import ContaComerciante from './profile/ContaComerciante.vue';
import CardapioComerciante from './profile/CardapioComerciante.vue';

export default {
    data() {
        return {
            activeTab: 0,
            tabs: [
                { name: 'conta', title: 'Minha Conta', component: ContaComerciante },
                { name: 'cardapio', title: 'Meu Cardápio', component: CardapioComerciante },
            ],
        };
    },
    methods: {},
};
</script>