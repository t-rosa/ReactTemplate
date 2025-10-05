import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
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
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Spinner } from "@/components/ui/spinner";
import { $api } from "@/lib/api/client";
import { cn } from "@/lib/utils";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { format, formatISO } from "date-fns";
import { fr } from "date-fns/locale";
import { CalendarIcon, PlusIcon } from "lucide-react";
import * as React from "react";
import { useForm } from "react-hook-form";
import { formSchema, type CreateForecastFormSchema } from "./create-forecast.types";

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
      successMessage: "Prévision ajouté",
      errorMessage: "Il y a eu une erreur.",
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
        <Button variant="outline">
          <PlusIcon /> Ajouter une prévision
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Ajouter une prévision</DialogTitle>
          <DialogDescription>Ajouter une nouvelle prévision météo à votre liste.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={void form.handleSubmit(onSubmit)} className="grid gap-3">
            <FormField
              control={form.control}
              name="date"
              render={({ field }) => (
                <FormItem className="flex flex-col">
                  <FormLabel>Date</FormLabel>
                  <Popover>
                    <PopoverTrigger asChild>
                      <FormControl>
                        <Button
                          variant={"outline"}
                          className={cn(
                            "pl-3 text-left font-normal",
                            !field.value && "text-muted-foreground",
                          )}
                        >
                          {field.value ?
                            format(field.value, "P", { locale: fr })
                          : <span>Pick a date</span>}
                          <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                        </Button>
                      </FormControl>
                    </PopoverTrigger>
                    <PopoverContent className="w-auto p-0" align="start">
                      <Calendar
                        mode="single"
                        selected={field.value}
                        onSelect={field.onChange}
                        disabled={(date) => date > new Date() || date < new Date("1900-01-01")}
                        captionLayout="dropdown"
                      />
                    </PopoverContent>
                  </Popover>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="temperatureC"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Température</FormLabel>
                  <FormControl>
                    <Input type="number" placeholder="20" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="summary"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Résumé</FormLabel>
                  <FormControl>
                    <Input placeholder="Cool" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <DialogFooter>
              <DialogClose asChild>
                <Button type="button" variant="outline">
                  Anuler
                </Button>
              </DialogClose>
              <Button type="submit" disabled={isPending}>
                {isPending && <Spinner />}
                Valider
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
