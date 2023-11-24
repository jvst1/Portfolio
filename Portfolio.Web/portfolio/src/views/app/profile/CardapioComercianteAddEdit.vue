<template>
    <modal-form v-model="show" :submit="submit" size="md">
        <template v-slot:title>
            {{ formData.codigo ? "Editar item do cardápio" : "Novo item para o cardápio" }}
        </template>
        <template v-slot:body>
            <v-card flat>
                <v-card-text>
                    Detalhes do produto
                    <input-text label="Nome do produto" placeholder="Informe o nome do produto" v-model="formData.nome"
                        :maxlength="255" required />
                    <input-text label="Descrição do produto" placeholder="Informe a descrição do produto"
                        v-model="formData.descricao" :maxlength="255" required />
                    <input-text label="URL da foto do produto" placeholder="Informe a URL da foto do produto"
                        v-model="formData.imageUrl" :maxlength="255" required />
                    <input-switch label="Produto à venda?" v-model="formData.vendaAtiva" required />

                    Preço
                    <input-text id="valorProduto" name="valorProduto" :type="'decimal'" label="Preço"
                        placeholder="Informe o preço do produto" v-model="formData.valorProduto"
                        :disabled="formData.aplicarDesconto" required />

                    <input-switch label="Aplicar desconto?" v-model="formData.aplicarDesconto" />

                    <div v-show="formData.aplicarDesconto">
                        <input-text id="valorDescontoFixo" name="valorDescontoFixo" :type="'decimal'"
                            label="Preço com desconto" placeholder="Informe o preço do produto com desconto"
                            v-model="formData.valorDescontoFixo" :required="formData.aplicarDesconto" :minimumValue="0"
                            :maximumValue="formData.valorProduto" />

                        <input-text id="valorDescontoPercentual" name="valorDescontoPercentual"
                            label="Desconto em porcentagem %" placeholder="Porcentagem do desconto"
                            v-model="formData.valorDescontoPercentual" :minimumValue="0" :maximumValue="100"
                            :required="formData.aplicarDesconto" />
                    </div>
                </v-card-text>
            </v-card>
        </template>
    </modal-form>
</template>
  
<script>
export default {
    name: "AccountPage",
    data() {
        return {
            show: false,
            formData: {
                nome: "",
                descricao: "",
                imageUrl: "",
                vendaAtiva: false,
                valorProduto: null,
                aplicarDesconto: false,
                valorDescontoFixo: null,
                valorDescontoPercentual: null,
            },
        };
    },
    watch: {
        'formData.valorDescontoFixo'(newVal, oldVal) {
            if (this.formData.valorProduto && newVal !== oldVal) {
                const desconto = this.formData.valorProduto - newVal;
                this.formData.valorDescontoPercentual = (desconto / this.formData.valorProduto) * 100;

                if (this.formData.valorDescontoPercentual <= 0)
                    this.formData.valorDescontoPercentual = 0;
            }
        },
        'formData.valorDescontoPercentual'(newVal, oldVal) {
            if (this.formData.valorProduto && newVal !== oldVal) {
                const desconto = (this.formData.valorProduto * newVal) / 100;
                this.formData.valorDescontoFixo = this.formData.valorProduto - desconto;

                if (this.formData.valorDescontoFixo <= 0)
                    this.formData.valorDescontoFixo = 0;

                if (this.formData.valorDescontoFixo >= this.formData.valorProduto)
                    this.formData.valorDescontoFixo = this.formData.valorProduto;
            }
        },
    },
    methods: {
        openModal(sender) {
            if (sender) {
                this.formData = { ...sender };
            }
            this.show = true;
        },
        submit() {
            const dto = { ...this.formData };
            dto.codigoUsuario = this.$store.getters.user.codigoUsuario;

            this.$api.CardapioComerciante.Persist(dto).then((response) => {
                if (this.$api.UI.PostAction(response)) {
                    this.formData = {};
                    this.show = false;
                    this.$emit("close", !response.error);
                }
            });
        },
    },
};
</script>