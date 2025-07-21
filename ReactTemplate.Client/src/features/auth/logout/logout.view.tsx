import { Loader } from "@/components/loader";
import { Button } from "@/components/ui/button";
import { LogOutIcon } from "lucide-react";
import { useLogoutView } from "./logout.hook";

export function LogoutView() {
  const { status, handleClick } = useLogoutView();

  if (status === "pending") {
    return (
      <Button disabled>
        <Loader />
      </Button>
    );
  }

  return (
    <Button onClick={handleClick}>
      <LogOutIcon />
    </Button>
  );
}
