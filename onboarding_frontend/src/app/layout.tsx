import Navbar from "./components/NavBar";
import "./globals.css";
import React from "react";
import { ImportProvider } from "./components/ImportContext";
import { UploadProvider } from "./components/UploadContext"; 

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
   
    <UploadProvider> 
    <ImportProvider>
    <html lang="en">
      <body className="bg-gradient-to-b from-[#54155C] to-[#AF554E] min-h-screen">
        <Navbar />
        <main className="p-4">{children}</main>
      </body>
    </html>
    </ImportProvider>
    </UploadProvider> 
   
  );
}
