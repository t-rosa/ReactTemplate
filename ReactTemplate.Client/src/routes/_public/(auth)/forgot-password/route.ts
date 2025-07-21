import { createFileRoute } from "@tanstack/react-router";
import { ForgotPasswordView } from "./-view/forgot-password.view";

export const Route = createFileRoute("/_public/(auth)/forgot-password")({
  component: ForgotPasswordView,
});
