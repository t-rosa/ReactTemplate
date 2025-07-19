import { createFileRoute } from "@tanstack/react-router";
import { DashboardView } from "./-view/view";

export const Route = createFileRoute("/_private/dashboard")({
  component: DashboardView,
});
