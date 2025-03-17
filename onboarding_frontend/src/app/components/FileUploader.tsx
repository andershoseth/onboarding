// app/components/FileUploader.tsx
"use client";
import React, { ChangeEvent, useState } from "react";
import { useUploadContext } from "./UploadContext";

interface FileUploaderProps {
  subject: string;        // e.g. "kontakter", "safTExport", etc.
  accept: string;         // e.g. ".xml" or ".csv,.xlsx"
}

export default function FileUploader({ subject, accept }: FileUploaderProps) {
  const {
    uploadProgress,
    setUploadProgress,
    uploadedData,     // old single-file approach if you still need it
    setUploadedData,
    uploadedFiles,
    setUploadedFiles
  } = useUploadContext();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      const file = e.target.files[0];

      // optional extra check:
      const allowedExtensions = accept
          .split(",")
          .map(x => x.trim().toLowerCase())
          .filter(Boolean);

      if (allowedExtensions.length > 0) {
        const fileNameLower = file.name.toLowerCase();
        const isValid = allowedExtensions.some(ext => fileNameLower.endsWith(ext));
        if (!isValid) {
          alert(`Invalid file type. Allowed: ${accept}`);
          return;
        }
      }
      setSelectedFile(file);
      setUploadProgress(0);
      setUploadResponse(null);
    }
  };

  const uploadFile = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile);
    formData.append("subject", subject);

    const xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5116/api/upload", true);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        setUploadProgress(Math.round((event.loaded / event.total) * 100));
      }
    };

    xhr.onload = () => {
      if (xhr.status === 200) {
        // parse the JSON
        const response = JSON.parse(xhr.responseText);
        setUploadResponse({ message: "Upload successful", fileName: selectedFile.name });

        // old single-file approach if you still want to store it:
        setUploadedData(response);

        // store in new multi-file approach:
        const { subject, data } = response;
        setUploadedFiles(prev => ({
          ...prev,
          [subject]: {
            fileName: selectedFile.name,
            data
          }
        }));

        setUploadProgress(100);
      } else {
        console.error("Error uploading the file:", xhr.statusText);
      }
    };

    xhr.onerror = (error) => {
      console.error("Error uploading file:", error);
    };

    xhr.send(formData);
  };

  return (
      <div className="space-y-4">
        <div>
          <input type="file" accept={accept} onChange={handleFileChange} />
          <button
              className="bg-white text-black px-4 py-2 rounded hover:bg-gray-800 ml-2"
              onClick={uploadFile}
              disabled={!selectedFile}
          >
            Upload
          </button>
        </div>

        {uploadProgress > 0 && (
            <div className="w-full bg-gray-200 h-6 rounded-full overflow-hidden mt-4 relative">
              <div
                  className="bg-blue-500 h-6 rounded-full transition-all duration-500 flex items-center justify-center text-white text-sm font-bold"
                  style={{ width: `${uploadProgress}%` }}
              >
                {uploadProgress}%
              </div>
            </div>
        )}

        {uploadResponse && (
            <div className="mt-4 text-center">
              <p>{uploadResponse.message}</p>
              <p>File Name: {uploadResponse.fileName}</p>
            </div>
        )}
      </div>
  );
}
