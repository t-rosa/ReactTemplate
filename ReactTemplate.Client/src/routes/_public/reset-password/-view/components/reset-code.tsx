import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { ResetPasswordFormSchema } from "../types";

interface ResetPasswordFormResetCodeProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "resetCode">;
}

export function _ResetPasswordFormResetCode(props: ResetPasswordFormResetCodeProps) {
  return (
    <FormItem>
      <FormLabel>Code de réinitialisation</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Code de réinitialisation" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}
