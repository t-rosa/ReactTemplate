import { ThemeProvider } from "@/components/theme-provider";
import { TooltipProvider } from "@/components/ui/tooltip";
import { Outlet } from "@tanstack/react-router";
import { Toaster } from "sonner";

export function Root() {
  return (
    <ThemeProvider>
      <TooltipProvider delayDuration={0}>
        <Outlet />
        <Toaster closeButton />
      </TooltipProvider>
    </ThemeProvider>
  );
}
