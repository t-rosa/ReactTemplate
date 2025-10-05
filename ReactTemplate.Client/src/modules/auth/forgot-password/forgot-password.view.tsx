import { FormField } from "@/components/ui/form";
import { $api } from "@/lib/api/client";
import { AuthCard } from "@/modules/auth/components/auth-card";
import { AuthLayout } from "@/modules/auth/components/auth-layout";
import { standardSchemaResolver } from "@hookform/resolvers/standard-schema";
import { Link, useNavigate } from "@tanstack/react-router";
import { useForm } from "react-hook-form";
import { formSchema, type ForgotPasswordFormSchema } from "./forgot-password.types";
import { ForgotPasswordForm } from "./forgot-password.ui";

export function ForgotPasswordView() {
  const navigate = useNavigate();

  const { mutate, status } = $api.useMutation("post", "/api/auth/forgotPassword", {
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

  return (
    <AuthLayout>
      <AuthCard>
        <AuthCard.Content>
          <AuthCard.Header>
            <AuthCard.Title>Forgot password.</AuthCard.Title>
            <AuthCard.Description>Enter your email address.</AuthCard.Description>
          </AuthCard.Header>
          <ForgotPasswordForm form={form} onSubmit={handleSubmit}>
            <FormField control={form.control} name="email" render={ForgotPasswordForm.Email} />
            <ForgotPasswordForm.Submit isPending={status === "pending"} />
          </ForgotPasswordForm>
        </AuthCard.Content>
        <AuthCard.Footer>
          <Link to="/login">Log in</Link>
        </AuthCard.Footer>
      </AuthCard>
    </AuthLayout>
  );
}
