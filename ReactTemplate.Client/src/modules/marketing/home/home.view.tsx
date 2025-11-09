import { Container } from "@/components/container";

export function HomeView() {
  return (
    <div>
      <Container className="border-b">
        <header className="flex flex-col gap-4 border-x p-3 md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-xl">
              <code>Core(stack)</code>
            </h1>
            <p className="text-muted-foreground text-sm md:text-base">
              Full-stack starter for ASP.NET Core 9 and React 19.2 so you can launch faster.
            </p>
          </div>
          <span className="text-muted-foreground text-xs tracking-wide uppercase">
            Production ready
          </span>
        </header>
      </Container>

      <Container className="border-b">
        <section className="border-x px-3 py-12">
          <div className="grid gap-8 md:grid-cols-[minmax(0,2fr),minmax(0,1fr)] md:items-center">
            <div className="space-y-5">
              <h2 className="text-3xl font-semibold md:text-4xl">
                Launch production-grade apps without the boilerplate.
              </h2>
              <p className="text-muted-foreground text-base">
                Core(stack) ships a ready-to-run backend, modern React front-end, and batteries
                included authentication so your team can focus on product outcomes instead of
                plumbing.
              </p>
              <div className="flex flex-wrap gap-3">
                <a
                  className="border-foreground hover:bg-foreground hover:text-background inline-flex items-center justify-center rounded border px-4 py-2 text-sm font-medium transition"
                  href="https://github.com/t-rosa/ReactTemplate"
                  rel="noreferrer"
                  target="_blank"
                >
                  Explore the template
                </a>
                <a
                  className="bg-foreground text-background inline-flex items-center justify-center rounded px-4 py-2 text-sm font-medium transition hover:opacity-90"
                  href="#quickstart"
                >
                  Start in 5 minutes
                </a>
              </div>
            </div>
            <dl className="grid gap-4 sm:grid-cols-2">
              <div className="rounded border p-4">
                <dt className="text-muted-foreground text-xs tracking-wide uppercase">API</dt>
                <dd className="text-lg font-semibold">ASP.NET Core 9</dd>
                <dd className="text-muted-foreground text-sm">
                  Identity, EF Core, FluentValidation
                </dd>
              </div>
              <div className="rounded border p-4">
                <dt className="text-muted-foreground text-xs tracking-wide uppercase">Front-end</dt>
                <dd className="text-lg font-semibold">React 19.2 + Vite</dd>
                <dd className="text-muted-foreground text-sm">TanStack, Tailwind, shadcn/ui</dd>
              </div>
              <div className="rounded border p-4">
                <dt className="text-muted-foreground text-xs tracking-wide uppercase">Testing</dt>
                <dd className="text-lg font-semibold">xUnit &amp; Playwright</dd>
                <dd className="text-muted-foreground text-sm">
                  Unit, API, and e2e coverage baked in
                </dd>
              </div>
              <div className="rounded border p-4">
                <dt className="text-muted-foreground text-xs tracking-wide uppercase">DevOps</dt>
                <dd className="text-lg font-semibold">Docker ready</dd>
                <dd className="text-muted-foreground text-sm">
                  Preview and production container workflows
                </dd>
              </div>
            </dl>
          </div>
        </section>
      </Container>

      <Container className="border-b">
        <section className="border-x px-3 py-12" id="features">
          <div className="flex items-center justify-between gap-3 pr-3">
            <code className="block w-fit border-r border-b px-4 py-2">01</code>
            <h2 className="text-lg font-semibold md:text-2xl">
              Production foundation out of the box
            </h2>
          </div>
          <div className="space-y-6 px-3 py-6 md:px-6">
            <p className="text-muted-foreground text-sm md:text-base">
              Everything in the template is wired for real workloads: a secure API, curated database
              setup, transactional email, and role-based access flows that work from day one.
            </p>
            <ul className="grid gap-4 sm:grid-cols-2">
              <li className="rounded border p-4">
                <h3 className="text-sm font-medium">Identity &amp; Auth</h3>
                <p className="text-muted-foreground text-sm">
                  ASP.NET Core Identity with ready-made auth flows, password reset, and seed users.
                </p>
              </li>
              <li className="rounded border p-4">
                <h3 className="text-sm font-medium">Data layer</h3>
                <p className="text-muted-foreground text-sm">
                  EF Core migrations plus PostgreSQL containers for local development and CI setups.
                </p>
              </li>
              <li className="rounded border p-4">
                <h3 className="text-sm font-medium">Email ready</h3>
                <p className="text-muted-foreground text-sm">
                  SMTP configuration, templated emails, and background queues already wired.
                </p>
              </li>
              <li className="rounded border p-4">
                <h3 className="text-sm font-medium">Validation pipeline</h3>
                <p className="text-muted-foreground text-sm">
                  FluentValidation policies wrap every request to keep contracts tight and
                  predictable.
                </p>
              </li>
            </ul>
          </div>
        </section>
      </Container>

      <Container className="border-b">
        <section className="border-x px-3 py-12" id="experience">
          <div className="flex items-center justify-between gap-3 pl-3">
            <h2 className="text-lg font-semibold md:text-2xl">
              Developer experience that scales with your team
            </h2>
            <code className="block w-fit border-b border-l px-4 py-2">02</code>
          </div>
          <div className="space-y-6 px-3 py-6 md:px-6">
            <p className="text-muted-foreground text-sm md:text-base">
              Enjoy a cohesive front-end toolkit with sensible defaults: TanStack Router for
              routing, Query for data fetching, and a shadcn/ui design system with Tailwind for
              polish.
            </p>
            <div className="grid gap-4 md:grid-cols-3">
              <div className="rounded border p-4">
                <h3 className="text-sm font-medium">Type-safe APIs</h3>
                <p className="text-muted-foreground text-sm">
                  openapi-typescript keeps server contracts in sync with generated React hooks.
                </p>
              </div>
              <div className="rounded border p-4">
                <h3 className="text-sm font-medium">UI primitives</h3>
                <p className="text-muted-foreground text-sm">
                  Reusable components, containers, and layout helpers speed up marketing and app
                  pages.
                </p>
              </div>
              <div className="rounded border p-4">
                <h3 className="text-sm font-medium">Fast feedback</h3>
                <p className="text-muted-foreground text-sm">
                  Vitest, ESLint, and Playwright are preconfigured, offering confidence with each
                  commit.
                </p>
              </div>
            </div>
          </div>
        </section>
      </Container>

      <Container className="border-b">
        <section className="border-x px-3 py-12" id="quickstart">
          <div className="flex items-center justify-between gap-3 pr-3">
            <code className="block w-fit border-r border-b px-4 py-2">03</code>
            <h2 className="text-lg font-semibold md:text-2xl">
              Quickstart that gets you coding now
            </h2>
          </div>
          <div className="space-y-6 px-3 py-6 md:px-6">
            <p className="text-muted-foreground text-sm md:text-base">
              Follow the documented workflow below and have the full stack running locally in
              minutes.
            </p>
            <ol className="grid gap-4 md:grid-cols-3">
              <li className="rounded border p-4">
                <span className="text-muted-foreground text-xs tracking-wide uppercase">
                  Step 1
                </span>
                <p className="text-sm font-medium">Install the template</p>
                <pre className="bg-muted/50 mt-3 overflow-x-auto rounded border p-3 font-mono text-xs md:text-sm">
                  <code>dotnet new install .</code>
                </pre>
              </li>
              <li className="rounded border p-4">
                <span className="text-muted-foreground text-xs tracking-wide uppercase">
                  Step 2
                </span>
                <p className="text-sm font-medium">Configure secrets</p>
                <div className="mt-3 space-y-2">
                  {[
                    "dotnet user-secrets init --project ReactTemplate.Server",
                    'dotnet user-secrets set "SMTP_USERNAME" "value" --project ReactTemplate.Server',
                    'dotnet user-secrets set "SMTP_PASSWORD" "value" --project ReactTemplate.Server',
                    'dotnet user-secrets set "ADMIN_EMAIL" "value" --project ReactTemplate.Server',
                    'dotnet user-secrets set "ADMIN_PASSWORD" "value" --project ReactTemplate.Server',
                    'dotnet user-secrets set "CONNECTION_STRING" "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=react-template;" --project ReactTemplate.Server',
                  ].map((command) => (
                    <pre
                      key={command}
                      className="bg-muted/50 overflow-x-auto rounded border p-3 font-mono text-xs md:text-sm"
                    >
                      <code>{command}</code>
                    </pre>
                  ))}
                </div>
              </li>
              <li className="rounded border p-4">
                <span className="text-muted-foreground text-xs tracking-wide uppercase">
                  Step 3
                </span>
                <p className="text-sm font-medium">Run and validate</p>
                <div className="mt-3 space-y-2">
                  {[
                    "dotnet run --project ReactTemplate.Server",
                    "cd ReactTemplate.Client",
                    "npm install",
                    "npm run dev",
                    "npm run test",
                    "npx playwright test",
                  ].map((command) => (
                    <pre
                      key={command}
                      className="bg-muted/50 overflow-x-auto rounded border p-3 font-mono text-xs md:text-sm"
                    >
                      <code>{command}</code>
                    </pre>
                  ))}
                </div>
              </li>
            </ol>
          </div>
        </section>
      </Container>

      <Container>
        <footer className="text-muted-foreground border-x p-3 text-center text-sm">
          <p>
            Built with <span className="text-red-500">❤️</span> on ASP.NET Core 9, React 19.2, and
            the tools we trust every day.
          </p>
        </footer>
      </Container>
    </div>
  );
}
