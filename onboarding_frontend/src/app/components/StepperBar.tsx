"use client";
import React, { useContext, useEffect, useState } from "react";
import { useUploadContext } from "@/app/components/UploadContext";
import { Stepper } from "primereact/stepper";
import { StepperPanel } from "primereact/stepperpanel";
import { usePathname } from "next/navigation";
import ImportContext from "../components/ImportContext";
import Link from "next/link";
import Image from "next/image";

// function that returns steps array
export const getSteps = (
  selectedSystem: any,
  selectedColumns: any,
  selectedFileType: any,
  uploadedFiles: any,
  mappingCompleted: boolean
) => {
  const isColumnsSelected = Object.values(selectedColumns).some(Boolean);
  const isExportUploaded = Object.keys(uploadedFiles).length > 0;

  //liste over sider
  return [
    { label: "Systemvalg", url: "/systemvalg", description: "Velg system", completed: selectedSystem !== null },
    { label: "Filtype", url: "/filtype", description: "Velg filtype", completed: selectedFileType !== null },
    { label: "Importvelger", url: "/importvelger", description: "Velg hva du vil laste opp", completed: isColumnsSelected },
    { label: "Eksport", url: "/export", description: "Last opp filene dine", completed: isExportUploaded },
    { label: "Forhåndsvisning", url: "/displaycsvexcel", description: "Se filene som er lastet opp", completed: isExportUploaded },
    { label: "Mål", url: "/success", completed: mappingCompleted },
  ];
};

const StepperBar: React.FC = () => {
  const pathname = usePathname();
  const { selectedSystem, selectedColumns, selectedFileType, mappingCompleted } = useContext(ImportContext);
  const { uploadedFiles } = useUploadContext();

  const steps = getSteps(selectedSystem, selectedColumns, selectedFileType, uploadedFiles, mappingCompleted);

  const activeStepIndex = steps.findIndex((step) => step.url === pathname);

  //holder følge på hvilken side du er på
  const [currentStep, setCurrentStep] = useState(activeStepIndex !== -1 ? activeStepIndex : 0);
  useEffect(() => {
    setCurrentStep(activeStepIndex !== -1 ? activeStepIndex : 0);
  }, [pathname]);

  return (
    <div className="fixed top-0 left-0 w-64 bg-white h-full flex flex-col justify-between">
      <div className="pt-10 pl-1">
        <Stepper activeStep={currentStep} orientation="vertical">
          {steps.map((step) => (
            <StepperPanel key={step.url} header={`${step.label} ${step.completed ? "✔" : ""}`}>
              <Link
                href={step.url}
                className="text-black hover:text-gray-300 text-left rounded w-full font-link">
                {step.description}
              </Link>
            </StepperPanel>
          ))}
        </Stepper>
      </div>

      {/* PowerOffice support logo*/}
      <div className="w-full absolute bottom-10 pb-10 pr-5">
        <Link href="https://support.poweroffice.com/hc/no" target="_blank" rel="noopener noreferreer">
          <div className="flex items-center justify-center ">
            <Image
              src="/po_support_logo.png"
              alt="PowerOffice support logo"
              width={150}
              height={50}
              className="ml-[2px]" />
            <Image
              src="/open_new_window.png"
              alt="PowerOffice support logo"
              width={12}
              height={12}
              className="ml-[5px]" />
          </div>
        </Link>
      </div>
    </div >
  );
};

export default StepperBar;