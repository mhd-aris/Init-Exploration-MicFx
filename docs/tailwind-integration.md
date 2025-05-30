# TailwindCSS Integration

This guide explains how TailwindCSS is integrated into the MicFx Orchard Core starter template and how to work with it effectively.

## 🎨 Overview

TailwindCSS is integrated as a utility-first CSS framework that works seamlessly with Orchard Core modules. The configuration allows you to use Tailwind classes across:

- Main application views
- Module views
- Razor pages
- JavaScript files

## 📁 File Structure

```
src/MicFx.Mvc.Web/
├── Assets/
│   └── css/
│       └── input.css              # Tailwind input file
├── wwwroot/
│   └── css/
│       └── site.css               # Generated output file
├── node_modules/                  # npm dependencies
├── package.json                   # npm configuration
├── tailwind.config.js            # Tailwind configuration
└── postcss.config.js             # PostCSS configuration
```

## ⚙️ Configuration Files

### package.json

The npm configuration includes TailwindCSS and related dependencies:

```json
{
  "name": "micfx.mvc.web",
  "version": "1.0.0",
  "scripts": {
    "tailwind:watch": "tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css --watch"
  },
  "devDependencies": {
    "autoprefixer": "^10.4.21",
    "postcss": "^8.5.4",
    "tailwindcss": "^3.4.17"
  }
}
```

### tailwind.config.js

The Tailwind configuration specifies content sources for CSS purging:

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml',                    // Main application views
    '../Modules/**/Views/**/*.cshtml',        // All module views
    './Pages/**/*.cshtml',                    // Razor pages
    './wwwroot/**/*.js'                       // JavaScript files
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
```

### postcss.config.js

PostCSS configuration for processing:

```javascript
module.exports = {
  plugins: {
    tailwindcss: {},
    autoprefixer: {},
  }
}
```

### Assets/css/input.css

The source CSS file with Tailwind directives:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

## 🚀 Development Workflow

### 1. Initial Setup

Install npm dependencies:

```bash
cd src/MicFx.Mvc.Web
npm install
```

### 2. Watch Mode for Development

Start TailwindCSS in watch mode for automatic compilation:

```bash
npm run tailwind:watch
```

This command:
- Watches for changes in all configured content files
- Automatically recompiles CSS when changes are detected
- Outputs the result to `wwwroot/css/site.css`
- Purges unused CSS for optimal file size

### 3. Production Build

For production builds, run once without watch mode:

```bash
npx tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css --minify
```

## 🎯 Using TailwindCSS in Views

### Main Application Views

In your main application views (`src/MicFx.Mvc.Web/Views/`):

```html
<!-- Example: Views/Home/Index.cshtml -->
<div class="min-h-screen bg-gray-100">
    <header class="bg-white shadow">
        <div class="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
            <h1 class="text-3xl font-bold text-gray-900">
                Dashboard
            </h1>
        </div>
    </header>
    
    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div class="px-4 py-6 sm:px-0">
            <div class="border-4 border-dashed border-gray-200 rounded-lg h-96 p-4">
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    <!-- Content goes here -->
                </div>
            </div>
        </div>
    </main>
