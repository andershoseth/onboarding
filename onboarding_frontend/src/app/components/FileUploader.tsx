'use client'
// components/FileUploader.tsx
import { ChangeEvent, useState } from "react";

export default function FileUploader() {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);
  const [uploadProgress, setUpladProgress] = useState(0);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setSelectedFile(e.target.files[0]);
    }
  };

  const uploadFile = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile);

    const res = await fetch("http://localhost:5116/api/upload", {
      method: "POST",
      body: formData,
    });

    if (res.ok) {
      const data = await res.json();
      setUploadResponse(data);
      console.log(data);
    } else {
      console.error("Error ulpoading the file:", res.statusText);
    }
  };

  const uploadFileWithProgress = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append("file", selectedFile)

    const xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5116/api/upload", true);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        const progress = Math.round((event.loaded / event.total) * 100);
        setUpladProgress(progress);
      }
    };

    xhr.onload = () => {
      if (xhr.status === 200) {
        const response = JSON.parse(xhr.responseText);
        setUploadResponse(response)
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
    <>
      <div>
        <input type="file" onChange={handleFileChange} />
        <button className="bg-white text-black px-4 py-2 rounded hover:bg-[#c85b34]" onClick={uploadFileWithProgress}>Upload</button>
      </div>
      <div className="w-full bg-gray-200 h-2 mt-4">
        <div className="bg-green-500 h-2" style={{ width: `${uploadProgress}%` }}>
        </div>
      </div>
      {uploadProgress > 0 && <div> {uploadProgress}%</div>}
      <div>
        {uploadResponse && <div>{uploadResponse.message}<br />{uploadResponse.fileName}</div>}
      </div>
    </>
  );
}