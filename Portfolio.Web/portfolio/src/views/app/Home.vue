<template>
  <section>
    <div class="container">
      <div class="deals__list">
        <card-oferta v-for="(dealCard, index) in dealList" :key="`dealCard__${index}`" :variant="dealCard.variant"
          :title="dealCard.nome" :offer="dealCard.valorDescontoPercentual" :image="dealCard.imageUrl" />
      </div>

      <div class="category__list center" v-if="!isLoading">
        <card-categoria v-for="(categoryCard, index) in categoryList" :key="`categoryCard__${index}`"
          :title="categoryCard.nome" :image="categoryCard.imageUrl" :selected="categoryCard.codigo === selectedCategory"
          @onClick="() => clickCategory(categoryCard)" />
      </div>

      <div class="products__list" v-if="!isLoading">
        <card-estabelecimento-comercial v-for="(product) in productList" :key="`productCard__${product.codigo}`"
          :image="product.imageUrl" :featured="product.featured" :title="product.nome" :minTime="product.tempoEntrega"
          :minSum="product.valorMinimoPedido" :tags="product.tags" :codigo="product.codigo" />
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

      this.$api.Usuario.GetAllComerciantes().then((response) => {
        this.productList = [];
        response.map(item => {
          item.tags = JSON.parse(item.tags);
          this.productList.push(item);
        })
      })


      this.$api.Cardapio.GetOfertas().then((response) => {
        this.dealList = [];
        response.map(item => {
          console.log(item)
          this.dealList.push(item);
        })
      })
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