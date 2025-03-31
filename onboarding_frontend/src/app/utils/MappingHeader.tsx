
"use client";

import React, { useState } from "react";
import { TableFieldMapping } from "../components/SaftData"; 

export interface MappingHeaderProps {
  columnLabel: string; 
  tableFieldMappings: TableFieldMapping[];
  currentMapping: string; 
  onMappingSelect: (mapping: string) => void;
  onLabelChange: (newLabel: string) => void;
}

const MappingHeader: React.FC<MappingHeaderProps> = ({
  columnLabel,
  tableFieldMappings,
  currentMapping,
  onMappingSelect,
  onLabelChange,
}) => {
  
  const [menuOpen, setMenuOpen] = useState(false);

  const [selectedTable, setSelectedTable] = useState<string | null>(null);

  const [isEditingLabel, setIsEditingLabel] = useState(false);
  const [editedLabel, setEditedLabel] = useState(columnLabel);

  const toggleMenu = () => setMenuOpen((prev) => !prev);


  const handleLabelEdit = (e: React.MouseEvent) => {
    e.stopPropagation(); // prevent dropdown toggle
    setIsEditingLabel(true);
  };

  const handleLabelBlur = () => {
    setIsEditingLabel(false);
    onLabelChange(editedLabel);
  };

  const handleLabelKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      setIsEditingLabel(false);
      onLabelChange(editedLabel);
    }
  };

  return (
    <div style={{ position: "relative" }}>
      <div style={{ display: "flex", alignItems: "center", cursor: "pointer" }}>
        {isEditingLabel ? (
          <input
            type="text"
            value={editedLabel}
            onChange={(e) => setEditedLabel(e.target.value)}
            onBlur={handleLabelBlur}
            onKeyDown={handleLabelKeyDown}
            autoFocus
            className="border px-1 py-0.5"
          />
        ) : (
          <div onClick={toggleMenu}>
            {editedLabel}
            {currentMapping ? ` → ${currentMapping}` : " ▼"}
          </div>
        )}
        <button
          onClick={handleLabelEdit}
          style={{
            marginLeft: "8px",
            background: "none",
            border: "none",
            cursor: "pointer",
            fontSize: "0.8rem",
          }}
          title="Edit column label"
        >
          ✎
        </button>
      </div>

      {menuOpen && !isEditingLabel && (
        <div
          style={{
            position: "absolute",
            top: "100%",
            left: 0,
            background: "white",
            border: "1px solid #ccc",
            zIndex: 999,
            minWidth: "200px",
            padding: "4px",
            color: "black"
          }}
        >
          {/* Level 1: Choose a table */}
          {!selectedTable && (
            <ul style={{ listStyle: "none", margin: 0, padding: 0 }}>
              {tableFieldMappings.map((tm) => (
                <li
                  key={tm.tableName}
                  style={{ padding: "4px 8px", cursor: "pointer" }}
                  onClick={() => setSelectedTable(tm.tableName)}
                >
                  {tm.tableName}
                </li>
              ))}
            </ul>
          )}

          {/* Level 2: Choose a field from the selected table */}
          {selectedTable && (
            <ul style={{ listStyle: "none", margin: 0, padding: 0 }}>
              {/* Back button */}
              <li
                style={{
                  padding: "4px 8px",
                  cursor: "pointer",
                  fontWeight: "bold",
                }}
                onClick={() => setSelectedTable(null)}
              >
                ← Back
              </li>
              {tableFieldMappings
                .find((tm) => tm.tableName === selectedTable)
                ?.fields.map((f) => (
                  <li
                    key={f.field}
                    style={{ padding: "4px 8px", cursor: "pointer" }}
                    onClick={() => {
                      // Update mapping in parent, e.g., "Contact.SupplierNo"
                      onMappingSelect(`${selectedTable}.${f.field}`);
                      setMenuOpen(false);
                      setSelectedTable(null);
                    }}
                  >
                    {f.field}
                  </li>
                ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default MappingHeader;
