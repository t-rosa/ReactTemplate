import { Login } from "@/views/public/login/login.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/login")({
  component: Login,
});
