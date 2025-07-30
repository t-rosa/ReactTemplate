import { $api } from "@/lib/api/client";
import { DashboardView } from "@/views/dashboard/dashboard.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_private/dashboard/")({
  loader({ context }) {
    return context.queryClient.ensureQueryData($api.queryOptions("get", "/api/weather-forecast"));
  },
  component: DashboardView,
});
