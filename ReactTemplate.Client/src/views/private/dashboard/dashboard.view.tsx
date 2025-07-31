import { SidebarTrigger } from "@/components/ui/sidebar";
import { $api } from "@/lib/api/client";

export function DashboardView() {
  const { data: forecasts } = $api.useSuspenseQuery("get", "/api/weather-forecasts");

  return (
    <div>
      <SidebarTrigger />
      <ul>
        {forecasts.map((forecast) => (
          <li key={`${forecast.date}-${forecast.temperatureC}-${forecast.summary}`}>
            {forecast.temperatureC}
          </li>
        ))}
      </ul>
    </div>
  );
}
