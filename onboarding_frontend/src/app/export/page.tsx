"use client"
import React from "react";
import Instructions from "../components/Instructions";
import ImportContext from '../components/ImportContext';
import { useContext, useEffect, useState } from 'react';


const ExportInstructions = () => {
  const { selectedSystem } = useContext(ImportContext);
  const [hasMounted, setHasMounted] = useState(false);
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]); //array for the checked boxes
  const fileType = "SAF-T"; // To be gotten later from context, can be SAF-T or .csv/.xslx 


  useEffect(() => {
    setHasMounted(true);

    const savedCheckBoxes = localStorage.getItem("checkboxState"); //saves the checked boxes in the array
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels)
    }

  }, []);

  if (!hasMounted) {
    return null;
  }


    return (
        <>
        <div>
            <p>
                {selectedSystem ? `You selected ${selectedSystem}` : "No system selected"}
            </p>
            <p>
                {`You selected: ${checkedBoxes.join(", ")}`} {/* simple string to display the boxes checked */}
            </p>cr
        </div>
        <div className="min-h-screen py-10">
          <Instructions
            title="Utklipp av Hovedbokstransaksjoner"
            steps={[
              {
                heading: 'Åpne riktig kunde',
                text: 'Stå i Explorer vinduet i Visma Business, trykk på valgt kunde. ' +
                      'Utvid Tabeler og Regnskap.',
                imageSrc: '/export/placeholderinstruction_1.png',
                imageAlt: 'Visma instructions screenshot 1'
              },
              {
                heading: 'Klikk på Hovedbok',
                imageSrc: '/export/placeholderinstruction_2.png',
              },
              {
                heading: 'Kontroller kolonneoppsett',
                text: 'Dette kan variere fra versjon til versjon i Visma Business. ' +
                      'Det er viktig at oppsettet er som beskrevet.',
                imageSrc: '/export/placeholderinstruction_3.png',
              },
                {
                    heading: 'Kopier data',
                    text: 'Marker alle rader i hovedboken, høyreklikk og velg Kopier. ' +
                        'Lim inn i Excel.',
                    imageSrc: '/export/placeholderinstruction_1.png',
                },
                {
                    heading: 'Lagre Excel-filen',
                    text: 'Lagre filen som en CSV-fil.',
                    imageSrc: '/export/placeholderinstruction_1.png',
                    imageAlt: ''
                },
                {
                    heading: 'Last opp filen',
                    text: 'Last opp filen i skjemaet.',
                    imageSrc: '/export/placeholderinstruction_100.png',
                    imageAlt: 'Step 5 alttext'
                }
            ]}
          />
        </div>
        </>
      );
};

export default ExportInstructions;