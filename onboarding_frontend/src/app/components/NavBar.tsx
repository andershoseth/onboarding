"use client";
import { useState, useEffect } from "react";
import Link from "next/link";

export default function Navbar() {
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 10) {
        setScrolled(true);
      } else {
        setScrolled(false);
      }
    };

    //Listening to the scroll event here
    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <nav
      className={`p-2 font-bold text-white transition-all duration-300 ${scrolled
        ? "bg-[rgba(0,0,0,0.6)]" // Semi-transparent black background to prevent navbar opaqueness rfom covering the text
        : "bg-transparent"
        } z-50 fixed w-full top-0`}
    >
      <ul className="flex space-x-4">
        <li>
          <Link href="/home" className="hover:underline">Home</Link>
        </li>
        <li>
          <Link href="/about" className="hover:underline">About</Link>
        </li>
      </ul>
    </nav>
  );
}
