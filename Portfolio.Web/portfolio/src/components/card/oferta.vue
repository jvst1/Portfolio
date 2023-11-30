<template>
    <div :class="`deal-card deal-card__bg-${variant}`">
        <v-row no-gutters>
            <v-col cols="12" md="6">
                <v-img :src="image" class="deal-card__image" aspect-ratio="1.5">
                    <template v-slot:placeholder>
                        <v-row class="fill-height ma-0" align="center" justify="center">
                            <v-progress-circular indeterminate color="grey lighten-5"></v-progress-circular>
                        </v-row>
                    </template>
                </v-img>
            </v-col>

            <v-col cols="12" md="6">
                <v-container class="deal-card__content">
                    <v-row align="center" justify="center">
                        <v-col class="text-center">
                            <span class="deal-card__title title">{{ title }}</span>
                            <div class="deal-card__offer headline">{{ offer }}% OFF</div>
                            <div class="deal-card__tag subtitle-1">{{ tag }}</div>
                        </v-col>
                    </v-row>
                </v-container>
            </v-col>
        </v-row>
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
            type: Number,
            default: 0,
        },
        tag: {
            type: String,
            default: "Deconto Imperdível!",
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
        color: var(--grey);
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