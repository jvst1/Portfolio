<template>
  <v-form enctype="multipart/form-data" ref="form" @submit.prevent="submitDefault" :lazy-validation="lazy">
    <slot></slot>
  </v-form>
</template>

<script>
export default {
  props: {
    submit: Function,
    lazy: { type: Boolean, default: false },
  },
  methods: {
    submitDefault: function (e) {
      e.preventDefault();

      if (this.$api.UI.FormValidate(this)) {
        if (this.submit) {
          this.submit();
        }
      }
    },
    reset() {
      this.$refs.form.reset();
    },
  },
};
</script>
