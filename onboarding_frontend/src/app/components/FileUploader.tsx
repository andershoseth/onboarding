import React, { useContext, useEffect, useRef } from "react";
import { FileUpload } from "primereact/fileupload";
import { useUploadContext } from "./UploadContext";
import ImportContext from "./ImportContext";

interface FileUploaderProps {
    subject: string;
    accept: string;
    onShowErrorToast?: (msg: string) => void;
}

export default function FileUploader({
                                         subject,
                                         accept,
                                         onShowErrorToast
                                     }: FileUploaderProps) {
    const {setUploadedData, setUploadedFiles} = useUploadContext();
    // Create a ref to the FileUpload instance
    const fileUploadRef = useRef<FileUpload>(null);

    const handleFileSelect = (e: any) => {
        const acceptedExtensions = accept
            .split(",")
            .map((ext) => ext.trim().toLowerCase());

        const validFiles = e.files.filter((file: any) => {
            const fileName = file.name.toLowerCase();
            return acceptedExtensions.some((ext) => fileName.endsWith(ext));
        });

        if (validFiles.length < e.files.length) {
            // Show error toast (if available)
            onShowErrorToast?.("One or more files were dropped that do not match the accepted extensions.");

            // Clear the entire queue from the UI:
            fileUploadRef.current?.clear();

            // Optionally exit early
            return;
        }

        // Otherwise we keep the valid files
        e.files = validFiles;
    };
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
            onShowErrorToast?.("Failed to parse server response");
        }
    };

    const handleUploadError = (e: any) => {
        console.error("Upload error:", e.xhr);
        onShowErrorToast?.("Upload failed or unsupported file.");
    };

  // appends the subject to form data before uploading
  const handleBeforeUpload = (event: any) => {
    event.formData.append("subject", subject);
  };

  const handleFileRemove = (e: any) => {
    setUploadedFiles((prev) => {
      const updated = { ...prev };
      delete updated[subject];
      return updated;
    });

    setFileName((prev) => prev.filter((name) => name !== uploadedFiles[subject].fileName)); //får tak i navnet til fila som skal slettes, og fjerner valgt fil fra fileName-lista
  };

    return (
        // TODO: Width should match instruction component
        <div className="min-w-[740px]">
            <FileUpload
                ref={fileUploadRef}
                name="file"
                url="http://localhost:5116/api/upload"
                multiple={true}
                mode="advanced"
                auto={false}
                accept={accept}
                onSelect={handleFileSelect}
                onBeforeUpload={handleBeforeUpload}
                onUpload={handleUploadComplete}
                onError={handleUploadError}
                onRemove={handleFileRemove}
                chooseLabel="Choose File(s)"
                uploadLabel="Upload"
                cancelLabel="Clear"
                // Get correct styling from figma
                emptyTemplate={
                    <div className="text-center">
                        <span className="pi pi-file-plus text-8xl pb-4"></span>
                        <p>Drag and drop files to here to upload.</p>
                    </div>
                }
            />
        </div>
    );
}
