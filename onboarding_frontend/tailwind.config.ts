import type { Config } from "tailwindcss";

export default {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    //tatt fra PowerOffice Typeography i Figma
    fontFamily: {
      sans: ["Body", "sans-serif"],
    },
    extend: {
      colors: {
        background: "var(--background)",
        foreground: "var(--foreground)",
      },
      fontFamily: {
        widgetHuge: ["WidgetHuge", "sans-serif"],
        widgetExtraLarge: ["WidgetExtraLarge", "sans-serif"],
        widgetLarge: ["WidgetLarge", "sans-serif"],
        h1: ["H1", "sans-serif"],
        h2: ["H2", "sans-serif"],
        h3: ["H3", "sans-serif"],
        h4: ["H4", "sans-serif"],
        h5: ["H5", "sans-serif"],
        h6: ["H6", "sans-serif"],
        body: ["Body", "sans-serif"],
        placeholder: ["Placeholder", "sans-serif"],
        placeholderStrong: ["PlaceholderStrong", "sans-serif"],
        link: ["Link", "sans-serif"],
        label: ["Label", "sans-serif"],
        labelStrong: ["LabelStrong", "sans-serif"],
      },
    },
  },
  plugins: [],
} satisfies Config;