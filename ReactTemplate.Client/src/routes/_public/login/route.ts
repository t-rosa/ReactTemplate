import { LoginView } from "@/routes/_public/login/-view/view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/login")({
  component: LoginView,
});
