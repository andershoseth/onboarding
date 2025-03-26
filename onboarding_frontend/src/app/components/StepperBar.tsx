"use client";
import React, { useContext } from "react";
import ImportContext from "../components/ImportContext";
import { useUploadContext } from "@/app/components/UploadContext";
import { Stepper } from "primereact/stepper";
import { StepperPanel } from "primereact/stepperpanel";
import { useRouter } from "next/navigation";

const Sidebar: React.FC = () => {
  const router = useRouter();
  const { selectedSystem, fileName, selectedColumns, selectedFileType } = useContext(ImportContext);
  const { uploadedFiles } = useUploadContext();

  // Might change this, since the circle is not "completed" until an action is taken, NOT if anything is saved
  const isColumnsSelected = Object.values(selectedColumns).some(Boolean);

  // Just adding a variable to check if an uploaded file exists for the condition
  const isExportUploaded = Object.keys(uploadedFiles).length > 0;

  const steps = [
    { label: "Systemvalg", url: "/systemvalg", completed: selectedSystem !== null },
    { label: "Filtype", url: "/filtype", completed: selectedFileType !== null },
    { label: "Importvelger", url: "/importvelger", completed: isColumnsSelected },
    { label: "Eksport", url: "/export", completed: isExportUploaded },
    { label: "Last opp", url: "/upload", completed: fileName !== null },
  ];

  const activeindex = steps.findIndex((step) => !step.completed);
  const currentStep = activeindex === -1 ? steps.length - 1 : activeindex;

  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-gray-800 text-white pt-10 flex flex-col items-start pl-6">
      <Stepper activeStep={currentStep} orientation="vertical">
        {steps.map((step, index) => (
          <StepperPanel key={index} header={step.label}>
          </StepperPanel>
        ))}
      </Stepper>
    </div>
  );
};

export default Sidebar;
