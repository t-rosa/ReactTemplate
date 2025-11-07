import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { Spinner } from "@/components/ui/spinner";
import { $api } from "@/lib/api/client";
import { useNavigate } from "@tanstack/react-router";
import { LogOutIcon } from "lucide-react";

export function LogoutView() {
  const navigate = useNavigate();

  const { mutate, status } = $api.useMutation("post", "/api/auth/logout", {
    meta: {
      successMessage: "Logged out",
      errorMessage: "An error occurred.",
      invalidatesQuery: ["get", "manage/info"],
    },
    async onSuccess() {
      await navigate({ to: "/" });
    },
  });

  function handleClick() {
    mutate({});
  }

  if (status === "pending") {
    return (
      <DropdownMenuItem disabled>
        <Spinner />
        Logging out
      </DropdownMenuItem>
    );
  }

  return (
    <DropdownMenuItem onClick={handleClick}>
      <LogOutIcon />
      Log out
    </DropdownMenuItem>
  );
}
