"use client";
import React, { useState } from "react";


export interface TableFieldMapping {
  tableName: string;
  fields: { field: string }[];
}

// 2) Define props that are generic: 
//    - "columnLabel" can be any name: “SAF-T path segment”, “CSV header”, etc.
//    - "currentMapping" is the user’s selected "tableName.field" so far.
//    - "onMappingSelect" is how we notify the parent that the user picked a field.
interface MappingDropdownHeaderProps {
  columnLabel: string;
  tableFieldMappings: TableFieldMapping[];
  currentMapping: string;
  onMappingSelect: (mapping: string) => void;
}

export const MappingDropdownHeader: React.FC<MappingDropdownHeaderProps> = ({
  columnLabel,
  tableFieldMappings,
  currentMapping,
  onMappingSelect,
}) => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [selectedTable, setSelectedTable] = useState<string | null>(null);

  const toggleMenu = () => setMenuOpen((prev) => !prev);

  return (
    <div style={{ position: "relative" }}>
      <div style={{ cursor: "pointer" }} onClick={toggleMenu}>
        {currentMapping
          ? `${columnLabel} → ${currentMapping}`
          : `${columnLabel} ▼`}
      </div>

      {menuOpen && (
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
            color: "black",
          }}
        >
          {/* Step 1: choose which table */}
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
          {/* Step 2: choose which field from that table */}
          {selectedTable && (
            <ul style={{ listStyle: "none", margin: 0, padding: 0 }}>
              {/* "Back" button */}
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
