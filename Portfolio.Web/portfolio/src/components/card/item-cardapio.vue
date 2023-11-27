<template>
    <v-container>
        <v-row>
            <v-col cols="12">
                <v-card>
                    <span v-if="offer" class="card__featured">{{ discountPercentage }}% OFF</span>
                    <v-row>
                        <v-col cols="6" class="d-flex">
                            <v-img class="align-items:center; justify-content: center; vertical-align: center;" :src="image"
                                height="auto" width="auto"></v-img>
                        </v-col>
                        <v-col cols="6">
                            <v-card-title>{{ name }}</v-card-title>
                            <v-card-subtitle>{{ description }}</v-card-subtitle>
                            <v-card-text>
                                <div>Preço: <u>{{ this.$api.UI.Format.Currency(totalPrice) }}</u></div>
                                <v-text-field label="Comentário adicional" v-model="comment"></v-text-field>
                                <v-btn icon @click="decreaseQuantity">
                                    <v-icon>mdi-minus</v-icon>
                                </v-btn>
                                {{ quantity }}
                                <v-btn icon @click="increaseQuantity">
                                    <v-icon>mdi-plus</v-icon>
                                </v-btn>
                                <v-btn class="align-right" color="primary" @click="addToCart">Adicionar ao carrinho</v-btn>
                            </v-card-text>
                        </v-col>
                    </v-row>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
    
<script>
export default {
    name: "ItemCardapioCard",
    props: {
        codigo: {
            type: String,
            default: ""
        },
        codigoComerciante: {
            type: String,
            default: ""
        },
        image: {
            type: String,
            default: 'https://i.pinimg.com/originals/ae/8a/c2/ae8ac2fa217d23aadcc913989fcc34a2.png'
        },
        name: {
            type: String,
            default: "",
        },
        description: {
            type: String,
            default: "",
        },
        basePrice: {
            type: Number,
        },
        offerPrice: {
            type: Number,
        },
        offer: {
            type: Boolean,
            default: false,
        },
    },
    data() {
        return {
            comment: '',
            quantity: 1,
        };
    },
    computed: {
        totalPrice() {
            if (this.offer)
                return this.offerPrice * this.quantity;
            else
                return this.basePrice * this.quantity;
        },
        discountPercentage() {
            return ((this.basePrice - this.offerPrice) / this.basePrice) * 100;
        },
    },
    methods: {
        increaseQuantity() {
            this.quantity++;
        },
        decreaseQuantity() {
            if (this.quantity > 1) {
                this.quantity--;
            }
        },
        addToCart() {
            console.log("Adicionado ao carrinho:", this.codigoComerciante, this.codigo, this.quantity, this.comment);
        },
    },
};
</script>