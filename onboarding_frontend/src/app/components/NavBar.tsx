"use client";
import { useState, useEffect } from "react";
import Link from "next/link";
import Image from "next/image";

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
        ${scrolled ? "bg-[rgba(0,0,0,0.8)]" : "bg-[rgba(0,0,0,0.2)]"}`
            }
        >
            <ul className="flex space-x-7 space-y-1 text-xl">
                <li>
                    <Link href="/"> <Image src="/po-logo.png" alt="logo" width={165} height={165} /> </Link>
                </li>
                <li>
                    <Link href="/about" className="hover:underline">Info</Link>
                </li>
                <li>
                    <Link href="/success" className="hover:underline">Suksess</Link>
                </li>
            </ul>
        </nav>
    );
}
