/** @type {import('tailwindcss').Config} */
export default {
	content: [
		"../Pages/**/*.{cshtml,razor}",
		"./src/components/**/*.{js,ts,jsx,tsx}",
	],


	theme: {
		extend: {
			transitionProperty: {
				height: 'height'
			},
			colors: {
				primary: "#6A5C9F",
				secondary: "#72CACE",
				denim: {
					50: '#f2f8fd',
					100: '#e3f0fb',
					200: '#c1e1f6',
					300: '#8ac8ef',
					400: '#4cace4',
					500: '#2493d3',
					600: '#1779ba',
					700: '#135d91',
					800: '#144f78',
					900: '#164264',
					950: '#0f2b42'
				},
				transparent: 'transparent',
				current: 'currentColor',
				white: '#FFFFFF',
				black: '#000000',
				
				'light-purple': {
					DEFAULT: '#ABADF5',
					dark: '#858AEC'
				},
				'dark-gray': {
					DEFAULT: '#4D4D4D',
					dark: '#333333'
				},
				blue: {
					50: '#eff5fe',
					100: '#e2ebfd',
					200: '#cbdafa',
					300: '#abc2f6',
					400: '#8aa0ef',
					500: '#5e72e4',
					600: '#525cd9',
					700: '#434abf',
					800: '#383f9b',
					900: '#343a7b'
				},
				slate: {
					50: '#f6f7f9',
					100: '#eceef2',
					200: '#d5d9e2',
					300: '#b1bac8',
					400: '#8694aa',
					500: '#66758e',
					600: '#525e77',
					700: '#434d61',
					800: '#3a4252',
					900: '#343946'
				},
				gray: {
					25: '#FCFCFD',
					50: '#F9FAFB',
					100: '#F2F4F7',
					200: '#EAECF0',
					300: '#D0D5DD',
					400: '#98A2B3',
					500: '#667085',
					600: '#475467',
					700: '#344054',
					800: '#1D2939',
					900: '#101828'
				},
				
				success: {
					25: '#EBF5F2',
					50: '#D7EAE6',
					100: '#C3E0D9',
					200: '#9BCBC0',
					300: '#73B6A6',
					400: '#5FAC99',
					500: '#379780',
					600: '#328873',
					700: '#2C7966',
					800: '#276A5A',
					900: '#1C4C40'
				},
				warning: '#4E3797',
				danger: '#E15652',
				'light-gray': {
					25: '#FCFCFD',
					300: '#D0D5DD',
					400: '#98A2B3',
					600: '#475467',
					700: '#344054',
					800: '#1D2939'
				}
			},
			fontFamily: {
				sans: ['"Next"', 'sans-serif']
			},
			boxShadow: {
				shadow: '0px 4px 30px -1px rgba(226, 228, 231, 0.4)'
			}
		}
	},
	plugins: [
		require('flowbite/plugin'),
		require('@tailwindcss/forms'),
		function ({ addBase, theme }) {
			addBase({
				h1: { fontSize: theme('fontSize.6xl') },
				h2: { fontSize: theme('fontSize.3xl') },
				h3: { fontSize: theme('fontSize.xl') },
				h4: { fontSize: theme('fontSize.lg') }
			});
		}
	]
};
