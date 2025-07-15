import { Button } from "@/components/ui/button";
import { $api } from "@/lib/api/client";
import { useNavigate } from "@tanstack/react-router";
import { Loader2Icon, LogOutIcon } from "lucide-react";

export function Logout() {
  const navigate = useNavigate();

  const { mutate, status } = $api.useMutation("post", "/logout", {
    meta: {
      successMessage: "Déconnecté",
      errorMessage: "Il y a eu une érreur.",
      invalidatesQuery: ["get", "manage/info"],
    },
    async onSuccess() {
      await navigate({ to: "/" });
    },
  });

  function handleClick() {
    mutate({ body: {} });
  }

  if (status === "pending") {
    return (
      <Button disabled>
        <Loader2Icon className="animate-spin" />
      </Button>
    );
  }

  return (
    <Button onClick={handleClick}>
      <LogOutIcon />
    </Button>
  );
}
