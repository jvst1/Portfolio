<template>
  <v-tooltip top>
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" :class="btnClass" fab small :color="color" @click="$emit('click', $event)">
        <v-icon
          :small="($attrs.small === true || $attrs.small === '') || $vuetify.breakpoint[$vuetify.theme.options.compact.gridButton]">{{
            icon }}</v-icon>
        <slot></slot>
      </v-btn>
    </template>
    <span>{{ text }}</span>
  </v-tooltip>
</template>

<script>
export default {
  props: {
    color: { type: String, default: 'grid-btn' },
    icon: { type: String },
    text: { type: String, default: '' }
  },
  computed: {
    btnClass: function () {
      return 'mr-2 ' + (this.$attrs['btn-class'] ? this.$attrs['btn-class'] : ((this.$attrs.small === true || this.$attrs.small === '') || this.$vuetify.breakpoint[this.$vuetify.theme.options.compact.gridButton] ? 'pa-2' : 'pa-6'))
    }
  }
}
</script>
