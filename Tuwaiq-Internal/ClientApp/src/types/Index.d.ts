import { Alpine as AlpineType, AlpineComponent } from 'alpinejs';

declare global {
  interface Window {
    Alpine: AlpineType;
  }
}

