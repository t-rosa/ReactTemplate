import { Link } from "@tanstack/react-router";
import {
  CheckIcon,
  Code2,
  CopyIcon,
  Database,
  Layers,
  MoonIcon,
  Rocket,
  Shield,
  SunIcon,
  Zap,
} from "lucide-react";
import * as React from "react";

import { Container } from "@/components/container";
import { useTheme } from "@/components/theme-provider";
import { Button } from "@/components/ui/button";

const quickStartSteps = [
  {
    step: "01",
    title: "Install",
    command: "dotnet new install .",
    color: { bg: "bg-blue-500/10", text: "text-blue-500" },
  },
  {
    step: "02",
    title: "Create",
    command: "dotnet new full-stack-react -o MyProject --title my-project",
    color: { bg: "bg-purple-500/10", text: "text-purple-500" },
  },
  {
    step: "03",
    title: "Configure",
    command: 'dotnet user-secrets set "ADMIN_EMAIL" "admin@example.com"',
    color: { bg: "bg-pink-500/10", text: "text-pink-500" },
  },
  {
    step: "04",
    title: "Launch",
    command: "docker compose up -d && dotnet run --project ReactTemplate.Server",
    color: { bg: "bg-green-500/10", text: "text-green-500" },
  },
];

const features = [
  {
    icon: Rocket,
    title: "Production Ready",
    description: "Full-stack architecture with ASP.NET Core 9 and React 19.2",
    accentColor: "text-blue-500",
  },
  {
    icon: Layers,
    title: "Vertical Slices",
    description: "Feature-first modules for clean, maintainable code",
    accentColor: "text-purple-500",
  },
  {
    icon: Shield,
    title: "Auth Built-in",
    description: "Identity, roles, and protected routes out of the box",
    accentColor: "text-green-500",
  },
  {
    icon: Zap,
    title: "Type-Safe",
    description: "OpenAPI-generated types for end-to-end safety",
    accentColor: "text-orange-500",
  },
];

const techStack = {
  server: [
    { name: "ASP.NET Core 9", category: "Framework" },
    { name: "Entity Framework Core", category: "ORM" },
    { name: "Identity", category: "Auth" },
    { name: "FluentValidation", category: "Validation" },
    { name: "OpenTelemetry", category: "Observability" },
    { name: "xUnit", category: "Testing" },
    { name: "Testcontainers", category: "Testing" },
  ],
  client: [
    { name: "React 19.2", category: "Framework" },
    { name: "Vite", category: "Build Tool" },
    { name: "TanStack Router", category: "Routing" },
    { name: "TanStack Query", category: "Data Fetching" },
    { name: "React Hook Form", category: "Forms" },
    { name: "Tailwind CSS", category: "Styling" },
    { name: "shadcn/ui", category: "Components" },
    { name: "Zod", category: "Validation" },
    { name: "openapi-typescript", category: "Types" },
    { name: "Playwright", category: "E2E Testing" },
  ],
};

export function HomeView() {
  return (
    <div className="relative overflow-hidden">
      <div className="bg-primary/5 absolute top-0 left-1/2 h-[500px] w-[500px] -translate-x-1/2 rounded-full blur-3xl" />
      <HeroSection />
      <FeaturesSection />
      <QuickStartSection />
      <TechStackSection />
      <CTASection />
    </div>
  );
}

