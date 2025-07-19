import { createFileRoute } from "@tanstack/react-router";
import { ForgotPasswordView } from "./-view/view";

export const Route = createFileRoute("/_public/forgot-password")({
  component: ForgotPasswordView,
});
