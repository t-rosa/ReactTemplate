import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";

interface LoginFormSubmitProps {
  isPending: boolean;
}

export function _LoginFormSubmit(props: LoginFormSubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Connexion
      {props.isPending && <Loader />}
    </Button>
  );
}
