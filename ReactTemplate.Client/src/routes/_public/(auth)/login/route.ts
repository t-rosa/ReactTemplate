import { LoginView } from "@/routes/_public/(auth)/login/-view/login.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/(auth)/login")({
  component: LoginView,
});
