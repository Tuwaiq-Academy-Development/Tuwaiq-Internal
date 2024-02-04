/** @type {import('tailwindcss').Config} */
export default {
	content: [
		"../Pages/**/*.{cshtml,razor}",
		"./src/components/**/*.{js,ts,jsx,tsx}",
	],

	theme: {
		extend: {
			fontFamily: {
				sans: ["Next", "Helvetica", "Arial", "sans-serif"],
			},
			colors: {
				primary: "#6A5C9F",
				secondary: "#72CACE",
			},
		},
	},
	plugins: [],
};
