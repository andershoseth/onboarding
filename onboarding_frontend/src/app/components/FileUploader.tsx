import React, { useContext, useEffect } from "react";
import { FileUpload } from "primereact/fileupload";
import { useUploadContext } from "./UploadContext";
import ImportContext from "./ImportContext";

interface FileUploaderProps {
  subject: string;
  accept: string;
}

export default function FileUploader({ subject, accept }: FileUploaderProps) {
  const { setUploadedData, setUploadedFiles, uploadedFiles } = useUploadContext();
  const { setFileName } = useContext(ImportContext)

  const handleUploadComplete = (e: any) => {
    try {
      const response = JSON.parse(e.xhr.response);
      const uploadedFileName = e.files[0]?.name ?? "unknown"

      setFileName((prev) => [...prev, uploadedFileName]);
      setUploadedData(response.data);
      setUploadedFiles((prev) => ({
        ...prev,
        [response.subject]: {
          fileName: e.files[0]?.name ?? "unknown",
          data: response.data,
        },
      }));
    } catch (err) {
      console.error("Error parsing server response:", err);
    }
  };

  const handleUploadError = (e: any) => {
    console.error("Upload error:", e.xhr);
  };

  // appends the subject to form data before uploading
  const handleBeforeUpload = (event: any) => {
    event.formData.append("subject", subject);
  };

  const handleFileRemove = (e: any) => {
    const removeFileName = uploadedFiles[subject].fileName //får tak i navnet på fila som skal slettes

    setUploadedFiles((prev) => {
      const updated = { ...prev };
      delete updated[subject];
      return updated;
    });

    setFileName((prev) => prev.filter((name) => name !== removeFileName)); //fjerner valgt fil fra fileName-lista
  };

  return (
    <FileUpload
      name="file"
      url="http://localhost:5116/api/upload"
      multiple={true}
      mode="advanced"
      auto={false}
      accept={accept}
      onBeforeUpload={handleBeforeUpload}
      onUpload={handleUploadComplete}
      onError={handleUploadError}
      onRemove={handleFileRemove}

      chooseLabel="Choose File(s)"
      uploadLabel="Upload"
      cancelLabel="Clear"
    />
  );
}
