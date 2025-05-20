    // components/Instructions.tsx

import React from 'react';
import Image from 'next/image';

interface InstructionItem {
  heading?: string;
  text?: string;
  imageSrc?: string;
  imageAlt?: string;
}

interface InstructionsProps {
  /** Main heading for the instructions block */
  title?: string;
  /** Array of steps (InstructionItem) to show */
  steps: InstructionItem[];
}

/**
 * Renders a list of instructions vertically.
 * Each step can include an optional heading, body text, and/or image.
 */
const Instructions: React.FC<InstructionsProps> = ({ title, steps }) => {
  return (
    <div className="max-w-3xl mx-auto p-4">
      {title && (
        <h1 className="text-2xl font-bold mb-6">
          {title}
        </h1>
      )}

      <ol className="space-y-8">
        {steps.map((step, index) => (
          <li key={index}>
            {step.heading && (
              <h2 className="text-xl font-semibold mb-2">
                {step.heading}
              </h2>
            )}

            {step.text && (
              <p className=" mb-4">
                {step.text}
              </p>
            )}

            {step.imageSrc && (
              <div className="w-full border rounded-lg overflow-hidden">
                <Image
                  src={step.imageSrc}
                  alt={step.imageAlt || `Step ${index + 1}`}
                  width={800} // Adjust as needed
                  height={450} // Adjust as needed
                  className="object-cover"
                />
              </div>
            )}
          </li>
        ))}
      </ol>
    </div>
  );
};

export default Instructions;
