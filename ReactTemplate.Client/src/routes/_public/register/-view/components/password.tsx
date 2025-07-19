import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { ControllerRenderProps } from "react-hook-form";
import type { RegisterFormSchema } from "../types";

interface RegisterFormPasswordProps {
  field: ControllerRenderProps<RegisterFormSchema, "password">;
}

export function _RegisterFormPassword(props: RegisterFormPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}
