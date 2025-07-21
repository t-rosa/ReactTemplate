import { ResetPasswordView } from "@/routes/_public/(auth)/reset-password/-view/reset-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/(auth)/reset-password")({
  component: ResetPasswordView,
});
