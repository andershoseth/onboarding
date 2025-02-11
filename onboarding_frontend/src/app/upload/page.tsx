// pages/upload.tsx
import React from 'react';
import FileUploader from '../components/FileUploader';

export default function UploadPage() {
  return (
    <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20 font-[family-name:var(--font-geist-sans)]">
      <main className="flex flex-col gap-8 row-start-2 items-center sm:items-center">
        <h1 className="text-3xl sm:text-4xl font-bold text-center">
          Upload your files
        </h1>

        <p className="text-center">
          Get started by uploading your files to our server.
        </p>

        {/* Render the FileUploader component here */}
        <div className="mx-auto">
          <FileUploader />
        </div>
      </main>
      <footer className="row-start-3 flex gap-6 flex-wrap items-center justify-center">
        <a
          className="flex items-center gap-2 hover:underline hover:underline-offset-4"
          href="/home"
        >
          Home
        </a>
      </footer>
    </div>
  );
}
