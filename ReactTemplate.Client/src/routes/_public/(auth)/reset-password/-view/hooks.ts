import { $api } from "@/lib/api/client";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { useForm } from "react-hook-form";
import { formSchema, type ResetPasswordFormSchema } from "./types";

export function useResetPasswordView() {
  const { mutate, status } = $api.useMutation("post", "/resetPassword");

  const form = useForm<ResetPasswordFormSchema>({
    resolver: standardSchemaResolver(formSchema),
    defaultValues: {
      email: "",
      resetCode: "",
      newPassword: "",
      confirmPassword: "",
    },
  });

  function handleSubmit(values: ResetPasswordFormSchema) {
    mutate({
      body: {
        email: values.email,
        newPassword: values.newPassword,
        resetCode: values.resetCode,
      },
    });
  }

  return { status, form, handleSubmit };
}