</div>
```

### Module Views

In module views (`src/Modules/*/Views/`):

```html
<!-- Example: Module view with TailwindCSS -->
<div class="container mx-auto px-4 py-8">
    <div class="bg-white rounded-lg shadow-lg overflow-hidden">
        <div class="px-6 py-4 bg-gradient-to-r from-blue-500 to-blue-600">
            <h2 class="text-xl font-bold text-white">Module Content</h2>
        </div>
        
        <div class="p-6">
            <p class="text-gray-700 mb-4">
                This content is styled with TailwindCSS utilities.
            </p>
            
            <div class="flex space-x-2">
                <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded transition duration-200">
                    Primary Action
                </button>
                <button class="bg-gray-300 hover:bg-gray-400 text-gray-800 font-bold py-2 px-4 rounded transition duration-200">
                    Secondary Action
                </button>
            </div>
        </div>
    </div>
</div>
```

### Layout Integration

Include the generated CSS in your layout file:

```html
<!-- _Layout.cshtml -->
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MicFx</title>
    
    <!-- TailwindCSS -->
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    @RenderBody()
    
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

## 🔧 Customization

### Extending the Theme

Add custom styles to the Tailwind configuration:

```javascript
// tailwind.config.js
module.exports = {
  content: [
    './Views/**/*.cshtml',
    '../Modules/**/Views/**/*.cshtml',
    './Pages/**/*.cshtml',
    './wwwroot/**/*.js'
  ],
  theme: {
    extend: {
      colors: {
        'micfx-primary': '#3B82F6',
        'micfx-secondary': '#6B7280',
        'micfx-accent': '#10B981',
      },
      fontFamily: {
        'micfx': ['Inter', 'sans-serif'],
      },
      spacing: {
        '18': '4.5rem',
        '88': '22rem',
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
};
```

### Adding Custom Components

Create reusable component classes:

```css
/* Assets/css/input.css */
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer components {
  .btn-primary {
    @apply bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded transition duration-200;
  }
  
  .card {
    @apply bg-white rounded-lg shadow-md overflow-hidden;
  }
  
  .card-header {
    @apply px-6 py-4 bg-gray-50 border-b border-gray-200;
  }
  
  .card-body {
    @apply p-6;
  }
}
```

### Adding Tailwind Plugins

Install and configure additional Tailwind plugins:

```bash
npm install @tailwindcss/forms @tailwindcss/typography @tailwindcss/aspect-ratio
```

```javascript
// tailwind.config.js
module.exports = {
  // ... existing config
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
    require('@tailwindcss/aspect-ratio'),
  ],
};
```

## 📱 Responsive Design

TailwindCSS provides responsive utilities out of the box:

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
    <div class="p-4 bg-white rounded-lg shadow">
        <h3 class="text-lg font-semibold mb-2">Card Title</h3>
        <p class="text-gray-600">Card content that adapts to screen size.</p>
    </div>
</div>

<!-- Responsive text sizes -->
<h1 class="text-2xl md:text-3xl lg:text-4xl xl:text-5xl font-bold">
    Responsive Heading
</h1>

<!-- Responsive spacing -->
<div class="p-4 md:p-6 lg:p-8">
    Content with responsive padding
</div>
```

## 🎨 Common UI Patterns

### Navigation

```html
<nav class="bg-white shadow-lg">
    <div class="max-w-7xl mx-auto px-4">
        <div class="flex justify-between">
            <div class="flex space-x-7">
                <div>
                    <a href="#" class="flex items-center py-4 px-2">
                        <span class="font-semibold text-gray-500 text-lg">MicFx</span>
                    </a>
                </div>
            </div>
            <div class="hidden md:flex items-center space-x-3">
                <a href="#" class="py-4 px-2 text-gray-500 hover:text-green-500 transition duration-300">Home</a>
                <a href="#" class="py-4 px-2 text-gray-500 hover:text-green-500 transition duration-300">About</a>
            </div>
        </div>
    </div>
</nav>
```

### Forms

```html
<form class="space-y-6">
    <div>
        <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
        <input type="email" id="email" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50">
    </div>
    
    <div>
        <label for="message" class="block text-sm font-medium text-gray-700">Message</label>
        <textarea id="message" rows="4" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"></textarea>
    </div>
    
    <button type="submit" class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
        Submit
    </button>
</form>
```

### Cards and Grids

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
    @for (int i = 1; i <= 6; i++)
    {
        <div class="bg-white overflow-hidden shadow rounded-lg">
            <div class="p-5">
                <div class="flex items-center">
                    <div class="flex-shrink-0">
                        <div class="h-8 w-8 bg-indigo-500 rounded-full flex items-center justify-center">
                            <span class="text-white font-bold">@i</span>
                        </div>
                    </div>
                    <div class="ml-5 w-0 flex-1">
                        <dl>
                            <dt class="text-sm font-medium text-gray-500 truncate">Card @i</dt>
                            <dd class="text-lg font-medium text-gray-900">Sample Content</dd>
                        </dl>
                    </div>
                </div>
            </div>
        </div>
    }
</div>
```

## 🔍 Troubleshooting

### CSS Not Updating

1. Ensure TailwindCSS is running in watch mode:
   ```bash
   npm run tailwind:watch
   ```

2. Check that your files are included in the `content` array in `tailwind.config.js`

3. Clear browser cache or hard refresh (Ctrl+F5)

### Styles Not Applying

1. Verify the CSS file is properly linked in your layout
2. Check for CSS specificity issues
3. Ensure the generated CSS file exists in `wwwroot/css/site.css`

### Build Issues

1. Check that Node.js and npm are properly installed
2. Run `npm install` to ensure all dependencies are installed
3. Verify file paths in configuration files

### Module Views Not Styled

1. Ensure module view paths are included in `tailwind.config.js`:
   ```javascript
   '../Modules/**/Views/**/*.cshtml'
   ```

2. Restart the TailwindCSS watch process after adding new modules

## 📚 Additional Resources

- [TailwindCSS Documentation](https://tailwindcss.com/docs)
- [TailwindCSS Components](https://tailwindui.com/components)
- [Headless UI for React/Vue](https://headlessui.dev/)
- [TailwindCSS Play](https://play.tailwindcss.com/) - Online playground 