import Link from "next/link";

export default function Navbar() {
  return (
    <nav className="p-4 text-white shadow-md">
      <ul className="flex space-x-4">
        <li>
          <Link href="/" className="hover:underline">Home</Link>
        </li>
        <li>
          <Link href="/about" className="hover:underline">About</Link>
        </li>
      </ul>
    </nav>
  );
}
