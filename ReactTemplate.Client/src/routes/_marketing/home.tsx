import { HomeView } from "@/views/home/home.view";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_marketing/home")({
  component: HomeView,
});
