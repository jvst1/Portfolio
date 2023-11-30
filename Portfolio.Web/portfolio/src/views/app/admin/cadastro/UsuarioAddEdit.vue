<template>
  <div>
    <modal-form v-model="show" :submit="submit">
      <template v-slot:title>
        {{ model.codigo ? "Editar" : "Novo" }} Usuario
      </template>
      <template v-slot:body>
        <v-row>
          <v-col cols="12">
            <input-text label="Nome" v-model="model.nome" required/>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-documento label="Documento Federal" v-model="model.documentoFederal" required/>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-email label="Email" v-model="model.email" required :disabled="blockEdit" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-text label="Identificador" v-model="model.identificador" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-phone label="Telefone Celular" v-model="model.telefoneCelular" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12">
            <input-phone label="Foto de perfil" v-model="model.imageUrl" />
          </v-col>
        </v-row>
        <v-row>
          <v-col xs="12" cols="12">
            <input-multiselect label="Tipo perfil" v-model="model.tipoPerfil" :options="arrayPerfil" :text="'name'" />
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
      perfil: [],
      show: false,
      model: {},
      arrayPerfil: [],
      title: '',
      blockEdit: false
    }
  },
  props: {
    refresh: Function
  },
  methods: {
    openModal(sender) {
      this.show = true
      if (sender) {
        this.model = sender
        this.model.telefoneCelular = this.$api.UI.Format.Telefone(sender.telefoneCelular)
        this.blockEdit = true
        this.model.tipoPerfil = this.retornaArrayPerfil(sender.tipoPerfil)
      } else {
        this.model = {}
        this.model.tipoPerfil = []
        this.blockEdit = false
      }
      sender = {}
    },

    retornaArrayPerfil(perfil) {
      const p = []
      for (var t in this.$api.Enums.TipoPerfilUsuario) {
        if ((perfil && this.$api.Enums.TipoPerfilUsuario[t]) > 0) {
          perfil = perfil - this.$api.Enums.TipoPerfilUsuario[t]
          p.push(this.arrayPerfil.find(o => o.value === this.$api.Enums.TipoPerfilUsuario[t]).value)
        }
      }
      return p
    },
    submit() {
      const th = this
      th.model.tipoPerfil = th.model.tipoPerfil.reduce((a, b) => a + b, 0)
      th.$api.Usuario.Persist(th.model).then(function (response) {
        var mensagem = null
        if (!th.model.codigo) { mensagem = 'Operação Realizada com Sucesso!<br/>E-mail para cadastro de senha enviado para: ' + th.model.email } else { mensagem = `Usuário ${th.model.identificador} alterado com sucesso.` }
        if (th.$api.UI.PostAction(response, mensagem)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    }
  },
  beforeMount() {
    this.$api.Enum.GetAll().then((response) => {
      this.arrayPerfil = response.TipoPerfilUsuario;
    });
  },
}
</script>
