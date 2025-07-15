import { Register } from "@/views/public/register/register.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/register")({
  component: Register,
});
