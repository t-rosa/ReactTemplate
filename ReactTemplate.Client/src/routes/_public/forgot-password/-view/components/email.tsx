import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { components } from "@/lib/api/schema";
import type { ControllerRenderProps } from "react-hook-form";

interface ForgotPasswordFormEmailProps {
  field: ControllerRenderProps<components["schemas"]["ForgotPasswordRequest"], "email">;
}

export function _ForgotPasswordFormEmail(props: ForgotPasswordFormEmailProps) {
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
