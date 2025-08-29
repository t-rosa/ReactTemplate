import { SidebarProvider } from "@/components/ui/sidebar";
import { Outlet } from "@tanstack/react-router";
import { ApplicationLayout } from "./components/application-layout";
import { ApplicationSidebar } from "./components/application-sidebar";

export function PrivateView() {
  return (
    <SidebarProvider>
      <ApplicationSidebar>
        <ApplicationSidebar.Header />
        <ApplicationSidebar.Content />
        <ApplicationSidebar.Footer />
      </ApplicationSidebar>
      <ApplicationLayout>
        <Outlet />
      </ApplicationLayout>
    </SidebarProvider>
  );
}
