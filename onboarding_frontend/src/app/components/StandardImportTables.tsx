"use client";
import React, { useMemo } from "react";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";

// Definer typen for hvert felt
export interface StandardImportField {
  field: string;
}

// Definer typen for en gruppe (tabell)
export interface TableGroup {
  tableName: string;
  fields: StandardImportField[];
}

interface GroupTableProps {
  tableGroup: TableGroup;
}


function GroupTable({ tableGroup }: GroupTableProps) {
  // data for react-table er en array med objekter ({ field: string })
  const data = useMemo(() => tableGroup.fields, [tableGroup.fields]);

  const columns = useMemo<ColumnDef<StandardImportField>[]>(() => [
    {
      accessorKey: "field",
      header: "Field",
    },
  ], []);

 
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
    </div>
  );
}

interface StandardImportTablesProps {
  data: TableGroup[];
}

// Hovedkomponenten som itererer over alle grupper og viser en egen tabell for hver gruppe
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