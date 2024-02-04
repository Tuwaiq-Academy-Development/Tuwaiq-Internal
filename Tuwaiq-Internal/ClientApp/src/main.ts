// @ts-nocheck
import './style.css'
import Alpine from 'alpinejs'
import layout from './components/layout'
import indexData from './components/index'
import CandidateViewData from './components/candidate'
import 'flowbite';
import companies from './components/companies'
import categories from './components/categories'
window.Alpine = Alpine

Alpine.data('layoutData', layout);
Alpine.data('indexData', indexData);

Alpine.data('CandidateViewData', CandidateViewData);
Alpine.data('CompanyIndexData', companies);
Alpine.data('CategoryIndexData', categories);
Alpine.start();