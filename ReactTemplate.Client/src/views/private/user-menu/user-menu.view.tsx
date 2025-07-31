import {
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { SidebarMenu, SidebarMenuItem, useSidebar } from "@/components/ui/sidebar";
import { $api } from "@/lib/api/client";
import { UserCircleIcon } from "lucide-react";
import { LogoutView } from "../../../features/auth/logout/logout.view";
import { UserMenuDropdown } from "./user-menu.ui";

export function UserMenu() {
  const { isMobile } = useSidebar();
  const { data } = $api.useSuspenseQuery("get", "/manage/info");

  return (
    <SidebarMenu>
      <SidebarMenuItem>
        <UserMenuDropdown info={data} side={isMobile ? "bottom" : "right"}>
          <DropdownMenuSeparator />
          <DropdownMenuGroup>
            <DropdownMenuItem disabled asChild>
              <div>
                <UserCircleIcon />
                Profil
              </div>
            </DropdownMenuItem>
          </DropdownMenuGroup>
          <DropdownMenuSeparator />
          <LogoutView />
        </UserMenuDropdown>
      </SidebarMenuItem>
    </SidebarMenu>
  );
}
