import tailwindcss from "@tailwindcss/vite";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import react from "@vitejs/plugin-react-oxc";
import child_process from "child_process";
import fs from "fs";
import { fileURLToPath, URL } from "node:url";
import path from "path";
import { env } from "process";
import { defineConfig } from "vite";

const baseFolder =
  env.APPDATA !== undefined && env.APPDATA !== "" ?
    `${env.APPDATA}/ASP.NET/https`
  : `${env.HOME}/.aspnet/https`;

const certificateName = "ReactTemplate.Client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
  fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (
    0 !==
    child_process.spawnSync(
      "dotnet",
      ["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"],
      { stdio: "inherit" },
    ).status
  ) {
    throw new Error("Could not create certificate.");
  }
}

const target =
  env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7000";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    tanstackRouter({
      target: "react",
      autoCodeSplitting: true,
    }),
    react(),
    tailwindcss(),
  ],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  build: {
    rollupOptions: {
      output: {
        advancedChunks: {
          groups: [
            {
              test: /node_modules\/react/,
              name: "react",
            },
            {
              test: /node_modules\/react-dom/,
              name: "react-dom",
            },
            {
              test: /node_modules\/@tanstack/,
              name: "tanstack",
            },
            {
              test: /node_modules\/react-hook-form/,
              name: "react-hook-form",
            },
            {
              test: /node_modules\/radix-ui/,
              name: "radix-ui",
            },
            {
              test: /node_modules\/zod/,
              name: "zod",
            },
          ],
        },
      },
    },
    minify: true,
  },
  server: {
    proxy: {
      "^/register": {
        target,
        secure: false,
      },
      "^/login": {
        target,
        secure: false,
      },
      "^/refresh": {
        target,
        secure: false,
      },
      "^/confirmEmail": {
        target,
        secure: false,
      },
      "^/resendConfirmationEmail": {
        target,
        secure: false,
      },
      "^/forgotPassword": {
        target,
        secure: false,
      },
      "^/resetPassword": {
        target,
        secure: false,
      },
      "^/manage": {
        target,
        secure: false,
      },
      "^/logout": {
        target,
        secure: false,
      },
      "^/api": {
        target,
        secure: false,
      },
    },
    port: parseInt(env.DEV_SERVER_PORT ?? "50000"),
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath),
    },
  },
});
