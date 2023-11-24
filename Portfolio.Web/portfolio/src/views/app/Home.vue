<template>
  <section>
    <div class="container">
      <div class="deals__list">
        <card-oferta v-for="(dealCard, index) in dealList" :key="`dealCard__${index}`" :variant="dealCard.variant"
          :title="dealCard.title" :offer="dealCard.offer" :tag="dealCard.tag" :image="dealCard.image" />
      </div>

      <div class="category__list center" v-if="!isLoading">
        <card-categoria v-for="(categoryCard, index) in categoryList" :key="`categoryCard__${index}`"
          :title="categoryCard.nome" :image="categoryCard.imageUrl" :selected="categoryCard.codigo === selectedCategory"
          @onClick="() => clickCategory(categoryCard)" />
      </div>

      <div class="products__list" v-if="!isLoading">
        <card-estabelecimento-comercial v-for="(product) in productList" :key="`productCard__${product.codigo}`"
          :image="product.image" :featured="product.featured" :title="product.name" :minTime="product.deliveryTime"
          :minSum="product.minimalOrder" :tags="product.tags" />
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: "home",
  data() {
    return {
      model: {},
      isLoading: false,
      categoryList: [],
      dealList: [],
      productList: [],
      selectedCategory: null
    };
  },
  methods: {
    loadData() {
      this.isLoading = true;

      this.$api.Categoria.GetAll().then((response) => {
        this.categoryList = [...response];
      })
      this.productList = [
        {
          categories:
            [
              {
                id: "353772735829115091",
                Image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/v1673642603/sushi_nfwx8a.svg",
                name: "Sushi"
              }
            ],
          deliveryTime: "30-40 min",
          id: "352601598882480219",
          image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/c_pad,b_auto:predominant,fl_preserve_transparency/v1672525249/food-delivery-app/1_huvxq7.jpg",
          minimalOrder: "$32 min sum",
          name: "Royal Sushi House",
          tags: ["Sushi", "VIP", "easy"],
          featured: 'Recomendado'
        },
        {
          categories:
            [
              {
                id: "353772735829115092",
                Image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/v1673642603/sushi_nfwx8a.svg",
                name: "Sushi"
              }
            ],
          deliveryTime: "30-40 min",
          id: "352601598882444339",
          image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/c_pad,b_auto:predominant,fl_preserve_transparency/v1672525249/food-delivery-app/1_huvxq7.jpg",
          minimalOrder: "$32 min sum",
          name: "Royal Sushi House",
          tags: ["Sushi", "VIP", "easy"],
        },
        {
          categories:
            [
              {
                id: "35377273582911321",
                Image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/v1673642603/sushi_nfwx8a.svg",
                name: "Sushi"
              }
            ],
          deliveryTime: "30-40 min",
          id: "352601598882480339",
          image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/c_pad,b_auto:predominant,fl_preserve_transparency/v1672525249/food-delivery-app/1_huvxq7.jpg",
          minimalOrder: "$32 min sum",
          name: "Royal Sushi House",
          tags: ["Sushi", "123", "xzc"],
        },
        {
          categories:
            [
              {
                id: "3537727358291151091",
                Image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/v1673642603/sushi_nfwx8a.svg",
                name: "Sushi"
              }
            ],
          deliveryTime: "30-40 min",
          id: "35260159888248035",
          image: "https://res.cloudinary.com/dn5rtwwcx/image/upload/c_pad,b_auto:predominant,fl_preserve_transparency/v1672525249/food-delivery-app/1_huvxq7.jpg",
          minimalOrder: "$32 min sum",
          name: "Royal Sushi House",
          tags: ["Peixe", "NNN", "VVV"],
        },
      ]

      this.dealList = [
        {
          title: "teste",
          offer: "40% OFF",
          tag: "tag123",
          image: "https://food-delivery-app-front.netlify.app/img/deal_icecream.ea4a65e9.png"
        },
        {
          title: "teste",
          offer: "40% OFF",
          tag: "tag123",
          image: "https://food-delivery-app-front.netlify.app/img/deal_icecream.ea4a65e9.png"
        },
      ];
      this.isLoading = false
    },
    clickCategory(categoryObject) {
      const { id } = categoryObject;

      if (this.selectedCategory === id) {
        this.selectedCategory = null
      } else {
        this.selectedCategory = id
      }
    }
  },
  mounted() {
    this.loadData();
  }
};
</script>

<style lang="scss">
.deals {
  &__list {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 30px;
    margin-bottom: 32px;
  }
}

.category {
  &__list {
    display: grid;
    grid-template-columns: repeat(6, 1fr);
    grid-gap: 30px;
    margin-bottom: 40px;
  }
}

.products {
  &__list {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-gap: 30px;
    margin-bottom: 100px;
  }
}

@media screen and (max-width: 768px) {
  .deals {
    &__list {
      grid-template-columns: 1fr;
    }
  }

  .category {
    &__list {
      grid-template-columns: repeat(3, 1fr);
    }
  }

  .products {
    &__list {
      grid-template-columns: 1fr;
    }
  }
}
</style>