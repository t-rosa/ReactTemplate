import { Button } from "@/components/ui/button";
import { MoonIcon, SunIcon } from "lucide-react";
import { useTheme } from "./theme-provider";

export function ThemeSwitcher() {
  const { setTheme, theme } = useTheme();

  function handleClick() {
    if (theme === "light") {
      return setTheme("dark");
    }

    setTheme("light");
  }

  if (theme === "dark") {
    return (
      <Button variant="ghost" onClick={handleClick} size="icon" className="size-8">
        <SunIcon aria-hidden="true" />
      </Button>
    );
  }

  return (
    <Button variant="ghost" onClick={handleClick} size="icon" className="size-8">
      <MoonIcon aria-hidden="true" />
    </Button>
  );
}
