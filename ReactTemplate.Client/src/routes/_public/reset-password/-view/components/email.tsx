import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { ResetPasswordFormSchema } from "../types";

interface ResetPasswordFormEmailProps {
  field: ControllerRenderProps<ResetPasswordFormSchema, "email">;
}

export function _ResetPasswordFormEmail(props: ResetPasswordFormEmailProps) {
  return (
    <FormItem>
      <FormLabel>Email</FormLabel>
      <FormControl>
        <Input placeholder="email@react-template.com" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}
