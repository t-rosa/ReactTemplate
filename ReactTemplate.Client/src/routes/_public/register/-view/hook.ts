import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useForm } from "react-hook-form";
import { formSchema, type RegisterFormSchema } from "./types";

export function useRegisterView() {
  const { mutate, status } = $api.useMutation("post", "/register", {});

  const form = useForm<RegisterFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  function handleSubmit(values: RegisterFormSchema) {
    mutate({
      body: {
        email: values.email,
        password: values.password,
      },
    });
  }

  return { status, form, handleSubmit };
}
