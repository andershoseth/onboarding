"use client";
import React, { useMemo } from "react";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";

export interface FlattenedEntry {
  path: string;
  value: string;
}

export interface GroupedSaftEntries {
  groupKey: string;
  entries: FlattenedEntry[];
}

interface SaftDataProps {
  data: GroupedSaftEntries[];
}


//Pivoterer group.entries

function SaftGroup({ group }: { group: GroupedSaftEntries }) {
    const { rows, columns } = useMemo(() => pivotByLastSegment(group.entries), [group.entries]);
  

    const columnDefs = useMemo<ColumnDef<Record<string, string>>[]>(
      () => {
        
        // { accessorKey: "rowKey", header: "Prefix" }
    
        return columns.map((colKey) => ({
          accessorKey: colKey,
          header: colKey, // overskriften
        }));
      },
      [columns]
    );
  
    // Data: rows (hver row har rowKey og felter for de unike segmentene)

    const data = rows; 
  
    const table = useReactTable({
      data,
      columns: columnDefs,
      getCoreRowModel: getCoreRowModel(),
    });
  
    return (
      <div className="mb-6">
        <h2 className="text-lg font-bold mb-2">{group.groupKey}</h2>
        <table className="border border-gray-300 w-full">
          <thead>
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id} className="bg-black-100">
                {headerGroup.headers.map((header) => (
                  <th key={header.id} className="border px-4 py-2 text-left">
                    {flexRender(header.column.columnDef.header, header.getContext())}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.map((row) => (
              <tr key={row.id} className="hover:bg-gray-50">
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id} className="border px-4 py-2">
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }
  
  // Hjelpefunksjonen for pivot
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
      rowKey,// f.eks. "AuditFile.MasterFiles.GeneralLedgerAccounts.Account[1]"
      ...rowMap[rowKey],//f.eks. { "AccountID": "1000", "AccountDescription": "Forskning og utvikling", ... }
    }));
    const columns = Array.from(allSegments);
  
    return { rows, columns };
  }
  

 
  export default function SaftData({ data }: SaftDataProps) {
    return (
      <div>
        {data.map((group) => (
          <SaftGroup key={group.groupKey} group={group} />
        ))}
      </div>
    );
  }
