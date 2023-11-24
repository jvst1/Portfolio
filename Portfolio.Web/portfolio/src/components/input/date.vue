<template>
  <div>
    <v-menu v-bind="$attrs" v-model="menu" :close-on-content-click="false" transition="scale-transition" offset-y
      full-width max-width="290px" min-width="290px">
      <template v-slot:activator="{ on, attrs }">
        <v-text-field ref="campo" v-bind="attrs" v-model="model" :label="label" prepend-inner-icon="event" :rules="rules"
          :required="required" :placeholder="placeholderDefault" :outlined="outlinedDefault" :dense="denseDefault"
          v-on="on">
          <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
            <slot :name="name" v-bind="slotData" />
          </template>
          <template v-for="(_, name) in $slots" v-slot:[name]>
            <slot :name="name" />
          </template>
        </v-text-field>
      </template>
      <v-date-picker v-bind="$attrs" no-title :max="max" ref="picker" locale="pt-br" v-model="date"
        @input="menu = false"></v-date-picker>
    </v-menu>
  </div>
</template>
<script>
export default {
  props: {
    value: null,
    label: String,
    format: { default: 'dd/MM/yyyy' },
    max: { default: new Date().toISOString().substr(0, 10) },
    tipo: { default: 'diario' },
    required: { type: Boolean, default: false }
  },
  data: () => ({
    menu: false
  }),
  computed: {
    placeholderDefault: function () {
      return this.$attrs.placeholder
    },
    outlinedDefault: function () {
      return this.$attrs.outlined
    },
    denseDefault: function () {
      return this.$attrs.dense
    },
    model: {
      get() {
        if (this.value) {
          var val = this.formatDate(
            new Date(Date.parse(this.value)).toISOString().substr(0, 10)
          )
          return val
        }
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      }
    },
    date: {
      get() {
        if (this.value) {
          var val = new Date(Date.parse(this.value))
            .toISOString()
            .substr(0, 10)
          return val
        }
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      }
    },
    rules() {
      const errors = []
      if (this.required === '' || this.required === true) {
        !!this.model || errors.push(this.label + ' é obrigatório')
      }

      if (this.$refs && this.$refs.campo && this.$refs.campo.$el) {
        this.$refs.campo.$el.setAttribute(
          'data-message',
          errors ? errors[0] : ''
        )
        this.$refs.campo.$el.setAttribute('data-invalid', errors.length > 0)
      }

      return errors
    }
  },
  methods: {
    formatDate(date) {
      if (!date) return null

      const [year, month, day] = date.split('-')
      return `${day}/${month}/${year}`
    },
    validar() { }
  },
  watch: {
    menu(val) {
      switch (this.tipo) {
        case 'anual':
          val && setTimeout(() => (this.$refs.picker.activePicker = 'YEAR'))
          break
        default:
          break
      }
    },
    date(val) {
      this.$emit('input', val)
    }
  },
  mounted() {
    this.$nextTick(function () {
      const errors = this.rules
      this.$refs.campo.$el.setAttribute(
        'data-message',
        errors ? errors[0] : ''
      )
      this.$refs.campo.$el.setAttribute('data-invalid', errors.length > 0)
    })
  }
}
</script>
