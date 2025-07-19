import { RegisterView } from "@/routes/_public/register/-view/view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/register")({
  component: RegisterView,
});
