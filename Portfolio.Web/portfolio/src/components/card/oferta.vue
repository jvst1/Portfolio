<template>
    <div :class="`deal-card deal-card__bg-${variant}`">
        <div class="deal-card__image">
            <img :src="image" alt="cardimage" />
        </div>

        <div class="deal-card__content">
            <div class="deal-card__title">{{ title }}</div>
            <div class="deal-card__offer">{{ offer }}</div>
            <div class="deal-card__tag">{{ tag }}</div>
        </div>
    </div>
</template>
  
<script>
export default {
    name: "DealCard",
    props: {
        variant: {
            type: String,
            default: "primary",
        },
        title: {
            type: String,
            default: "",
        },
        offer: {
            type: String,
            default: "",
        },
        tag: {
            type: String,
            default: "",
        },
        image: {
            type: String,
            default: 'https://i.pinimg.com/originals/ae/8a/c2/ae8ac2fa217d23aadcc913989fcc34a2.png'
        },
    },
};
</script>
  
<style lang="scss">
.deal-card {
    display: flex;
    justify-content: center;
    align-items: center;
    background: darkgrey;
    border-radius: 16px;
    padding-right: 24px;

    &__bg-primary {
        background-color: var(--primary-light);
    }

    &__bg-secondary {
        background-color: var(--secondary-light);
    }

    &__offer {
        color: var(--primary);
    }

    &__bg-primary {
        color: var(--primary);
    }

    &__bg-secondary {
        color: var(--secondary);
    }

    &__offer {
        color: var(--secondary);
    }

    &__image {
        img {
            width: 100%;
            background-size: cover;
        }
    }

    &__title {
        font-style: normal;
        font-weight: 600;
        font-size: 20px;
        line-height: 28px;
        letter-spacing: 0.1px;
        color: #2b2b43;
    }

    &__offer {
        font-weight: 800;
        font-size: 40px;
        line-height: 56px;
        letter-spacing: 0.1px;
        padding-bottom: 36px;
    }

    &__tag {
        font-weight: 400;
        font-size: 15px;
        line-height: 20px;
        letter-spacing: 0.1px;
        color:  var(--grey);
    }
}




@media screen and (max-width: 768px) {
    .deal-card {
        flex-direction: column-reverse;

        &__content {
            position: relative;
            display: flex;
            justify-content: space-between;
            flex-direction: row;
            width: 100%;
            padding: 20px;
        }

        &__title {
            padding-bottom: 60px;
        }

        &__tag {
            padding: 2px 0 0 132px;
        }

        &__offer {
            position: absolute;
            top: 52px;
            left: 16px;
        }
    }
}
</style>