"use client";
import {useState, useEffect} from "react";
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

        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
    }, []);

    return (
        <nav
            className={`p-2 font-bold text-white transition-all duration-300 fixed w-full top-0 z-50 
        ${scrolled ? "bg-[rgba(0,0,0,0.3)]" : "bg-[rgba(0,0,0,0.2)]"}`
            }
        >
            <ul className="flex space-x-4">
                <li>
                    <Link href="/home" className="hover:underline">Home</Link>
                </li>
                <li>
                    <Link href="/about" className="hover:underline">About</Link>
                </li>
                <li>
                    <Link href="/success" className="hover:underline">Success</Link>
                </li>
            </ul>
        </nav>
    );
}
