import { $api } from "@/lib/api/client";
import { FORECAST_COLUMNS } from "./forecast-table/forecast-columns";
import { ForecastTable } from "./forecast-table/forecast-table.view";

export function ForecastsView() {
  const { data } = $api.useSuspenseQuery("get", "/api/weather-forecasts");

  return (
    <div className="container mx-auto py-10">
      <ForecastTable columns={FORECAST_COLUMNS} data={data} />
    </div>
  );
}
