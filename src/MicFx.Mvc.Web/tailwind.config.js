/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml',
    '../Modules/**/Views/**/*.cshtml', // untuk modul Razor OrchardCore
    './Pages/**/*.cshtml', // jika pakai Razor Pages
    './wwwroot/**/*.js' // jika ada js pakai class tailwind
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};

