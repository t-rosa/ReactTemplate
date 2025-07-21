import { RegisterView } from "@/routes/_auth/register/-view/register.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/register")({
  component: RegisterView,
});
