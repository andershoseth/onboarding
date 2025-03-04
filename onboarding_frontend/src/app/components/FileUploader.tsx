"use client";
import { ChangeEvent, useState, useContext } from "react";
import { useUploadContext } from "../components/UploadContext";
import ImportContext from "./ImportContext";

export default function FileUploader() {
  const { setUploadedData, uploadProgress, setUploadProgress } = useUploadContext();
  const { setFileName } = useContext(ImportContext);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);


  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setSelectedFile(e.target.files[0]);
      setUploadProgress(0);
      setUploadResponse(null);
    }
  };

  const uploadFile = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile);


    const xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5116/api/upload", true);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        const progress = Math.round((event.loaded / event.total) * 100);
        setUploadProgress(progress)
      }
    };

    xhr.onload = () => {
      if (xhr.status === 200) {
        setTimeout(() => {
          const response = JSON.parse(xhr.responseText);
          setUploadResponse({ message: "Upload successful", fileName: selectedFile.name });
          setUploadedData(response);
          setFileName(selectedFile.name)
          console.log("Uploaded response:", response);
          setUploadProgress(100);
        }, 500);
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
        <input type="file" onChange={handleFileChange} />
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
          <div className="bg-blue-500 h-6 rounded-full transition-all duration-500 flex items-center justify-center text-white text-sm font-bold">
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