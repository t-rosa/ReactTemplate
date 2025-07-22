import { LogoutView } from "@/features/auth/logout/logout.view";
import { $api } from "@/lib/api/client";

export function DashboardView() {
  const { data: forecasts } = $api.useSuspenseQuery("get", "/api/weather-forecast");

  return (
    <div>
      <ul>
        {forecasts.map((forecast) => (
          <li key={`${forecast.date}-${forecast.temperatureC}-${forecast.summary}`}>
            {forecast.temperatureC}
          </li>
        ))}
      </ul>
      <LogoutView />
    </div>
  );
}
