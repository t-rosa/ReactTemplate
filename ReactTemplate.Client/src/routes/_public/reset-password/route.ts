import { ResetPasswordView } from "@/routes/_public/reset-password/-view/view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/reset-password")({
  component: ResetPasswordView,
});
