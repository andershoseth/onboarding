
"use client";

import React, { useState } from "react";
import { TableFieldMapping } from "../components/SaftData"; 

export interface MappingHeaderProps {
  saftColumn: string;
  tableFieldMappings: TableFieldMapping[];
  currentMapping: string;
  onMappingSelect: (mapping: string) => void;
}

const MappingHeader: React.FC<MappingHeaderProps> = ({
  saftColumn,
  tableFieldMappings,
  currentMapping,
  onMappingSelect,
}) => {
  const [menuOpen, setMenuOpen] = useState(false);
  const [selectedTable, setSelectedTable] = useState<string | null>(null);

  const toggleMenu = () => setMenuOpen((prev) => !prev);

  return (
    <div style={{ position: "relative" }}>
      {/* Display either the current mapping or the dropdown arrow */}
      <div style={{ cursor: "pointer" }} onClick={toggleMenu}>
        {currentMapping
          ? `${saftColumn} → ${currentMapping}`
          : `${saftColumn} ▼`}
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

          {/* Level 2: Choose a field from that table */}
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
