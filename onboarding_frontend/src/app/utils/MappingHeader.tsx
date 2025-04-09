"use client";
import React, { useState } from "react";
import { TableFieldMapping } from "../components/SaftData";

export interface MappingHeaderProps {
  columnLabel: string; 
  tableFieldMappings: TableFieldMapping[];
  currentMapping: string; 
  onMappingSelect: (mapping: string) => void;
}

const MappingHeader: React.FC<MappingHeaderProps> = ({
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
      {}
      <div style={{ cursor: "pointer" }} onClick={toggleMenu}>
        {columnLabel} {currentMapping ? `→ ${currentMapping}` : "▼"}
      </div>

      {/* Dropdown Menu */}
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
            maxHeight: "500px",
            overflowY: "auto",
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
