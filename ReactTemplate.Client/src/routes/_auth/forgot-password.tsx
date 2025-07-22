import { ForgotPasswordView } from "@/features/auth/forgot-password/forgot-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/forgot-password")({
  component: ForgotPasswordView,
});
