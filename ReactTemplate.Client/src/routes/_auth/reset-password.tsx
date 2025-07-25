import { ResetPasswordView } from "@/features/auth/reset-password/reset-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/reset-password")({
  component: ResetPasswordView,
});
