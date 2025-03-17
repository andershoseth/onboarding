// FileUploader.tsx
"use client";
import { ChangeEvent, useState } from "react";
import { useUploadContext } from "../components/UploadContext";

interface FileUploaderProps {
  subject: string;    // e.g. "kontakter", "safTExport"
  accept: string;     // e.g. ".xml" or ".csv,.xlsx"
}

export default function FileUploader({ subject, accept }: FileUploaderProps) {
  const {
    setUploadedData,
    uploadProgress,
    setUploadProgress,
    setUploadedFiles
  } = useUploadContext();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      const file = e.target.files[0];
      // If you want to do a manual extension check:
      const allowedExtensions = accept
          .split(",")
          .map((x) => x.trim().toLowerCase())
          .filter(Boolean); // e.g. [".xml", ".csv", ".xlsx"]

      // If the user wants to rely on the native file dialog filtering only,
      // you can skip this check. But here's an example:
      const lowerName = file.name.toLowerCase();
      const isValid = allowedExtensions.some((ext) => lowerName.endsWith(ext));
      if (!isValid && allowedExtensions.length > 0) {
        alert(`Invalid file type. Allowed types are: ${accept}`);
        return;
      }

      setSelectedFile(file);
      setUploadProgress(0);
      setUploadResponse(null);
    }
  };

  const uploadFile = () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile);
    formData.append("subject", subject);

    const xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5116/api/upload", true);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        const progress = Math.round((event.loaded / event.total) * 100);
        setUploadProgress(progress);
      }
    };

    xhr.onload = () => {
      if (xhr.status === 200) {
        const response = JSON.parse(xhr.responseText);
        setUploadResponse({ message: "Upload successful", fileName: selectedFile.name });
        setUploadedData(response);

        // Update multi-file dictionary
        const { subject, data } = response;
        setUploadedFiles((prev) => ({
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
          {/* Use the accept prop here */}
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
