import { $api } from "@/lib/api/client";
import { useNavigate } from "@tanstack/react-router";

export function useLogoutView() {
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

  return { status, handleClick };
}
