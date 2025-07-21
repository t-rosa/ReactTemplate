import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useNavigate } from "@tanstack/react-router";
import { useForm } from "react-hook-form";
import { formSchema, type ForgotPasswordFormSchema } from "./forgot-password.types";

export function useForgotPasswordView() {
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

  function handleSubmit(values: ForgotPasswordFormSchema) {
    mutate({
      body: values,
    });
  }

  return { status, form, handleSubmit };
}
