import React, {useContext, useEffect, useRef} from "react";
import {FileUpload} from "primereact/fileupload";
import {useUploadContext} from "./UploadContext";
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
    // Merge all the logic from both old functions:

    // Access multiple contexts as needed
    const {setUploadedData, setUploadedFiles, uploadedFiles} = useUploadContext();
    const {setFileName} = useContext(ImportContext);

    // Create a ref for the <FileUpload> so we can call `clear()`
    const fileUploadRef = useRef<FileUpload>(null);

    // e.g. from your first default export
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
            // Clear the entire queue from the UI:
            fileUploadRef.current?.clear();
            return;
        }

        // Otherwise keep the valid files
        e.files = validFiles;
    };

    // e.g. from your second default export
    const handleUploadComplete = (e: any) => {
        try {
            const response = JSON.parse(e.xhr.response);
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
        // E.g. remove from contexts
        setUploadedFiles((prev) => {
            const updated = {...prev};
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
                chooseLabel="Choose File(s)"
                uploadLabel="Upload"
                cancelLabel="Clear"
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
