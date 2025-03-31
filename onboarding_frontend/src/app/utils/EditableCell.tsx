
"use client";
import React, { useState } from "react";
import { Row } from "@tanstack/react-table";

interface EditableCellProps {
  row: Row<any>;
  columnId: string;
  value: string;
  onSave: (rowIndex: number, columnId: string, newValue: string) => void;
}

export default function EditableCell({
  row,
  columnId,
  value,
  onSave,
}: EditableCellProps) {
  const [isEditing, setIsEditing] = useState(false);
  const [editValue, setEditValue] = useState(value);

  const handleSave = () => {
    setIsEditing(false);
    onSave(row.index, columnId, editValue);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSave();
    } else if (e.key === "Escape") {
      setIsEditing(false);
      setEditValue(value);
    }
  };

  if (!isEditing) {
    return (
      <div
        onClick={() => setIsEditing(true)}
        style={{ cursor: "pointer", minWidth: "80px" }}
        title="Click to edit"
      >
        {value}
      </div>
    );
  }

  return (
    <input
      type="text"
      autoFocus
      value={editValue}
      onChange={(e) => setEditValue(e.target.value)}
      onBlur={handleSave}
      onKeyDown={handleKeyDown}
      style={{ minWidth: "80px" }}
      className="border px-1 py-0.5"
    />
  );
}
