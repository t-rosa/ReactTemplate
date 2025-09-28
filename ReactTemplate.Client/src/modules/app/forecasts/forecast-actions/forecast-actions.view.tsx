import { AlertDialog, AlertDialogTrigger } from "@/components/ui/alert-dialog";
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
import type { CellContext } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";
import { RemoveForecast } from "../remove-forecast/remove-forecast";

type ForecastActionsProps = CellContext<
  components["schemas"]["GetWeatherForecastResponse"],
  unknown
>;

export function ForecastActions(props: ForecastActionsProps) {
  const forecast = props.row.original;

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
      <AlertDialog>
        <DropdownMenuContent align="end">
          <DropdownMenuLabel>Actions</DropdownMenuLabel>
          <DropdownMenuItem onClick={handleCopyIdClick}>Copier l&apos;ID</DropdownMenuItem>
          <DropdownMenuSeparator />
          <DropdownMenuItem asChild variant="destructive">
            <AlertDialogTrigger>Supprimer</AlertDialogTrigger>
          </DropdownMenuItem>
        </DropdownMenuContent>
        <RemoveForecast id={forecast.id} />
      </AlertDialog>
    </DropdownMenu>
  );
}
