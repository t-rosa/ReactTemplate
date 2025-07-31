import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { $api } from "@/lib/api/client";
import { useNavigate } from "@tanstack/react-router";
import { LogOutIcon } from "lucide-react";

export function LogoutView() {
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
        <Loader />
      </Button>
    );
  }

  return (
    <Button onClick={handleClick}>
      <LogOutIcon />
    </Button>
  );
}
