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
export interface GroupedSaftEntries{
    groupKey : string;
    entries :FlattenedEntry[]
}
interface SaftDataProps {
    data : GroupedSaftEntries []
}

// 3) Lag en underkomponent som viser én enkelt gruppe
function SaftGroup({ group }: { group: GroupedSaftEntries }) {
    
    const columns = useMemo<ColumnDef<FlattenedEntry>[]>(
      () => [
        {
          accessorKey: "path",
         
          cell: ({ row }) => {
            const fullPath = row.original.path;
            const segments = fullPath.split(".");
            const lastSegment = segments[segments.length - 1] || fullPath;
            return lastSegment;
          },
        },
        {
          accessorKey: "value",
          
        },
      ],
      []
    );
// Opprett tabellinstans
const table = useReactTable({
    data: group.entries,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="mb-6">
      <h2 className="text-lg font-bold mb-2">{group.groupKey}</h2>
      <table className="border border-gray-300 w-full">
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id} className="bg-gray-100">
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
// rendrer en SaftGroup-tabell per gruppe
export default function SaftData({ data }: SaftDataProps) {
    return (
      <div>
        {data.map((group) => (
          <SaftGroup key={group.groupKey} group={group} />
        ))}
      </div>
    );
  }