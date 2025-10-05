import { z } from "zod";

export const formSchema = z.object({
  email: z.email({
    error: "Invalid email address",
  }),
});

export type ForgotPasswordFormSchema = z.infer<typeof formSchema>;
