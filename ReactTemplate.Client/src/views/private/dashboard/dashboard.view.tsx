import { Button } from "@/components/ui/button";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { CreateForecast } from "@/features/weather-forecast/create-forecast/create-forecast.view";
import { forecastsCollection } from "@/lib/collections";
import { useLiveQuery } from "@tanstack/react-db";
import { XIcon } from "lucide-react";

export function DashboardView() {
  const { data: forecasts } = useLiveQuery((query) =>
    query
      .from({ forecasts: forecastsCollection })
      .orderBy(({ forecasts }) => forecasts.temperatureC, "desc"),
  );

  return (
    <div>
      <SidebarTrigger />
      <CreateForecast />
      <ul className="grid gap-3 divide-y-2">
        {forecasts.map((forecast) => (
          <li key={forecast.id} className="grid grid-cols-[1fr_auto_auto] gap-6">
            <div className="text-red-400">{forecast.id}</div>
            <div>{forecast.temperatureC}</div>
            <Button size="icon" onClick={() => forecastsCollection.delete(forecast.id!)}>
              <XIcon />
            </Button>
          </li>
        ))}
      </ul>
    </div>
  );
}
