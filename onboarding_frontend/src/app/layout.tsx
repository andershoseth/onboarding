"use client";
import Navbar from "./components/NavBar";
import "./globals.css";
import React from "react";
import { ImportProvider } from "./components/ImportContext";
import { UploadProvider } from "./components/UploadContext"; 
import StepperBar from "./components/StepperBar"
import Sidebar from "./components/StepperBar";
import { usePathname } from "next/navigation";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();

  const pagesWithStepperBar = ["/systemvalg", "/importvelger", "/upload", "/export", "upload"]

  const showStepperBar = pagesWithStepperBar.includes(pathname || '')
  
  return (
   
    <UploadProvider> 
    <ImportProvider>
    <html lang="en">
      <body className="bg-gradient-to-b from-[#54155C] to-[#AF554E] min-h-screen">
        <div className="flex">
           {showStepperBar && <StepperBar/>}        
           <div className={`flex-1 ${showStepperBar ? 'ml-64' : ''}`}>
              <Navbar />
              <main>{children}</main>
            </div>
          </div>
      </body>
    </html>
    </ImportProvider>
    </UploadProvider> 
  );
}
