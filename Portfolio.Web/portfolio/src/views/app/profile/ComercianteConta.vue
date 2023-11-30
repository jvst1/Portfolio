<template>
    <v-container fluid>
        <v-card flat>
            <v-card-title>Estabelecimento comercial</v-card-title>
            <v-card-text>
                <input-text label="Nome" placeholder="Informe o nome do estabelecimento comercial" v-model="formData.nome"
                    :maxlength="255" required />
                <input-documento label="Documento federal" placeholder="Informe o CPF ou CNPJ do estabelecimento comercial"
                    v-model="formData.documentoFederal" required />
                <input-email label="Email"
                    placeholder="Informe o email para contato com o estabelecimento comercial. Exemplo: example@example.com"
                    v-model="formData.email" :maxlength="100" :disabled="true" required />
                <input-text label="Apelido" placeholder="Informe um apelido para o estabelecimento comerical"
                    v-model="formData.identificador" :maxlength="100" required />
                <input-text label="Foto de perfil" placeholder="Informe o URL da sua foto de perfil"
                    v-model="formData.imageUrl" required />
                <input-phone label="Número de telefone"
                    placeholder="Informe o número de telefone do estabelecimento comercial (UF) X XXXX-XXXX"
                    v-model="formData.telefoneCelular" required />

                <input-select label="Tempo médio de entrega" v-model="formData.tempoEntrega" single
                    :options="tempoMedioEntregaEnum" />
                <input-text label="Valor minimo do pedido" placeholder="Informe um valor mínimo para o pedido"
                    v-model="formData.valorMinimoPedido" :maxlength="100" required />

                <v-combobox label="Tags" placeholder="Adicione até 3 tags!" chips multiple @input="onInput" clearable
                    v-model="formData.tags" required />
            </v-card-text>
            <v-btn text color="primary" @click="save">Salvar alterações</v-btn>
            <v-btn color="error" @click="logout">Log out</v-btn>
        </v-card>
    </v-container>
</template>
  
<script>
export default {
    name: "ComercianteContaPage",
    data() {
        return {
            tempoMedioEntregaEnum: this.$api.Enums.TempoMedioEntrega,
            formData: {
                nome: "",
                documentoFederal: "",
                email: "",
                identificador: "",
                telefoneCelular: "",
                imageUrl: "",
                tempoEntrega: "",
                valorMinimoPedido: "",
                tags: null,
            },
        };
    },
    methods: {
        onInput(selected) {
            if (selected.length > 3) {
                this.formData.tags = selected.slice(0, 3);
            }
        },
        initPage() {
            this.$api.Usuario.Get(this.$store.getters.user.codigoUsuario).then((response) => {
                this.formData = { ...response };
                this.formData.tags = JSON.parse(response.tags)
            });
        },
        save() {
            const dto = { ...this.formData };
            let codigoUsuario = this.$store.getters.user.codigoUsuario;
            dto.codigo = codigoUsuario;
            dto.tags = JSON.stringify(dto.tags)

            this.$api.Usuario.PutComerciante(codigoUsuario, dto).then((data) => {
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