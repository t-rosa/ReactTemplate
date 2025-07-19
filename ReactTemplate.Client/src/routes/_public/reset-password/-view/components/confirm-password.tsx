import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { ResetPasswordFormSchema } from "../types";

interface ResetPasswordFormConfirmPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "confirmPassword">;
}

export function _ResetPasswordFormConfirmPassword(props: ResetPasswordFormConfirmPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Confirmer le mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Confirmation" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}
