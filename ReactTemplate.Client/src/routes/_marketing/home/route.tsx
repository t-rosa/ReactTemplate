import { createFileRoute } from "@tanstack/react-router";
import { HomeView } from "./-view/home.view";

export const Route = createFileRoute("/_marketing/home")({
  component: HomeView,
});
