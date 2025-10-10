function Root(props: React.PropsWithChildren) {
  return props.children;
}

function Header(props: React.PropsWithChildren) {
  return <header className="flex items-center justify-between py-4">{props.children}</header>;
}

function Content(props: React.PropsWithChildren) {
  return <main className="overflow-hidden rounded-md border">{props.children}</main>;
}

function Footer(props: React.PropsWithChildren) {
  return <footer className="flex items-center justify-end space-x-2 py-4">{props.children}</footer>;
}

export const TableContainer = Object.assign(Root, {
  Header,
  Content,
  Footer,
});
