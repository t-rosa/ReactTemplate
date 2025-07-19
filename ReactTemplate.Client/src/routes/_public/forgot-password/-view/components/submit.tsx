import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";

interface ForgotPasswordFormSubmitProps {
  isPending: boolean;
}

export function _ForgotPasswordFormSubmit(props: ForgotPasswordFormSubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Envoyer
      {props.isPending && <Loader />}
    </Button>
  );
}
