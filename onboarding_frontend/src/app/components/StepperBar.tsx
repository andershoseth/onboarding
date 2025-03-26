"use client";
import React, { useContext, useEffect, useState } from "react";
import ImportContext from "../components/ImportContext";
import { useUploadContext } from "@/app/components/UploadContext";
import { Stepper } from "primereact/stepper";
import { StepperPanel } from "primereact/stepperpanel";
import { usePathname } from "next/navigation";

const Sidebar: React.FC = () => {
  const pathname = usePathname(); //må ha path for å gå til neste steg
  const { selectedSystem, fileName, selectedColumns, selectedFileType } = useContext(ImportContext);
  const { uploadedFiles } = useUploadContext();

  const isColumnsSelected = Object.values(selectedColumns).some(Boolean);
  const isExportUploaded = Object.keys(uploadedFiles).length > 0;

  //liste over sider (vurder å fjern completed senere hvis mulig)
  const steps = [
    { label: "Systemvalg", url: "/systemvalg", completed: selectedSystem !== null },
    { label: "Filtype", url: "/filtype", completed: selectedFileType !== null },
    { label: "Importvelger", url: "/importvelger", completed: isColumnsSelected },
    { label: "Eksport", url: "/export", completed: isExportUploaded },
    { label: "Last opp", url: "/upload", completed: fileName !== null },
  ];

  const activeStepIndex = steps.findIndex((step) => step.url === pathname);

  //holder følge på hvilket trinn
  const [currentStep, setCurrentStep] = useState(activeStepIndex !== -1 ? activeStepIndex : 0);
  useEffect(() => {
    setCurrentStep(activeStepIndex !== -1 ? activeStepIndex : 0);
  }, [pathname]);

  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-gray-800 text-white pt-10 flex flex-col items-start pl-6">
      <Stepper activeStep={currentStep} orientation="vertical">
        {steps.map((step, index) => (
          <StepperPanel
            key={index}
            header={step.label}>
          </StepperPanel>
        ))}
      </Stepper>
    </div>
  );
};

export default Sidebar;