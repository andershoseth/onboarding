"use client";
import { ChangeEvent, useState } from "react";
import { useUploadContext } from "../components/UploadContext";

export default function FileUploader() {
  const { setUploadedData } = useUploadContext();
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);
  const [uploadProgress, setUploadProgress] = useState(0);

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

        setTimeout(() => {
          setUploadProgress(progress);
        }, 200 * progress);
      }
    };

    xhr.onload = () => {
      setTimeout(() => {
        if (xhr.status === 200) {
          const response = JSON.parse(xhr.responseText);
          setUploadResponse({ message: "Upload successful", fileName: selectedFile.name });
          setUploadedData(response);
          setUploadProgress(100);
        } else {
          console.error("Error uploading the file:", xhr.statusText);
        }
      }, 2000);
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
          disabled={!selectedFile} // Disable button if no file is selected
        >
          Upload
        </button>
      </div>

      {/* Always visible progress bar */}
      <div className="mt-4">
        <progress value={uploadProgress} max="100" className="w-full" />
        <span>{uploadProgress}%</span>
      </div>

      {uploadResponse && (
        <div className="mt-4">
          <p>{uploadResponse.message}</p>
          <p>File Name: {uploadResponse.fileName}</p>
        </div>
      )}
    </div>
  );
}