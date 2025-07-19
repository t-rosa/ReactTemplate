import { FormControl, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import type { components } from "@/lib/api/schema";
import { Link } from "@tanstack/react-router";
import type { ControllerRenderProps } from "react-hook-form";

interface LoginFormPasswordProps {
  field: ControllerRenderProps<components["schemas"]["LoginRequest"], "password">;
}

export function _LoginFormPassword(props: LoginFormPasswordProps) {
  return (
    <FormItem>
      <FormLabel>Mot de passe</FormLabel>
      <FormControl>
        <Input type="password" placeholder="Mot de passe" {...props.field} />
      </FormControl>
      <Link
        to="/forgot-password"
        className="text-foreground/80 inline-flex w-fit justify-start p-0 text-sm/5 font-normal hover:underline"
      >
        Mot de passe oublié ?
      </Link>
      <FormMessage />
    </FormItem>
  );
}
