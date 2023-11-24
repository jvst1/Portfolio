<template>
  <a class="category" :class="{ 'is-selected': selected }" @click.prevent="handleClick">
    <div class="category__img-wrapper">
      <img class="category__img" :src="image" alt="Category Image" />
    </div>
    <div class="category__name">{{ title }}</div>
  </a>
</template>

<script>
export default {
  name: "CategoryCard",
  props: {
    image: {
      required: true,
      type: String,
      default: "",
    },
    title: {
      type: String,
      default: "",
    },
    selected: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    handleClick() {
      this.$emit('onClick')
    }
  }
};
</script>
  
<style lang="scss">
.category {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  height: 80px;
  padding: 10px;
  border: 1px solid var(--grey-lightest);
  border-radius: 16px;
  background: var(--white);
  text-align: left;
  transition: all 0.3s ease;

  &:hover {
    cursor: pointer;
    border-color: var(--primary);
    background-color: var(--grey-lightest);
  }

  &.is-selected {
    border: 2px solid var(--primary);
  }

  &__img-wrapper {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    overflow: hidden;
  }

  &__img {
    width: 100%;
    height: auto;
  }

  &__name {
    font-size: 16px;
    font-weight: 700;
    color: var(--primary);
    line-height: 1.2;
  }
}
</style>
