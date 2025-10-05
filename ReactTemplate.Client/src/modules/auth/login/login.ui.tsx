import { Button } from "@/components/ui/button";
import { Form, FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import type { ControllerRenderProps, UseFormReturn } from "react-hook-form";
import type { LoginFormSchema } from "./login.types";

interface LoginFormProps extends React.PropsWithChildren {
  form: UseFormReturn<LoginFormSchema>;
  onSubmit: (values: LoginFormSchema) => void;
}

function LoginFormRoot(props: LoginFormProps) {
  return (
    <Form {...props.form}>
      <form
        onSubmit={(e) => {
          void props.form.handleSubmit(props.onSubmit)(e);
        }}
        className="grid gap-6"
      >
        {props.children}
      </form>
    </Form>
  );
}

interface EmailProps {
  field: ControllerRenderProps<LoginFormSchema, "email">;
}

function Email(props: EmailProps) {
  return (
    <FormItem>
      <FormLabel>Courriel</FormLabel>
      <FormControl>
        <Input type="email" placeholder="nom@example.com" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface PasswordProps {
  field: ControllerRenderProps<LoginFormSchema, "password">;
}

function Password(props: PasswordProps) {
  return (
    <FormItem>
      <FormLabel>Mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="••••••••" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

interface SubmitProps {
  isPending: boolean;
}

function Submit(props: SubmitProps) {
  return (
    <Button type="submit" className="w-full" disabled={props.isPending}>
      {props.isPending ?
        <Spinner className="mr-2" />
      : null}
      {props.isPending ? "Connexion..." : "Se connecter"}
    </Button>
  );
}

export const LoginForm = Object.assign(LoginFormRoot, {
  Email,
  Password,
  Submit,
});
