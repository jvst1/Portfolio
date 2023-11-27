<template>
    <v-container>
        <v-row>
            <v-col cols="12" md="4" lg="3">
                <v-tabs vertical v-model="activeTab" @change="handleTabChange">
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
import ClienteConta from './ClienteConta.vue';
import ClienteEndereco from './ClienteEndereco.vue';

export default {
    data() {
        return {
            activeTab: 0,
            tabs: [
                { name: 'conta', title: 'Minha Conta', component: ClienteConta },
                { name: 'endereco', title: 'Meus Endereços', component: ClienteEndereco },
            ],
        };
    },
    methods: {},
};
</script>