import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import { ThemeSwitcher } from "@/features/theme-switcher/theme-switcher.view";
import { UserMenu } from "@/views/private/user-menu/user-menu.view";
import { Link } from "@tanstack/react-router";
import { FrameIcon, LayoutDashboardIcon } from "lucide-react";

function _ApplicationSidebar(props: React.PropsWithChildren) {
  return (
    <div className="pattern hidden sm:block">
      <Sidebar variant="floating" collapsible="icon">
        {props.children}
      </Sidebar>
    </div>
  );
}

function _ApplicationSidebarHeader() {
  return (
    <SidebarHeader className="border-b">
      <SidebarMenu>
        <SidebarMenuItem>
          <SidebarMenuButton asChild className="hover:bg-card">
            <Link to="/home">
              <FrameIcon />
              <span>ReactTemplate</span>
            </Link>
          </SidebarMenuButton>
        </SidebarMenuItem>
      </SidebarMenu>
    </SidebarHeader>
  );
}

function _ApplicationSidebarContent() {
  return (
    <SidebarContent>
      <SidebarGroup>
        <SidebarGroupContent>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarMenuButton tooltip="Cartes" asChild>
                <Link
                  to="/dashboard"
                  activeProps={{
                    className: "bg-muted",
                  }}
                >
                  <LayoutDashboardIcon />
                  <span>Dashboard</span>
                </Link>
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarGroupContent>
      </SidebarGroup>
      <SidebarGroup className="mt-auto">
        <SidebarGroupContent>
          <SidebarMenu>
            <ThemeSwitcher />
          </SidebarMenu>
        </SidebarGroupContent>
      </SidebarGroup>
    </SidebarContent>
  );
}

function _ApplicationSidebarFooter() {
  return (
    <SidebarFooter className="border-t">
      <UserMenu />
    </SidebarFooter>
  );
}

export const ApplicationSidebar = Object.assign(_ApplicationSidebar, {
  Content: _ApplicationSidebarContent,
  Header: _ApplicationSidebarHeader,
  Footer: _ApplicationSidebarFooter,
});
