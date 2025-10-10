import { SidebarProvider } from "@/components/ui/sidebar";
import { Outlet, useRouteContext } from "@tanstack/react-router";
import { AppLayout } from "./app.ui";
import { AppSidebar } from "./components/app-sidebar";

export function AppView() {
  const context = useRouteContext({ from: "/_app/forecasts/" });

  return (
    <SidebarProvider>
      <AppSidebar>
        <AppSidebar.Header />
        <AppSidebar.Content />
        <AppSidebar.Footer />
      </AppSidebar>
      <AppLayout>
        <AppLayout.Header title={context.title} />
        <Outlet />
      </AppLayout>
    </SidebarProvider>
  );
}
