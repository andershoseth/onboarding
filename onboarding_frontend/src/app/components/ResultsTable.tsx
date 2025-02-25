"use client";
import React from "react";
import {
  ColumnDef,
 
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { flattenObject } from "../utils/flattenObject";

type AnyData = Record<string, unknown>;

interface ResultTableProps {
  data: AnyData[];
}

export default function ResultTable({ data }: ResultTableProps) {
  // 1) Flate ut hver rad i data
  const Data: AnyData[] = React.useMemo(() => {
    const out = data.map((row) => flattenObject(row));
 
    console.log("Flattened data:", JSON.stringify(out, null, 2));
    return out;
   
  }, [data]);


  const columns = React.useMemo<ColumnDef<AnyData>[]>(() => {
    if (!Data.length) return [];

    const keys = Object.keys(Data[0]);
   
    return keys.map((key) => ({
      id: key,           //unik Id 
      header: key,
      accessorFn: (row) => row[key],
    
      cell: (info) => {
        const val = info.getValue();
        console.log(val)
        return String(val ?? "");
      },
    }));
  }, [Data]);

  // 3) Opprett tabellinstans
  const table = useReactTable({
    data: Data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <table className="border border-gray-300 w-full">
      <thead>
        {table.getHeaderGroups().map((headerGroup) => (
          <tr key={headerGroup.id} className="bg-gray-100">
            {headerGroup.headers.map((header) => (
              <th key={header.id} className="border px-4 py-2 text-left">
                {header.isPlaceholder
                  ? null
                  : flexRender(
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
  );
}
