import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";

interface RegisterFormSubmitProps {
  isPending: boolean;
}

export function _RegisterFormSubmit(props: RegisterFormSubmitProps) {
  return (
    <Button type="submit" disabled={props.isPending} className="w-full rounded-full">
      Créer un compte
      {props.isPending && <Loader />}
    </Button>
  );
}
