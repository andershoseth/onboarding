import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
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
      fontSize: {
        widgetHuge: ["56px", { lineHeight: "48px" }],
        widgetExtraLarge: ["40px", { lineHeight: "48px" }],
        widgetLarge: ["28px", { lineHeight: "32px" }],
        h1: ["24px", { lineHeight: "28px" }],
        h2: ["18px", { lineHeight: "22px" }],
        h3: ["16px", { lineHeight: "20px" }],
        h4: ["16px", { lineHeight: "20px" }],
        h5: ["14px", { lineHeight: "18px" }],
        h6: ["12px", { lineHeight: "16px" }],
        body: ["12px", { lineHeight: "16px" }],
        placeholder: ["12px", { lineHeight: "16px" }],
        placeholderStrong: ["12px", { lineHeight: "16px" }],
        link: ["12px", { lineHeight: "16px" }],
        label: ["11px", { lineHeight: "14px" }],
        labelStrong: ["11px", { lineHeight: "14px" }],
      },
      fontWeight: {
        regular: "400",
        medium: "500",
        semiBold: "600",
        bold: "700",
      },
    },
  },
  plugins: [],
};

export default config;