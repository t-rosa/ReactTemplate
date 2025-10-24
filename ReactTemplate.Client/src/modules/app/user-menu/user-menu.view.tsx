import {
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { SidebarMenu, SidebarMenuItem, useSidebar } from "@/components/ui/sidebar";
import { useUser } from "@/modules/auth/authorize/authorize.hooks";
import { Authorize } from "@/modules/auth/authorize/authorize.view";
import { LogoutView } from "@/modules/auth/logout/logout.view";
import { Link } from "@tanstack/react-router";
import { ShieldIcon, UserCircleIcon } from "lucide-react";
import { UserMenuDropdown } from "./user-menu.ui";

export function UserMenu() {
  const { isMobile } = useSidebar();
  const { user } = useUser();

  return (
    <SidebarMenu>
      <SidebarMenuItem>
        <UserMenuDropdown user={user} side={isMobile ? "bottom" : "right"}>
          <DropdownMenuSeparator />

          <DropdownMenuGroup>
            <DropdownMenuItem disabled asChild>
              <div>
                <UserCircleIcon />
                Profile
              </div>
            </DropdownMenuItem>
            <Authorize role="Admin">
              <DropdownMenuItem asChild>
                <Link to="/admin">
                  <ShieldIcon />
                  Administration
                </Link>
              </DropdownMenuItem>
            </Authorize>
          </DropdownMenuGroup>
          <DropdownMenuSeparator />
          <LogoutView />
        </UserMenuDropdown>
      </SidebarMenuItem>
    </SidebarMenu>
  );
}
