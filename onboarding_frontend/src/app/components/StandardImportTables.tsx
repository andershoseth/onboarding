"use client";
import React, { useMemo, useState, useEffect } from "react";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { useMapping } from "./MappingContext";

// Typene våre
export interface StandardImportField {
  field: string;
}

export interface TableGroup {
  tableName: string;
  fields: StandardImportField[];
}

interface GroupTableProps {
  tableGroup: TableGroup;
}


export function GroupTable({ tableGroup }: GroupTableProps) {
  const { mappingData, setMappingData } = useMapping();

  // Initialiser data:
  // Hvis context allerede har mapping for denne tabellen, bruk den.
  // Ellers bygg en ny liste fra tableGroup.fields med en tom mapping.
  const initialData = useMemo(() => {
    return mappingData[tableGroup.tableName]
      ? mappingData[tableGroup.tableName]
      : tableGroup.fields.map((item) => ({
          ...item,
          mapping: "",
        }));
  }, [mappingData, tableGroup]);

  const [data, setData] = useState<(StandardImportField & { mapping: string })[]>(initialData);

 
  useEffect(() => {
    setMappingData((prev) => ({
      ...prev,
      [tableGroup.tableName]: data,
    }));
  }, [data, setMappingData, tableGroup.tableName]);

  // Definer kolonner med to kolonner: Field og Mapping (input)
  const columns = useMemo<ColumnDef<(StandardImportField & { mapping: string })>[]>(
    () => [
      {
        accessorKey: "field",
        header: "Field",
      },
      {
        accessorKey: "mapping",
        header: "Mapping",
        cell: ({ row }) => (
          <input
            type="text"
            value={row.original.mapping}
            onChange={(e) => {
              const newMapping = e.target.value;
              setData((oldData) =>
                oldData.map((item, index) =>
                  index === row.index ? { ...item, mapping: newMapping } : item
                )
              );
            }}
            className="border p-1 w-full text-black"
            placeholder="Skriv mapping..."
          />
        ),
      },
    ],
    [setData]
  );

  // Opprett React Table-instans
  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div style={{ marginBottom: "2rem" }}>
      <h2 className="text-xl font-semibold mb-2">{tableGroup.tableName}</h2>
      <table className="border border-gray-300 w-full">
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id} className="bg-gray-100">
              {headerGroup.headers.map((header) => (
                <th key={header.id} className="border px-4 py-2 text-left">
                  {header.isPlaceholder
                    ? null
                    : flexRender(header.column.columnDef.header, header.getContext())}
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

interface StandardImportTablesProps {
  data: TableGroup[];
}


export default function StandardImportTables({ data }: StandardImportTablesProps) {
  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Standard Import Felter</h1>
      {data.map((group) => (
        <GroupTable key={group.tableName} tableGroup={group} />
      ))}
    </div>
  );
}





/*"use client";
import React from "react";
import {
  ColumnDef,
 
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";


type AnyData = {
  path: string;
  value: string;
};
interface ResultTableProps {
  data: AnyData[];
}

export default function ResultTable({ data }: ResultTableProps) {
  // 1) Flate ut hver rad i data
  const Data: AnyData[] = React.useMemo(() => {
    console.log("✅ Flattened data:", JSON.stringify(data, null, 2));
    return data;
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
          <tr key={headerGroup.id} className="bg-black-100">
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
*/