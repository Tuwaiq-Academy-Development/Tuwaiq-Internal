// @ts-nocheck
import './style.css'
import Alpine from 'alpinejs'
import layout from './components/layout'
import indexPage from './components/index'
import CheckIdentities from './components/CheckIdentities'

import 'flowbite';

window.Alpine = Alpine

Alpine.data('layoutData', layout);
Alpine.data('indexData', indexPage);
Alpine.data('CheckIdentitiesData', CheckIdentities);

Alpine.start();