import { Alpine as AlpineType, AlpineComponent } from 'alpinejs';

declare global {
  interface Window {
    Alpine: AlpineType;
    token: string | null | undefined;
    base_url: string;
  }
}

