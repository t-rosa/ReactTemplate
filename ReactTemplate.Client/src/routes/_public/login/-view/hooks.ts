import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useNavigate } from "@tanstack/react-router";
import { useForm } from "react-hook-form";
import { formSchema, type LoginFormSchema } from "./types";

export function useLoginView() {
  const navigate = useNavigate();

  const { mutate, status } = $api.useMutation("post", "/login", {
    async onSuccess() {
      await navigate({ to: "/dashboard" });
    },
  });

  const form = useForm<LoginFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  function handleSubmit(values: LoginFormSchema) {
    mutate({
      body: values,
      params: {
        query: {
          useCookies: true,
        },
      },
    });
  }

  return { status, form, handleSubmit };
}
