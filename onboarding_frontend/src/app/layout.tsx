"use client";
import Navbar from "./components/NavBar";
import "./globals.css";
import { ImportProvider } from "./components/ImportContext";
import { UploadProvider } from "./components/UploadContext";
import StepperBar from "./components/StepperBar";
import { MappingProvider } from "./components/MappingContext";
import 'primereact/resources/themes/lara-light-indigo/theme.css';
import "primeicons/primeicons.css";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <ImportProvider>
      <UploadProvider>
        <MappingProvider>
          <html lang="en">
            <body className="bg-gradient-to-b from-[#54155C] to-[#AF554E] min-h-screen">
              <div className="flex ml-64">
                <StepperBar />
                <div className="flex-1">
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
