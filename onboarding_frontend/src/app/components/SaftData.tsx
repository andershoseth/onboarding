"use client";
import React, { useState, useEffect, useMemo } from "react";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { MappingDropdownHeader, TableFieldMapping } from "./MappingDropdownHeader"

// SAF-T Type
export interface FlattenedEntry {
  path: string;
  value: string;
}

export interface GroupedSaftEntries {
  groupKey: string;
  entries: FlattenedEntry[];
}

//Standard Import Type
export interface StandardImportField {
  field: string;
}

// Pivot function for SAF-T Data
function pivotByLastSegment(entries: FlattenedEntry[]) {
  const rowMap: Record<string, Record<string, string>> = {};
  const allSegments = new Set<string>();

  for (const entry of entries) {
    const segments = entry.path.split(".");
    const lastSegment = segments[segments.length - 1] || entry.path;
    const prefix = segments.slice(0, -1).join(".");

    if (!rowMap[prefix]) {
      rowMap[prefix] = {};
    }
    rowMap[prefix][lastSegment] = entry.value;
    allSegments.add(lastSegment);
  }

  const rows = Object.keys(rowMap).map((rowKey) => ({
    rowKey, // e, "AuditFile.MasterFiles.GeneralLedgerAccounts.Account[1]"
    ...rowMap[rowKey],
  }));
  const columns = Array.from(allSegments);

  return { rows, columns };
}

// Dropdown
interface MappingHeaderProps {
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
          {/* Step 1: choose a table */}
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
          {/* Step 2: choose a field from that table */}
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
                      // Update mapping in parent
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

// Renders one group of Saft data with a mapping header for each column
function SaftGroup({
  group,
  tableFieldMappings,
}: {
  group: GroupedSaftEntries;
  tableFieldMappings: TableFieldMapping[];
}) {
  // Pivot SAF-T data
  const { rows, columns } = useMemo(
    () => pivotByLastSegment(group.entries),
    [group.entries]
  );

  // Maintain the user's mappings of SAF-T fields to standard fields
  const [mapping, setMapping] = useState<Record<string, string>>({});

  const columnDefs = useMemo<ColumnDef<Record<string, string>>[]>(
    () =>
      columns.map((colKey) => ({
        accessorKey: colKey,
        header: () => (
          <MappingDropdownHeader
            columnLabel={colKey}                 // pass the SAF-T column name
            tableFieldMappings={tableFieldMappings}
            currentMapping={mapping[colKey] || ""} // same as before
            onMappingSelect={(selected) =>
              setMapping((prev) => ({ ...prev, [colKey]: selected }))
            }
          />
        ),
      })),
    [columns, mapping, tableFieldMappings]
  );

  const table = useReactTable({
    data: rows,
    columns: columnDefs,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="mb-6">
      {/* Group heading */}
      <h2 className="text-xl font-bold mb-2">{group.groupKey}</h2>

      {/* Excel-style container for each group's table */}
      <div className="overflow-y-auto max-h-[calc(80vh-150px)] border border-gray-500 rounded-lg shadow-md bg-white">
        <table className="min-w-full">
          <thead className="bg-gray-600 text-black sticky top-0 z-10">
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th
                    key={header.id}
                    className="border border-gray-400 px-4 py-2 text-left"
                  >
                    {flexRender(
                      header.column.columnDef.header,
                      header.getContext()
                    )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.map((row) => (
              <tr
                key={row.id}
                // Alternate row color (Excel-like)
                className={row.index % 2 === 0 ? "bg-gray-300" : "bg-gray-100"}
              >
                {row.getVisibleCells().map((cell) => (
                  <td
                    key={cell.id}
                    className="border border-gray-400 px-4 py-2 text-gray-900"
                  >
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// Fetches Standard Import fields and renders all SAF-T groups
export default function SaftData({ data }: { data: GroupedSaftEntries[] }) {
  const [tableFieldMappings, setTableFieldMappings] = useState<TableFieldMapping[]>([]);

  useEffect(() => {

    fetch("http://localhost:5116/api/standard-import-mapping")
      .then((res) => res.json())
      .then((fetchedData: TableFieldMapping[]) => {
        setTableFieldMappings(fetchedData);
      })
      .catch((error) =>
        console.error("Error fetching standard import mapping:", error)
      );
  }, []);


  return (
    <div className="p-6 min-h-screen pt-16">
      <h2 className="text-2xl font-bold mb-4">SAF-T Data</h2>

      {data.length === 0 && (
        <p className="text-gray-500">Ingen SAF-T data tilgjengelig.</p>
      )}

      {/* Render a table for each group */}
      {data.map((group) => (
        <SaftGroup
          key={group.groupKey}
          group={group}
          tableFieldMappings={tableFieldMappings}
        />
      ))}
    </div>
  );
}
