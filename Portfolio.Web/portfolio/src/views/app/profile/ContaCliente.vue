<template>
    <v-container fluid>
        <v-card flat>
            <v-card-text>
                <input-text label="Nome completo" placeholder="Informe o nome completo" v-model="formData.nome"
                    :maxlength="255" :disabled="true" required />
                <input-documento label="Documento federal" placeholder="Informe o CPF ou CNPJ"
                    v-model="formData.documentoFederal" :disabled="true" required />
                <input-email label="Email" placeholder="Informe o email. Exemplo: example@example.com"
                    v-model="formData.email" :maxlength="100" :disabled="true" required />
                <input-text label="Apelido" placeholder="Informe um apelido" v-model="formData.identificador"
                    :maxlength="100" required />
                <input-phone label="Número de telefone" placeholder="Informe o número de telefone (UF) X XXXX-XXXX"
                    v-model="formData.telefoneCelular" required />
            </v-card-text>
            <v-btn text color="primary" @click="save">Salvar alterações</v-btn>
            <v-btn color="error" @click="logout">Log out</v-btn>
        </v-card>
    </v-container>
</template>
  
<script>
export default {
    name: "AccountPage",
    data() {
        return {
            formData: {
                nome: "",
                documentoFederal: "",
                email: "",
                identificador: "",
                telefoneCelular: ""
            },
        };
    },
    methods: {
        initPage() {
            this.$api.Usuario.Get(this.$store.getters.user.codigoUsuario).then((response) => {
                this.formData = { ...response };
            });
        },
        save() {
            const dto = { ...this.formData };
            let codigoUsuario = this.$store.getters.user.codigoUsuario;
            dto.codigo = codigoUsuario;

            this.$api.Usuario.Put(codigoUsuario, dto).then((data) => {
                if (this.$api.UI.PostAction(data, `Cadastro de "${dto.identificador}" atualizado com sucesso.`))
                    this.initPage();
            });
        },
        logout() {
            this.$store.dispatch('logout')
        },
    },
    mounted() {
        this.initPage();
    },
};
</script>