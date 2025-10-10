import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { FormField } from "@/components/ui/form";
import { Spinner } from "@/components/ui/spinner";
import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { formatISO } from "date-fns";
import { PlusIcon } from "lucide-react";
import * as React from "react";
import { useForm } from "react-hook-form";
import { formSchema, type CreateForecastFormSchema } from "./create-forecast.types";
import { CreateForecastForm } from "./create-forecast.ui";

export function CreateForecast() {
  const [open, setOpen] = React.useState(false);

  const form = useForm<CreateForecastFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      date: new Date(),
      temperatureC: "",
      summary: "",
    },
  });

  const { mutate, isPending } = $api.useMutation("post", "/api/weather-forecasts", {
    meta: {
      invalidatesQuery: ["get", "/api/weather-forecasts"],
      successMessage: "Forecast added",
      errorMessage: "An error occurred.",
    },
    onSuccess() {
      setOpen(!open);
    },
  });

  function onSubmit(values: CreateForecastFormSchema) {
    mutate({
      body: {
        temperatureC: parseInt(values.temperatureC),
        date: formatISO(values.date, { representation: "date" }),
        summary: values.summary,
      },
    });
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>
          <PlusIcon /> Add forecast
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add forecast</DialogTitle>
          <DialogDescription>Add a new weather forecast to your list.</DialogDescription>
        </DialogHeader>
        <CreateForecastForm form={form} onSubmit={onSubmit}>
          <FormField control={form.control} name="date" render={CreateForecastForm.Date} />
          <FormField
            control={form.control}
            name="temperatureC"
            render={CreateForecastForm.Temperature}
          />
          <FormField control={form.control} name="summary" render={CreateForecastForm.Summary} />
          <DialogFooter>
            <DialogClose asChild>
              <Button type="button" variant="outline">
                Cancel
              </Button>
            </DialogClose>
            <Button type="submit" disabled={isPending}>
              {isPending && <Spinner />}
              Submit
            </Button>
          </DialogFooter>
        </CreateForecastForm>
      </DialogContent>
    </Dialog>
  );
}
