import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import { format } from "date-fns";
import { fr } from "date-fns/locale";
import { CalendarIcon } from "lucide-react";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { CreateForecastFormSchema } from "./create-forecast.types";

interface CreateForecastFormProps extends React.PropsWithChildren {
  form: UseFormReturn<CreateForecastFormSchema>;
  onSubmit: (values: CreateForecastFormSchema) => void;
}

function CreateForecastFormRoot(props: CreateForecastFormProps) {
  return (
    <Form {...props.form}>
      <form
        onSubmit={(e) => {
          void props.form.handleSubmit(props.onSubmit)(e);
        }}
        className="grid gap-3"
      >
        {props.children}
      </form>
    </Form>
  );
}

interface DateProps {
  field: ControllerRenderProps<CreateForecastFormSchema, "date">;
}

function Date(props: DateProps) {
  return (
    <FormItem className="flex flex-col">
      <FormLabel>Date</FormLabel>
      <Popover>
        <PopoverTrigger asChild>
          <FormControl>
            <Button
              variant="outline"
              className={cn(
                "pl-3 text-left font-normal",
                !props.field.value && "text-muted-foreground",
              )}
            >
              {props.field.value ?
                format(props.field.value, "P", { locale: fr })
              : <span>Pick a date</span>}
              <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
            </Button>
          </FormControl>
        </PopoverTrigger>
        <PopoverContent className="w-auto p-0" align="start">
          <Calendar
            mode="single"
            selected={props.field.value}
            onSelect={props.field.onChange}
            disabled={(date) =>
              date > new globalThis.Date() || date < new globalThis.Date("1900-01-01")
            }
            captionLayout="dropdown"
          />
        </PopoverContent>
      </Popover>
      <FormMessage />
    </FormItem>
  );
}

interface TemperatureProps {
  field: ControllerRenderProps<CreateForecastFormSchema, "temperatureC">;
}

function Temperature(props: TemperatureProps) {
  return (
    <FormItem>
      <FormLabel>Temperature</FormLabel>
      <FormControl>
        <Input type="number" placeholder="20" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface SummaryProps {
  field: ControllerRenderProps<CreateForecastFormSchema, "summary">;
}

function Summary(props: SummaryProps) {
  return (
    <FormItem>
      <FormLabel>Summary</FormLabel>
      <FormControl>
        <Input placeholder="Cool" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

export const CreateForecastForm = Object.assign(CreateForecastFormRoot, {
  Date,
  Temperature,
  Summary,
});
