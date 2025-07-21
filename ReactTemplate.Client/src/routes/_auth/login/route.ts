import { LoginView } from "@/routes/_auth/login/-view/login.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/login")({
  component: LoginView,
});
