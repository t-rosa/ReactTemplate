import { expect, test } from "@playwright/test";

test("has title", async ({ page }) => {
  await page.goto("/home");

  await expect(page).toHaveTitle("Vite + React + TS");
});
