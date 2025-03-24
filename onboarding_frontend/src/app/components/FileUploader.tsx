import React from "react";
import { FileUpload } from "primereact/fileupload";

import { useUploadContext } from "./UploadContext";
import ImportContext from "./ImportContext";


interface FileUploaderProps {
  subject: string;
  accept: string;
}

export default function FileUploader({ subject, accept }: FileUploaderProps) {
  const { setUploadedData, setUploadedFiles } = useUploadContext();

  const handleUploadComplete = (e: any) => {
    try {
      const response = JSON.parse(e.xhr.response);
      console.log("Server Response Data:", response.data);
      console.log("Type of First Entry:", typeof response.data?.[0]);
      console.log("Structure of First Entry:", response.data?.[0]);

      setUploadedData(response.data);
      setUploadedFiles(prev => ({
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

  const handleFileRemove = (e: any) => {
    setUploadedFiles(prev => {
      const updated = { ...prev };
      delete updated[subject];
      return updated;
    });
  }

  return (
    <FileUpload
      name="file"
      url="http://localhost:5116/api/upload"
      multiple={false}
      mode="advanced"
      accept={accept}
      onBeforeUpload={(event) => {
        // Adds subject to the form data
        event.formData.append("subject", subject);
      }}
      onUpload={handleUploadComplete}
      onError={handleUploadError}
      onRemove={handleFileRemove}
      chooseLabel="Choose"
      uploadLabel="Upload"
      cancelLabel="Cancel"
    />
  );
}