function HeroSection() {
  return (
    <section className="relative py-16 sm:py-24 lg:py-32">
      <Container>
        <div className="flex flex-col gap-8 sm:gap-12">
          <div className="flex justify-end">
            <ThemeToggleButton />
          </div>

          <div className="grid gap-12 lg:grid-cols-2 lg:items-center lg:gap-16">
            <div className="flex flex-col gap-6 sm:gap-8">
              <div className="inline-flex w-max items-center gap-2">
                <div className="flex items-center justify-center rounded-lg bg-blue-500/10 p-2">
                  <Rocket className="size-5 text-blue-500" />
                </div>
                <span className="text-muted-foreground text-sm font-medium">
                  Full-Stack Template
                </span>
              </div>

              <h1 className="text-foreground text-4xl font-bold tracking-tight sm:text-5xl lg:text-6xl">
                Build faster with{" "}
                <span className="bg-gradient-to-r from-blue-500 to-purple-500 bg-clip-text text-transparent">
                  React + .NET
                </span>
              </h1>

              <p className="text-muted-foreground text-base leading-relaxed sm:text-lg">
                Production-ready template combining ASP.NET Core 9 and React 19.2. Vertical slice
                architecture, type-safe APIs, and everything you need to ship fast.
              </p>

              <div className="flex flex-col gap-3 sm:flex-row">
                <Link
                  to="/register"
                  className="bg-primary text-primary-foreground hover:bg-primary/90 inline-flex items-center justify-center rounded-lg px-6 py-3 text-sm font-semibold shadow-lg transition-all"
                >
                  Try Demo
                </Link>
                <Link
                  to="/forecasts"
                  className="border-border hover:bg-muted inline-flex items-center justify-center rounded-lg border px-6 py-3 text-sm font-semibold transition-colors"
                >
                  View Features
                </Link>
              </div>

              <div className="flex flex-wrap items-center gap-6 pt-4">
                <div className="flex items-center gap-2">
                  <Code2 className="text-primary size-5" />
                  <span className="text-muted-foreground text-sm">Type-Safe</span>
                </div>
                <div className="flex items-center gap-2">
                  <Database className="text-primary size-5" />
                  <span className="text-muted-foreground text-sm">PostgreSQL</span>
                </div>
                <div className="flex items-center gap-2">
                  <Shield className="text-primary size-5" />
                  <span className="text-muted-foreground text-sm">Auth Ready</span>
                </div>
              </div>
            </div>

            <div className="border-border bg-card relative overflow-hidden rounded-2xl border p-6 shadow-2xl sm:p-8">
              <div className="bg-primary/10 absolute -top-20 -right-20 h-40 w-40 rounded-full blur-3xl" />
              <div className="bg-primary/10 absolute -bottom-20 -left-20 h-40 w-40 rounded-full blur-3xl" />

              <div className="relative space-y-4">
                <div className="bg-muted/50 rounded-lg p-4">
                  <p className="text-primary font-mono text-xs">$ dotnet new install .</p>
                </div>
                <div className="bg-muted/50 rounded-lg p-4">
                  <p className="text-primary font-mono text-xs">
                    $ dotnet new full-stack-react -o MyApp
                  </p>
                </div>
                <div className="bg-muted/50 rounded-lg p-4">
                  <p className="text-primary font-mono text-xs">$ docker compose up -d</p>
                </div>
                <div className="flex items-center gap-2 rounded-lg bg-green-500/10 p-4">
                  <CheckIcon className="size-4 text-green-500" />
                  <p className="text-foreground text-sm font-medium">Ready to code in seconds</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Container>
    </section>
  );
}

function FeaturesSection() {
  return (
    <section className="py-16 sm:py-24">
      <Container>
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4 lg:gap-8">
          {features.map((feature) => (
            <div
              key={feature.title}
              className="group border-border hover:border-primary/50 bg-card hover:shadow-primary/5 relative overflow-hidden rounded-xl border p-6 transition-all hover:shadow-lg"
            >
              <div className="bg-primary/10 group-hover:bg-primary/20 mb-4 flex h-12 w-12 items-center justify-center rounded-lg transition-colors">
                <feature.icon className={`${feature.accentColor} size-6`} />
              </div>
              <h3 className="text-foreground mb-2 text-base font-semibold">{feature.title}</h3>
              <p className="text-muted-foreground text-sm leading-relaxed">{feature.description}</p>
            </div>
          ))}
        </div>
      </Container>
    </section>
  );
}

