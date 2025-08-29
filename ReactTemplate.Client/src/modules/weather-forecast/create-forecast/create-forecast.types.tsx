import { z } from "zod";

export const formSchema = z.object({
  date: z.date(),
  temperatureC: z.string(),
  summary: z.string(),
});

export type CreateForecastFormSchema = z.infer<typeof formSchema>;
