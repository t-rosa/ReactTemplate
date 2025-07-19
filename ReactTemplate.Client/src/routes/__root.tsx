import { ThemeProvider } from "@/components/theme-provider";
import { TooltipProvider } from "@/components/ui/tooltip";
import type { QueryClient } from "@tanstack/react-query";
import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { Toaster } from "sonner";

interface RouterContext {
  queryClient: QueryClient;
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: Root,
});

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
