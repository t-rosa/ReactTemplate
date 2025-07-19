import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";

interface ResetPasswordFormSubmitProps {
  isPending: boolean;
}

export function _ResetPasswordFormSubmit(props: ResetPasswordFormSubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Créer un compte
      {props.isPending && <Loader />}
    </Button>
  );
}
