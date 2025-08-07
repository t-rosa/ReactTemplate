import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { $api } from "@/lib/api/client";
import type { components } from "@/lib/api/schema";
import { CreateForecast } from "@/modules/weather-forecast/create-forecast/create-forecast.view";
import { XIcon } from "lucide-react";

export function ForecastsView() {
  const { data } = $api.useSuspenseQuery("get", "/api/weather-forecasts");

  return (
    <div>
      <SidebarTrigger />
      <CreateForecast />
      <ul className="grid gap-3 divide-y-2">
        {data.map((forecast) => (
          <Forecast key={forecast.id} forecast={forecast} />
        ))}
      </ul>
    </div>
  );
}

function Forecast(props: { forecast: components["schemas"]["GetWeatherForecastResponse"] }) {
  const { mutate, isPending } = $api.useMutation("delete", "/api/weather-forecasts/{id}", {
    meta: {
      invalidatesQuery: ["get", "/api/weather-forecasts"],
      successMessage: "Supprimé avec succès",
      errorMessage: "Il y a eu une erreur.",
    },
  });

  return (
    <li className="grid grid-cols-[1fr_auto_auto] gap-6">
      <div className="text-red-400">{props.forecast.id}</div>
      <div>{props.forecast.temperatureC}</div>
      <Button
        size="icon"
        disabled={isPending}
        onClick={() => mutate({ params: { path: { id: props.forecast.id! } } })}
      >
        {isPending ?
          <Loader />
        : <XIcon />}
      </Button>
    </li>
  );
}
