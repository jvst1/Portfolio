<template>
  <div>
    <modal-form v-model="show" :submit="submit" size="lg">
      <template v-slot:title>
        {{ endereco.codigo ? "Editar" : "Novo" }} Endereço
      </template>
      <template v-slot:body>
        <v-row class="mt-2">
          <v-col class="py-0" cols="3">
            <input-cep auto-focus v-model="endereco.cep" label="CEP" append-icon="fas fa-search-location"
              :model-names="cepModelNames" :model-endereco="endereco" required />
          </v-col>
          <v-col class="py-0" cols="6">
            <input-text v-model="endereco.logradouro" v-uppercase label="Logradouro" max-length="100" :disabled="true" required />
          </v-col>
          <v-col class="py-0" cols="3">
            <input-text v-model="endereco.nroLogradouro" label="Número" max-length="15" required />
          </v-col>
          <v-col class="py-0" cols="6">
            <input-text v-model="endereco.bairro" v-uppercase label="Bairro" max-length="50" :disabled="true" required />
          </v-col>
          <v-col class="py-0" cols="6">
            <input-text v-model="endereco.complemento" v-uppercase label="Complemento" max-length="50" />
          </v-col>
          <v-col class="py-0" cols="5">
            <input-text v-model="endereco.cidade" v-uppercase label="Cidade" max-length="40" :disabled="true" required />
          </v-col>
          <v-col class="py-0" cols="3">
            <input-select v-model="endereco.uf" label="Estado" :options="estados" :disabled="true" required />
          </v-col>
          <v-col class="py-0" cols="4">
            <input-date v-model="endereco.enderecoDesde" label="Endereço desde?" required />
          </v-col>
          <v-col class="d-flex flex-row align-center mt-0 py-0">
            <div class="mr-10">
              <label for="enderecoPrincipal">Endereço Principal?</label>
              <v-switch id="enderecoPrincipal" v-model="endereco.enderecoPrincipal" class="my-0 py-0" />
            </div>
          </v-col>
        </v-row>
      </template>
    </modal-form>
  </div>
</template>
  
<script>
export default {
  name: 'AddressAddEdit',
  data() {
    return {
      show: false,
      estados: this.$api.Enums.UFS,
      cepModelNames: {
        logradouro: 'logradouro',
        bairro: 'bairro',
        uf: 'uf',
        localidade: 'cidade'
      },
      endereco: {},
    };
  },
  methods: {
    openModal(sender) {
      this.show = true;
      this.endereco = {};
      if (sender) {
        this.endereco = { ...sender };
      }
    },
    submit() {
      const dto = { ...this.endereco };
      dto.codigoUsuario = this.$store.getters.user.codigoUsuario;
      
      this.$api.UsuarioEndereco.Persist(dto).then((response) => {
        if (this.$api.UI.PostAction(response)) {
          this.endereco = {};
          this.show = false;
          this.$emit("close", !response.error);
        }
      });
    },
  },
};
</script>
  