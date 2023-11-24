<template>
  <input-text v-bind="$attrs" ref="campo" v-model="model" v-mask="['##.###-###']" maxlength="20" type="cep"
    :cep-invalido="invalido" @change="changeDefault" v-on="$listeners">
    <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
      <slot :name="name" v-bind="slotData" />
    </template>
    <template v-for="(_, name) in $slots" v-slot:[name]>
      <slot :name="name" />
    </template>
  </input-text>
</template>

<script>
/* eslint-disable */
export default {
  props: {
    value: null,
    modelEndereco: { type: Object, default: null },
    modelNames: {
      type: Object,
      default: () => {
        return {
          logradouro: 'logradouro',
          bairro: 'bairro',
          uf: 'uf',
          localidade: 'localidade'
        }
      }
    }
  },
  data() {
    return {
      invalido: false
    }
  },
  methods: {
    changeDefault: function () {
      const th = this
      if (th.model && th.$api.UI.Format.SomenteNumeros(th.model).length === 8) {
        th.invalido = false
        th.$api.UI.ShowLoading()
        th.$api.CEP.buscarCep(th.$api.UI.Format.SomenteNumeros(th.model)).then(
          function (response) {
            th.$api.UI.HideLoading()
            if (response.erro) {
              th.$api.UI.ShowAlert('', 'Endereço não Localizado pelo CEP')
            } else if (th.modelEndereco) {
              th.modelEndereco[th.modelNames.logradouro] = response.logradouro
              th.modelEndereco[th.modelNames.bairro] = response.bairro
              th.modelEndereco[th.modelNames.uf] = response.uf
              th.modelEndereco[th.modelNames.localidade] = response.localidade

              th.$set(th.modelEndereco, th.modelEndereco)
            }
            if (th.blur) {
              th.blur(response)
            }
          }
        )
      } else {
        th.invalido = true
        th.$emit('blur', this.value)
      }
    }
  },
  computed: {
    model: {
      get() {
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      }
    }
  }
}
</script>
