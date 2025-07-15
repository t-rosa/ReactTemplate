import { ForgotPassword } from "@/views/public/forgot-password/forgot-password.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/forgot-password")({
  component: ForgotPassword,
});
