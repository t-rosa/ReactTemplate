import { $api } from "@/lib/api/client";
import { ForecastsView } from "@/views/private/forecasts/forecasts.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_private/forecasts/")({
  loader({ context }) {
    return context.queryClient.ensureQueryData($api.queryOptions("get", "/api/weather-forecasts"));
  },
  component: ForecastsView,
});
