'use client'
import { useState } from "react";

export default function FileUploader() {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
<<<<<<< HEAD
  const [uploadResult, setUploadResult] = useState<unknown>(null);
=======
  const [uploadResponse, setUploadResponse] = useState<{ message: string; fileName: string } | null>(null);
>>>>>>> origin/main

  function handleFileChange(e: React.ChangeEvent<HTMLInputElement>) {
    if (!e.target.files) return;
    setSelectedFile(e.target.files[0]);
  }

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
        throw new Error(`Error! status: ${res.status}`);
      }

<<<<<<< HEAD
      const data = await res.json();
      setUploadResult(data); // ⬅ Lagrer responsen i state

=======
      const data: UploadResponse = await res.json();
      setUploadResponse(data);
      console.log(data);
>>>>>>> origin/main
    } catch (error) {
      console.error("Error uploading file:", error);
    }
  };

  return (
    <>
    <div>
      <h1>File Uploader</h1>

      <input type="file" onChange={handleFileChange} />
<<<<<<< HEAD
      <button onClick={uploadFile}>Upload</button>

      {/* Viser JSON på nettsiden i en PRE-blokk */}
      {uploadResult && (
        <div style={{ marginTop: "1rem" }}>
          <h2>Upload Result:</h2>
          <pre>{JSON.stringify(uploadResult, null, 2)}</pre>
        </div>
      )}
=======
      <button className="bg-white text-black px-4 py-2 rounded hover:bg-gray-800" onClick={uploadFile}>Upload</button>
>>>>>>> origin/main
    </div>
    <div>
      {uploadResponse && <div>{uploadResponse.message}<br/>{uploadResponse.fileName}</div>}
    </div>
    </>
  );
}
