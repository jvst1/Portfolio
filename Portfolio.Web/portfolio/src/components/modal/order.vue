<template>
    <div>
        <modal-form v-model="show" textClose="Fechar" :size="size" ref="order" :textSubmit="textCheckout" :submit="submit">
            <template v-slot:title>
                {{ title }}
                <br />
                <span class="caption pt-1 ml-3">{{ caption }}</span>
            </template>
            <template v-slot:body>
                <card-item-cardapio v-for="(product) in items" :key="`productCard__${product.codigo}`"
                    :codigo="product.codigo" :codigoComerciante="codigoComerciante" :image="product.imageUrl"
                    :name="product.nome" :description="product.descricao" :basePrice="product.valorProduto"
                    :offerPrice="product.valorDescontoFixo" :offer="product.aplicarDesconto" />
            </template>
        </modal-form>
        <modal-cart ref="carrinho"></modal-cart>
    </div>
</template>

<script>
export default {
    props: {
        codigoComerciante: { type: String },
        title: { type: String, default: "Faça um pedido!" },
        caption: { type: String, default: "" },
        items: { type: Array },
        size: { default: "md" },
    },
    data() {
        return {
            show: false,
            products: [],
        };
    },
    computed: {
        textCheckout() {
            return `Finalizar pedido (${this.$store.getters.carrinho.count})`;
        }
    },
    methods: {
        openModal() {
            this.show = true;
        },
        submit() {
            this.$refs.carrinho.openModal();
        },
    },
};
</script>
