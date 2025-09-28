import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";
import { $api } from "@/lib/api/client";

interface RemoveForecastsProps {
  ids: string[];
}

export function RemoveForecasts(props: RemoveForecastsProps) {
  const removeForecasts = $api.useMutation("post", "/api/weather-forecasts/bulk-delete", {
    meta: { invalidatesQuery: $api.queryOptions("get", "/api/weather-forecasts").queryKey },
  });

  if (props.ids.length === 0) {
    return null;
  }

  function handleRemoveClick() {
    removeForecasts.mutate({
      body: props.ids,
    });
  }

  return (
    <AlertDialog>
      <AlertDialogTrigger asChild>
        <Button variant="destructive">Supprimer</Button>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone. This will permanently delete your account and remove your
            data from our servers.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <AlertDialogAction onClick={handleRemoveClick}>Continue</AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
