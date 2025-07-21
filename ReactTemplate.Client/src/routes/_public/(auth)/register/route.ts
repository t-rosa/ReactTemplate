import { RegisterView } from "@/routes/_public/(auth)/register/-view/view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/(auth)/register")({
  component: RegisterView,
});
