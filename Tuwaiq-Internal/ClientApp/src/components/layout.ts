import { AlpineComponent } from 'alpinejs';

export type ILayoutBase ={
    route: {
        url: string;
        name: string;
    };
    isLoading: boolean;
    getRouteUrl: () => string;
    getRouteName: () => string;
    openUser: boolean;
    setCurrentRoute: (url: string, name: string) => void;

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
    getRouteUrl() {
        return this.route.url;
    },
    getRouteName() {
        return this.route.name;
    },
    setCurrentRoute(url: string, name: string) {
        this.route.url = url;
        this.route.name = name;
    },
};
export default () => component;