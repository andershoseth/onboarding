"use client";
import { ChangeEvent, useState } from "react";
import { useUploadContext } from "../components/UploadContext"; // Importer kontekst


export default function FileUploader() {
  const {setUploadedData } = useUploadContext(); // Hent kontekst
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setSelectedFile(e.target.files[0]);
    }
  };

  const uploadFile = async () => {
    if (!selectedFile) return;
  
    const formData = new FormData();
    formData.append("file", selectedFile);
  
    try {
      const res = await fetch("http://localhost:5116/api/upload", {
        method: "POST",
        body: formData,
      });
  
      if (!res.ok) {
        throw new Error(`Upload failed with status ${res.status}`);
      }
      
      const data = await res.json();  // Motta Flattened JSON fra backend
      setUploadResponse({ message: "Upload successful", fileName: selectedFile.name });
      setUploadedData(data); 
  
      console.log("✅ Flattened Data:", data);
      console.log("✅ Context updated - uploadedData should now contain the API response.");
      
    } catch (error) {
      console.error("❌ Error uploading file:", error);
    }
  };
  return (
    <div>
      <input type="file" onChange={handleFileChange} />
      <button className="bg-white text-black px-4 py-2 rounded hover:bg-gray-800" onClick={uploadFile}>
        Upload
      </button>

      {uploadResponse && (
        <div>
          <p>{uploadResponse.message}</p>
          <p>File Name: {uploadResponse.fileName}</p>
        </div>
      )}
    </div>
  );
}



/*'use client'
// components/FileUploader.tsx
import { ChangeEvent, useState } from "react";

export default function FileUploader() {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setSelectedFile(e.target.files[0]);
    }
  };

  const uploadFile = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile);

    try {
      const res = await fetch("http://localhost:5116/api/upload", {
        method: "POST",
        body: formData,
      });

      interface UploadResponse {
        message: string;
        fileName: string;
      }

      const data: UploadResponse = await res.json();
      setUploadResponse(data);
      console.log(data);
    } catch (error) {
      console.error("Error uploading file:", error);
    }
  };

  return (
    <>
    <div>
      <input type="file" onChange={handleFileChange} />
      <button className="bg-white text-black px-4 py-2 rounded hover:bg-gray-800" onClick={uploadFile}>Upload</button>
    </div>
    <div>
      {uploadResponse && <div>{uploadResponse.message}<br/>{uploadResponse.fileName}</div>}
    </div>
    </>
  );
}*/