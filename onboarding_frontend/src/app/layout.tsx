"use client";
import Navbar from "./components/NavBar";
import "./globals.css";
import React, { useEffect, useState } from "react";
import { ImportProvider } from "./components/ImportContext";
import { UploadProvider } from "./components/UploadContext";
import StepperBar from "./components/StepperBar";  // Import your new StepperBar component
import { usePathname } from "next/navigation";
import { MappingProvider } from "./components/MappingContext";
import 'primereact/resources/themes/lara-light-indigo/theme.css';
import "primeicons/primeicons.css";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const [showStepperBar, setShowStepperBar] = useState(false);

  const isStepperPage = [
    "/systemvalg", "/importvelger", "/export", "/filtype", "/displaycsvexcel"
  ].includes(pathname);

  return (
    <ImportProvider>
      <UploadProvider>
        <MappingProvider>
          <html lang="en">
            <body className="bg-gradient-to-b from-[#54155C] to-[#AF554E] min-h-screen">
              <div className="flex">
                {isStepperPage && <StepperBar />}
                <div className={`flex-1 ${isStepperPage ? 'ml-64' : ''}`}>
                  <Navbar />
                  <main>{children}</main>
                </div>
              </div>
            </body>
          </html>
        </MappingProvider>
      </UploadProvider>
    </ImportProvider>
  );
}