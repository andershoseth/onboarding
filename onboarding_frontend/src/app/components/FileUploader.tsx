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

    const { setUploadedData, setUploadedFiles, uploadedFiles } = useUploadContext();
    const { setFileName } = useContext(ImportContext);

    // Create a ref for fileupload so it's possibl to call clear()
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
            onShowErrorToast?.(
                "One or more files were dropped that do not match the accepted extensions."
            );
            // Clearing the queue from the ui
            fileUploadRef.current?.clear();
            return;
        }

        e.files = validFiles;
    };

    const handleUploadComplete = (e: any) => {
        try {
            const response = JSON.parse(e.xhr.response);
            console.log(response)
            const uploadedFileName = e.files[0]?.name ?? "unknown";

            setFileName((prev) => [...prev, uploadedFileName]);
            setUploadedData(response.data);

            setUploadedFiles((prev) => ({
                ...prev,
                [response.subject]: {
                    fileName: uploadedFileName,
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

    const handleBeforeUpload = (event: any) => {
        event.formData.append("subject", subject);
    };

    const handleFileRemove = (e: any) => {
        setUploadedFiles((prev) => {
            const updated = { ...prev };
            delete updated[subject];
            return updated;
        });

        // Also remove from fileName array
        if (uploadedFiles[subject]) {
            const removedFileName = uploadedFiles[subject].fileName;
            setFileName((prev) => prev.filter((name) => name !== removedFileName));
        }
    };

    return (
        <div className="min-w-[740px]">
            <FileUpload
                ref={fileUploadRef}
                name="file"
                url="http://localhost:5116/api/upload"
                multiple
                mode="advanced"
                auto={false}
                accept={accept}
                onSelect={handleFileSelect}
                onBeforeUpload={handleBeforeUpload}
                onUpload={handleUploadComplete}
                onError={handleUploadError}
                onRemove={handleFileRemove}
                chooseLabel="Velg fil(er)"
                uploadLabel="Last opp"
                cancelLabel="Fjern"
                emptyTemplate={
                    <div className="text-center">
                        <span className="pi pi-file-plus text-8xl pb-4"></span>
                        <p>Klikk og slipp filer her for å laste opp</p>
                    </div>
                }
            />
        </div>
    );
}
