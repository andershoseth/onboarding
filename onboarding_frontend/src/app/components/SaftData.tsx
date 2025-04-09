//saftData.tsx
"use client";
import React, { useState, useEffect, useMemo } from "react";
import { useMapping } from "../components/MappingContext";
import MappingHeader from "../utils/MappingHeader";
import EditableCell from "../utils/EditableCell";
import SubmitStandardImportButton from "../components/Submit";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";

// Types 
export interface FlattenedEntry {
  path: string;
  value: string;
}
export interface GroupedSaftEntries {
  groupKey: string;
  entries: FlattenedEntry[];
}
export interface StandardImportField {
  field: string;
}
export interface TableFieldMapping {
  tableName: string;
  fields: StandardImportField[];
}


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
    rowKey, // 
    ...rowMap[rowKey],
  }));
  const columns = Array.from(allSegments);

  return { rows, columns };
}

//  SaftGroup (handles one group) 
function SaftGroup({
  group,
  tableFieldMappings,
}: {
  group: GroupedSaftEntries;
  tableFieldMappings: TableFieldMapping[];
}) {
  const { mapping, setMapping, groupedRows, setGroupedRows } = useMapping();

  const { rows: pivotedRows, columns } = useMemo(
    () => pivotByLastSegment(group.entries),
    [group.entries]
  );

  useEffect(() => {
    if (!groupedRows[group.groupKey]) {
      setGroupedRows((prev) => ({ ...prev, [group.groupKey]: pivotedRows }));
    }
  }, [group.groupKey, pivotedRows, groupedRows, setGroupedRows]);

  const contextRows = groupedRows[group.groupKey] || [];

  function handleCellSave(rowIndex: number, columnId: string, newValue: string) {
    setGroupedRows((prev) => {
      const updated = { ...prev };
      const arrCopy = [...(updated[group.groupKey] || [])];
      arrCopy[rowIndex] = { ...arrCopy[rowIndex], [columnId]: newValue };
      updated[group.groupKey] = arrCopy;
      return updated;
    });
  }

  const columnDefs = useMemo<ColumnDef<Record<string, string>>[]>(
    () =>
      columns.map((colKey) => ({
        accessorKey: colKey,
        header: () => (
          <MappingHeader
            columnLabel={colKey}
            tableFieldMappings={tableFieldMappings}
            currentMapping={mapping[colKey] || ""}
            onMappingSelect={(selected) =>
              setMapping((prev) => ({ ...prev, [colKey]: selected }))
            }
          />
        ),
        cell: (info) => {
          const value = String(info.getValue() ?? "");
          return (
            <EditableCell
              row={info.row}
              columnId={colKey}
              value={value}
              onSave={handleCellSave}
            />
          );
        },
      })),
    [columns, mapping, setMapping, tableFieldMappings]
  );

  const table = useReactTable({
    data: contextRows,
    columns: columnDefs,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="mb-6">
      <h2 className="text-xl font-bold mb-2">{group.groupKey}</h2>
      <div className="overflow-y-auto max-h-[calc(80vh-150px)] border border-gray-500 rounded-lg shadow-md bg-white">
        <table className="min-w-full">
          <thead className="bg-green-600 text-black sticky top-0 z-10">
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

// SaftData (renders multiple groups) 
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
  // Filtrer til kun "Voucher" og "VoucherLine"
  const filteredTableFieldMappings = useMemo(() => {
    return tableFieldMappings.filter(
      (mapping) =>
        mapping.tableName === "Voucher" || mapping.tableName === "VoucherLine"
    );
  }, [tableFieldMappings]);

  if (!data || (Array.isArray(data) && data.length === 0)) {
    return <p className="p-4 text-gray-500">Ingen SAF-T data tilgjengelig.</p>;
  }
  
  const groups = Array.isArray(data) ? data : [data];
  
  return (
    <>
      <SubmitStandardImportButton />
      <div className="p-6 min-h-screen pt-16">
        <h2 className="text-2xl font-bold mb-4">SAF-T Data</h2>
        {groups.map((group) => (
          <SaftGroup
            key={group.groupKey}
            group={group}
            tableFieldMappings={filteredTableFieldMappings}
          />
        ))}
      </div>
    </>
  );

}
  