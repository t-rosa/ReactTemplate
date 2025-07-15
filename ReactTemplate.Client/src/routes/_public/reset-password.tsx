import { ResetPassword } from "@/views/public/reset-password/reset-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/reset-password")({
  component: ResetPassword,
});
