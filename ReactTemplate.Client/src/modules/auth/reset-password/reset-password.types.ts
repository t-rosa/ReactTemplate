import { z } from "zod";

export const formSchema = z
  .object({
    email: z.email({
      error: "Invalid email address",
    }),
    resetCode: z.string({
      error: "Invalid code",
    }),
    newPassword: z
      .string({
        error: "Invalid password",
      })
      .min(6, "Password must be at least 6 characters long.")
      .regex(/[A-Z]/, "Password must contain at least one uppercase letter.")
      .regex(/[a-z]/, "Password must contain at least one lowercase letter.")
      .regex(/[0-9]/, "Password must contain at least one digit.")
      .regex(/[^a-zA-Z0-9]/, "Password must contain at least one special character."),
    confirmPassword: z
      .string({
        error: "Invalid password",
      })
      .min(6, "Password must be at least 6 characters long.")
      .regex(/[A-Z]/, "Password must contain at least one uppercase letter.")
      .regex(/[a-z]/, "Password must contain at least one lowercase letter.")
      .regex(/[0-9]/, "Password must contain at least one digit.")
      .regex(/[^a-zA-Z0-9]/, "Password must contain at least one special character."),
  })
  .refine((values) => values.newPassword === values.confirmPassword, {
    error: "Passwords do not match.",
    path: ["confirmPassword"],
  });

export type ResetPasswordFormSchema = z.infer<typeof formSchema>;
