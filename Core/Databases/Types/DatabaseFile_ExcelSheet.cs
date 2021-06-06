// This file is part of Emulators Organizer
// A program that can organize roms and emulators
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System.Collections.Generic;
using System.Data;
using ClosedXML.Excel;
using System.Data.OleDb;

namespace EmulatorsOrganizer.Core
{
    [DatabaseInfo("Excel Datasheet", new string[] { ".xlsx" }, CompareMode.RomFileName | CompareMode.RomName, true, false, false)]
    public class DatabaseFile_ExcelSheet : DatabaseFile
    {
        public override List<DBEntry> LoadFile(string filePath)
        {
            List<DBEntry> files = new List<DBEntry>();
            OnProgressStarted("Reading Excel Datasheet XLSX file...");
            var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";

                    var adapter = new OleDbDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // 1 Get the proerties types
                        List<string> properties = new List<string>();

                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            // First row is about the properties ...
                            int x = (i * 100) / (int)ds.Tables[0].Rows.Count;
                            OnProgress(x, "Reading Excel Datasheet XLSX file (reading properties)... [" + x + " %]");
                            properties.Add(ds.Tables[0].Rows[0][i].ToString());
                        }

                        // 2 Get data !
                        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DBEntry ff = DBEntry.Empty;
                            // Name is the first value !!

                            ff.FileNames.Add(ds.Tables[0].Rows[i][0].ToString());
                            ff.Properties.Add(new PropertyStruct("Name", ds.Tables[0].Rows[i][0].ToString()));
                            // Add the rest of the columns
                            for (int j = 1; j < properties.Count; j++)
                            {
                                ff.Properties.Add(new PropertyStruct(properties[j], ds.Tables[0].Rows[i][j].ToString()));
                            }
                            files.Add(ff);
                            int x = (i * 100) / (int)ds.Tables[0].Rows.Count;
                            OnProgress(x, "Reading Excel Datasheet XLSX file (reading data)... [" + x + " %]");
                        }

                    }
                }
            }
            OnProgressFinished("Excel Datasheet XLSX reading done successfully.");
            return files;
        }
        public override void SaveFile(string fileName, List<DBEntry> entries)
        {
            base.SaveFile(fileName, entries);
            OnProgressStarted("Saving Excel Datasheet file ...");
            // 1 Create a data set
            DataSet ds = new DataSet();
            ds.Tables.Add("EO");

            // 2 Add the columns
            foreach (PropertyStruct pr in entries[0].Properties)
            {
                ds.Tables[0].Columns.Add(pr.Property);
            }
            // 3 Add the row of the columns !
            ds.Tables[0].Rows.Add(entries[0].Properties);
            // 4 Add the data ...
            int j = 0;
            foreach (DBEntry ent in entries)
            {
                ds.Tables[0].Rows.Add();
                for (int i = 0; i < ent.Properties.Count; i++)
                {
                    ds.Tables[0].Rows[j][i] = ent.Properties[i].Value;
                }
                int p = (j * 100) / entries.Count;
                OnProgress(p, "Applying Excel Datasheet data [" + p + " %]");
                j++;
            }

            var wb = new XLWorkbook();
         
            // Add all DataTables in the DataSet as a worksheets
            wb.Worksheets.Add(ds);
            OnProgress(100, "Writing Excel Datasheet data ...");
            wb.SaveAs(fileName);
            wb.Dispose();
            OnProgressFinished("Excel Datasheet database file saved successfully.");
        }
    }
}
