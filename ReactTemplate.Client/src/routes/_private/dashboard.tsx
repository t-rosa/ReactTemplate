import { DashboardView } from "@/views/dashboard/dashboard.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_private/dashboard")({
  component: DashboardView,
});
