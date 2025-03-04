import React from 'react';
import { useUploadContext } from './UploadContext';
import { motion } from 'framer-motion';

const ProgressBar: React.FC = () => {
    const { uploadProgress } = useUploadContext();

    return (
        <div className="w-full bg-gray-200 rounded-full h-4 overflow-hidden">
            <motion.div
                className="bg-green-500 h-4 rounded-full"
                animate={{ width: `${uploadProgress}%` }}
                transition={{ duration: 0.3, ease: "linear" }}
            />
        </div>
    );
};

export default ProgressBar;