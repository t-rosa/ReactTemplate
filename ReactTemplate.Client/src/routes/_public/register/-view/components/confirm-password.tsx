import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { RegisterFormSchema } from "../types";

interface RegisterFormConfirmPasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "confirmPassword">;
}

export function _RegisterFormConfirmPassword(props: RegisterFormConfirmPasswordProps) {
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
