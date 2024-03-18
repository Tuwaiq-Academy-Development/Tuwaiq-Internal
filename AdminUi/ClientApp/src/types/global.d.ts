import { Alpine as AlpineType } from 'alpinejs'

declare global {
    let Alpine: AlpineType
    let token: string | null | undefined;
}
