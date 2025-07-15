import { Dashboard } from "@/views/private/dashboard/dashboard.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_private/dashboard")({
  component: Dashboard,
});