function QuickStartSection() {
  const [copiedIndex, setCopiedIndex] = React.useState<number | null>(null);
  const timerRef = React.useRef<number | null>(null);

  React.useEffect(() => {
    return () => {
      if (timerRef.current) {
        window.clearTimeout(timerRef.current);
      }
    };
  }, []);

  async function handleCopy(command: string, index: number) {
    if (typeof navigator === "undefined" || !navigator.clipboard) {
      return;
    }

    try {
      await navigator.clipboard.writeText(command);
      setCopiedIndex(index);
      if (timerRef.current) {
        window.clearTimeout(timerRef.current);
      }
      timerRef.current = window.setTimeout(() => {
        setCopiedIndex(null);
      }, 2000);
    } catch (error) {
      console.error("Failed to copy command", error);
    }
  }

  return (
    <section className="bg-muted/30 py-16 sm:py-24">
      <Container>
        <div className="flex flex-col gap-12">
          <div className="flex flex-col gap-3">
            <h2 className="text-foreground text-3xl font-bold tracking-tight sm:text-4xl">
              Quick Start
            </h2>
            <p className="text-muted-foreground max-w-2xl text-base">
              Get your full-stack app running in 4 simple steps
            </p>
          </div>

          <div className="grid gap-6 sm:grid-cols-2 lg:gap-8">
            {quickStartSteps.map((step, index) => (
              <div
                key={step.step}
                className="border-border bg-card group relative overflow-hidden rounded-xl border p-6 shadow-sm transition-all hover:shadow-md"
              >
                <div className="mb-4 flex items-start justify-between">
                  <div
                    className={`${step.color.bg} flex h-10 w-10 items-center justify-center rounded-lg`}
                  >
                    <span className={`${step.color.text} text-sm font-bold`}>{step.step}</span>
                  </div>
                </div>

                <h3 className="text-foreground mb-3 text-lg font-semibold">{step.title}</h3>

                <div className="bg-muted/50 group relative overflow-hidden rounded-lg">
                  <pre className="text-muted-foreground overflow-x-auto px-4 py-3 font-mono text-xs">
                    {step.command}
                  </pre>
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon-sm"
                    className="absolute top-2 right-2 opacity-0 transition-opacity group-hover:opacity-100"
                    onClick={() => handleCopy(step.command, index)}
                    aria-label={`Copy ${step.title}`}
                  >
                    {copiedIndex === index ?
                      <CheckIcon className="size-4" />
                    : <CopyIcon className="size-4" />}
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </Container>
    </section>
  );
}

function TechStackSection() {
  return (
    <section className="py-16 sm:py-24">
      <Container>
        <div className="flex flex-col gap-12">
          <div className="flex flex-col gap-3">
            <h2 className="text-foreground text-3xl font-bold tracking-tight sm:text-4xl">
              Modern Tech Stack
            </h2>
            <p className="text-muted-foreground max-w-2xl text-base">
              Production-grade technologies carefully selected for performance and developer
              experience
            </p>
          </div>

          <div className="grid gap-8 lg:grid-cols-2">
            <TechStackColumn
              heading="Server"
              icon={Database}
              items={techStack.server}
              iconColor="text-blue-500"
              bgColor="bg-blue-500/10"
            />
            <TechStackColumn
              heading="Client"
              icon={Code2}
              items={techStack.client}
              iconColor="text-purple-500"
              bgColor="bg-purple-500/10"
            />
          </div>
        </div>
      </Container>
    </section>
  );
}

interface TechStackColumnProps {
  heading: string;
  icon: React.ElementType;
  items: { name: string; category: string }[];
  iconColor: string;
  bgColor: string;
}

function TechStackColumn(props: TechStackColumnProps) {
  return (
    <div className="border-border bg-card rounded-xl border p-6 shadow-sm sm:p-8">
      <div className="mb-6 flex items-center gap-3">
        <div className={`${props.bgColor} flex h-10 w-10 items-center justify-center rounded-lg`}>
          <props.icon className={`${props.iconColor} size-5`} />
        </div>
        <h3 className="text-foreground text-xl font-semibold">{props.heading}</h3>
      </div>

      <div className="grid gap-3 sm:grid-cols-2">
        {props.items.map((item) => (
          <div
            key={item.name}
            className="border-border hover:border-primary/50 group flex flex-col gap-1 rounded-lg border p-3 transition-colors"
          >
            <span className="text-foreground text-sm font-medium">{item.name}</span>
            <span className="text-muted-foreground text-xs">{item.category}</span>
          </div>
        ))}
      </div>
    </div>
  );
}

function CTASection() {
  return (
    <section className="py-16 sm:py-24">
      <Container>
        <div className="from-primary/10 via-primary/5 to-background relative overflow-hidden rounded-2xl bg-gradient-to-br p-8 sm:p-12 lg:p-16">
          <div className="bg-primary/20 absolute -top-20 -right-20 h-60 w-60 rounded-full blur-3xl" />
          <div className="bg-primary/20 absolute -bottom-20 -left-20 h-60 w-60 rounded-full blur-3xl" />

          <div className="relative mx-auto flex max-w-3xl flex-col gap-8 text-center">
            <div className="flex flex-col gap-4">
              <h2 className="text-foreground text-3xl font-bold tracking-tight sm:text-4xl lg:text-5xl">
                Ready to build something amazing?
              </h2>
              <p className="text-muted-foreground mx-auto max-w-2xl text-base sm:text-lg">
                Experience the template with a live demo or dive into the code on GitHub
              </p>
            </div>

            <div className="flex flex-col gap-3 sm:flex-row sm:justify-center">
              <Link
                to="/login"
                className="bg-primary text-primary-foreground hover:bg-primary/90 inline-flex items-center justify-center rounded-lg px-8 py-4 text-base font-semibold shadow-lg transition-all"
              >
                Try Live Demo
              </Link>
              <a
                href="https://github.com/t-rosa/ReactTemplate"
                target="_blank"
                rel="noreferrer"
                className="border-border bg-background hover:bg-muted inline-flex items-center justify-center rounded-lg border px-8 py-4 text-base font-semibold shadow-sm transition-colors"
              >
                View on GitHub
              </a>
            </div>

            <div className="border-border bg-background/50 mx-auto flex flex-wrap items-center justify-center gap-4 rounded-lg border px-6 py-3 backdrop-blur-sm sm:gap-6">
              <div className="flex items-center gap-2">
                <CheckIcon className="size-4 text-green-500" />
                <span className="text-foreground text-sm font-medium">MIT License</span>
              </div>
              <div className="flex items-center gap-2">
                <CheckIcon className="size-4 text-blue-500" />
                <span className="text-foreground text-sm font-medium">Open Source</span>
              </div>
              <div className="flex items-center gap-2">
                <CheckIcon className="size-4 text-purple-500" />
                <span className="text-foreground text-sm font-medium">Free to Use</span>
              </div>
            </div>
          </div>
        </div>
      </Container>
    </section>
  );
}

function ThemeToggleButton() {
  const { theme, setTheme } = useTheme();
  const [mounted, setMounted] = React.useState(false);

  React.useEffect(() => {
    setMounted(true);
  }, []);

  function handleToggle() {
    if (!mounted) {
      return;
    }

    if (theme === "dark") {
      setTheme("light");
    } else {
      setTheme("dark");
    }
  }

  return (
    <Button
      type="button"
      variant="ghost"
      size="icon-sm"
      className="border-border bg-background/80 shadow-sm backdrop-blur"
      onClick={handleToggle}
      aria-label="Toggle theme"
    >
      {mounted ?
        theme === "dark" ?
          <SunIcon className="size-4" />
        : <MoonIcon className="size-4" />
      : <span className="bg-muted block h-4 w-4 rounded-full" />}
    </Button>
  );
}
