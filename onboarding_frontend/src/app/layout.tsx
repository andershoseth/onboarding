"use client";
import Navbar from "./components/NavBar";
import "./globals.css";
import { ImportProvider } from "./components/ImportContext";
import { UploadProvider } from "./components/UploadContext";
import StepperBar from "./components/StepperBar";
import { MappingProvider } from "./components/MappingContext";
import 'primereact/resources/themes/lara-light-indigo/theme.css';
import "primeicons/primeicons.css";
import { usePathname } from "next/navigation";  // Import usePathname to check the current URL

export default function RootLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname(); // Get the current pathname

  const showStepper = !(pathname === "/SaftTable" || pathname === "/home"); //skjuler stepper på hjemmesiden og saft-siden

  return (
    <ImportProvider>
      <UploadProvider>
        <MappingProvider>
          <html lang="en">
            <body className="min-h-screen"
              style={{ background: "linear-gradient(13.92deg, #FAB270 2.22%, #984798 49.7%, #422163 97.18%)" }}>
              <div className="flex">
                {showStepper && <StepperBar />}
                <div className={`flex-1 ${showStepper ? 'ml-64' : ''} overflow-x-auto`}>
                  <Navbar />
                  <main className="pt-[30px]">{children}</main>
                </div>
              </div>
            </body>
          </html>
        </MappingProvider>
      </UploadProvider>
    </ImportProvider>
  );
}