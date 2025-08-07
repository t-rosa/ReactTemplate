import { LoginView } from "@/modules/users/login/login.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/login")({
  component: LoginView,
});
