import React from 'react';
import { useUploadContext } from './UploadContext';

interface ProgressBarProps {
    progress: number;
}

const ProgressBar: React.FC<ProgressBarProps> = ({ progress }) => {
    const { uploadProgress } = useUploadContext();
    return (
        <div className="w-full bg-gray-200 rounded-full h-4">
            <div className="bg-green-500 h-4 rounded-full" style={{ width: `${progress}%` }}>
            </div>
        </div>
    );
};
export default ProgressBar;