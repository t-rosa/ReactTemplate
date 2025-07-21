import { ResetPasswordView } from "@/routes/_auth/reset-password/-view/reset-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/reset-password")({
  component: ResetPasswordView,
});
