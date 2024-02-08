import { AlpineComponent } from 'alpinejs';
import {myAxios} from "../config/axiosConfig";
import {api} from '../utils/endpoints';

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
    uploadFile: (files: File[]) => Promise<any>;

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
    async uploadFile(files: File[]) {
    try {
        const fd: FormData = new FormData();
        Array.from(files).forEach((file) => {
            fd.append("files", file);
        });

        const result = await myAxios.post(api.FileUploadUrl, fd, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

        return result;
    } catch (error: any) {
        throw new Error(error);
    }
}
};
export default () => component;