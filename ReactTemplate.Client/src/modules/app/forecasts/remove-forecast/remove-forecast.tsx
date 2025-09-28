import {
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { $api } from "@/lib/api/client";

interface RemoveForecastProps {
  id: string;
}

export function RemoveForecast(props: RemoveForecastProps) {
  const removeForecasts = $api.useMutation("delete", "/api/weather-forecasts/{id}", {
    meta: { invalidatesQuery: $api.queryOptions("get", "/api/weather-forecasts").queryKey },
  });

  function handleRemoveClick() {
    removeForecasts.mutate({
      params: {
        path: {
          id: props.id,
        },
      },
    });
  }

  return (
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
  );
}
