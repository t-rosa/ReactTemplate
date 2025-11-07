import { RegisterView } from "@/modules/auth/register.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/register")({
  component: RegisterView,
});
