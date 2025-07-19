import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { ResetPasswordFormSchema } from "../types";

interface ResetPasswordFormNewPasswordProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "newPassword">;
}

export function _ResetPasswordFormNewPassword(props: ResetPasswordFormNewPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Nouveau mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}
