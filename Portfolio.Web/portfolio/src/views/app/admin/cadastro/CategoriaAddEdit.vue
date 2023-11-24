<template>
  <div>
    <modal-form v-model="show" :submit="submit">
      <template v-slot:title>
        {{ model.codigo ? "Editar" : "Novo" }} Categoria
      </template>
      <template v-slot:body>
        <v-row>
          <v-col cols="12">
            <input-text label="Nome" v-model="model.nome" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-text label="URL Imagem" v-model="model.imageUrl" />
          </v-col>
        </v-row>
      </template>
    </modal-form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      show: false,
      model: {},
    }
  },
  methods: {
    openModal(sender) {
      this.show = true
      if (sender)
        this.model = { ...sender };
      else
        this.model = {}
    },
    submit() {
      this.$api.Categoria.Persist(this.model).then((response) => {
        let mensagem = null;
        if (!this.model.codigo)
          mensagem = `Categoria ${this.model.nome} criada com sucesso.`
        else
          mensagem = `Categoria ${this.model.nome} alterado com sucesso.`

        if (this.$api.UI.PostAction(response, mensagem)) {
          this.show = false
          this.$emit('close', !response.error)
        }
      })
    }
  },
}
</script>
