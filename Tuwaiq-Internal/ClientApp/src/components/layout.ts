import { AlpineComponent } from 'alpinejs';

export type ILayoutBase ={
    route: {
        url: string;
        name: string;
    };
    isLoading: boolean;
    openUser: boolean;
}
export type ILayout = AlpineComponent<ILayoutBase>;

const component: ILayout = {
    route: {
        url: '',
        name: '',
    },
    isLoading: true,
    openUser: false,
    async init() {
        this.isLoading = false;
    },
};
export default () => component;