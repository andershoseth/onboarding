"use client";
import React, { useContext, useEffect, useState } from "react";
import { useUploadContext } from "@/app/components/UploadContext";
import { Stepper } from "primereact/stepper";
import { StepperPanel } from "primereact/stepperpanel";
import { usePathname, useRouter } from "next/navigation";
import ImportContext from "../components/ImportContext";

const Sidebar: React.FC = () => {
  const pathname = usePathname(); //må ha path for å gå til neste steg
  const router = useRouter(); //navigering til neste side
  const { selectedSystem, selectedColumns, selectedFileType } = useContext(ImportContext);
  const { uploadedFiles } = useUploadContext();

  const isColumnsSelected = Object.values(selectedColumns).some(Boolean);
  const isExportUploaded = Object.keys(uploadedFiles).length > 0;

  //liste over sider (vurder å fjern completed senere hvis mulig)
  const steps = [
    { label: "Systemvalg", url: "/systemvalg", description: "Velg system", completed: selectedSystem !== null },
    { label: "Filtype", url: "/filtype", description: "Velg filtype", completed: selectedFileType !== null },
    { label: "Importvelger", url: "/importvelger", description: "Velg hva du vil laste opp", completed: isColumnsSelected },
    { label: "Eksport", url: "/export", description: "Last opp filene dine", completed: isExportUploaded },
  ];

  const activeStepIndex = steps.findIndex((step) => step.url === pathname);

  //holder følge på hvilket trinn
  const [currentStep, setCurrentStep] = useState(activeStepIndex !== -1 ? activeStepIndex : 0);
  useEffect(() => {
    setCurrentStep(activeStepIndex !== -1 ? activeStepIndex : 0);
  }, [pathname]);

  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-white text-black pt-10  items-start pl-1">
      <Stepper activeStep={currentStep} orientation="vertical">
        {steps.map((step, index) => (
          <StepperPanel key={index} header={step.label}>
            <button
              onClick={() => router.push(step.url)}
              className="text-black hover:text-gray-300 text-left w-full">
              {step.description}
            </button>
          </StepperPanel>
        ))}
      </Stepper>
    </div>
  );
};

export default Sidebar;