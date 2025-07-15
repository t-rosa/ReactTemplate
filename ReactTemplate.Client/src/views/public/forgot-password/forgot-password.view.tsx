import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useNavigate } from "@tanstack/react-router";
import { Loader2Icon } from "lucide-react";
import { useForm } from "react-hook-form";
import { z } from "zod";

const formSchema = z.object({
  email: z
    .string({
      error: "L'email est invalide",
    })
    .email({
      error: "L'email est invalide",
    }),
});

type ForgotPasswordFormSchema = z.infer<typeof formSchema>;

export function ForgotPassword() {
  const navigate = useNavigate();
  const { mutate, status } = $api.useMutation("post", "/forgotPassword", {
    async onSuccess() {
      await navigate({ to: "/reset-password" });
    },
  });

  const form = useForm<ForgotPasswordFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
    },
  });

  function onSubmit(values: ForgotPasswordFormSchema) {
    mutate({
      body: values,
    });
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input type="email" placeholder="example@react-template.fr" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" disabled={status === "pending"}>
          Envoyer
          {status === "pending" && <Loader2Icon className="animate-spin" />}
        </Button>
      </form>
    </Form>
  );
}
