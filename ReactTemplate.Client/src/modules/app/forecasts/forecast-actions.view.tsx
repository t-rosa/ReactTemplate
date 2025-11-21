import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import type { components } from "@/lib/api/schema";
import { RemoveForecast } from "@/modules/app/forecasts/remove-forecast";
import type { CellContext } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";
import { UpdateForecast } from "./update-forecast";

type ForecastActionsProps = CellContext<components["schemas"]["WeatherForecastResponse"], unknown>;

export function ForecastActions(props: ForecastActionsProps) {
  const forecast: components["schemas"]["WeatherForecastResponse"] = props.row.original;

  function handleCopyIdClick() {
    void navigator.clipboard.writeText(forecast.id);
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost">
          <MoreHorizontal />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuLabel>Actions</DropdownMenuLabel>
        <DropdownMenuItem onClick={handleCopyIdClick}>Copy ID</DropdownMenuItem>
        <DropdownMenuSeparator />
        <UpdateForecast forecast={forecast} />
        <RemoveForecast id={forecast.id} />
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
