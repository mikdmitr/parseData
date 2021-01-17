using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace parseData
{
    public class DataParser
    {
        public Layout oLayout { get; } = new Layout();
        public List<DataTable> DataTables { get; } = new List<DataTable>();


        public DataParser(String sDataLayoutFileName, String sDataFileName) 
        {
            //int counter = 0;
            string line;

            Boolean isComment = true;
            Boolean isRecordedComment = false;
            String previousComment = "";
            int recordCounter = 0;
            ILayoutRow previousLayoutRow = null;

            // reading layout data file

            System.IO.StreamReader file = new System.IO.StreamReader(sDataLayoutFileName);//@"D:\docs\jobs\boardrige\2\MULTI_ROW_LAYOUT.txt"

            while ((line = file.ReadLine()) != null)
            {
                //System.Console.WriteLine(line);
                //counter++;

                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Count() > 0)
                {
                    isComment = words[0].IndexOf("*") >= 0;
                    if (isComment)
                    {
                        previousComment = line;
                        isRecordedComment = false;
                    }
                    if ((!isComment) && (words.Length > 1))
                    {
                        ILevel oLevel = new LayoutRawLevel(words[0]);
                        oLayout.AddLevel(oLevel);

                        if (previousLayoutRow == null)
                        {
                            IIndex oIndex = new LayoutRawIndex(oLayout.Levels.Where(a => a.Equals(oLevel)).FirstOrDefault(), 0);
                            oLayout.AddRawIndex(oIndex);
                            ILayoutRow currentLayoutRow = new LayoutRaw(line.Trim(), recordCounter, oLayout.Levels.Where(a => a.Equals(oLevel)).FirstOrDefault(), oLayout.Indexes.Where(a => a.Equals(oIndex)).FirstOrDefault());

                            currentLayoutRow.sPreComments = previousComment;
                            isRecordedComment = true;
                            oLayout.AddLayoutRaw(currentLayoutRow);

                            recordCounter++;
                            previousLayoutRow = currentLayoutRow;
                        }
                        else
                        {
                            ILevel currentLevel = oLayout.Levels.Where(a => a.Equals(oLevel)).FirstOrDefault();
                            ILevel previousLevel = currentLevel.isRoot ? null : oLayout.Levels.Where(a => a.ordinalNumber == oLevel.ordinalNumber - 1).FirstOrDefault();

                            int ordinalInLevel = currentLevel.isRoot ? oLayout.LayoutRows.Where(a => a.Level.isRoot).LastOrDefault().Index.ordinalInLevel + 1 : previousLayoutRow.Level.Equals(currentLevel) ? previousLayoutRow.Index.ordinalInLevel + 1 : oLayout.LayoutRows.Where(a => a.Level.Equals(currentLevel)).Any() ? oLayout.LayoutRows.Where(a => a.Level.Equals(currentLevel)).LastOrDefault().Index.ordinalInLevel + 1 : 0;
                            IIndex parentIndex = currentLevel.isRoot ? null : previousLayoutRow.Level.Equals(currentLevel) ? previousLayoutRow.Index.Parent : oLayout.LayoutRows.Where(a => a.Level.Equals(currentLevel)).Any() ? oLayout.LayoutRows.Where(a => a.Level.Equals(currentLevel)).LastOrDefault().Index.Parent : oLayout.LayoutRows.Where(a => a.Level.Equals(previousLevel)).LastOrDefault().Index;
                            IIndex currentIndex = new LayoutRawIndex(currentLevel, ordinalInLevel, parentIndex);

                            oLayout.AddRawIndex(currentIndex);
                            ILayoutRow currentLayoutRow = new LayoutRaw(line.Trim(), recordCounter, currentLevel, oLayout.Indexes.Where(a => a.Equals(currentIndex)).FirstOrDefault());

                            currentLayoutRow.sPreComments = !isRecordedComment ? previousComment : "";
                            isRecordedComment = true;

                            oLayout.AddLayoutRaw(currentLayoutRow);

                            recordCounter++;
                            previousLayoutRow = currentLayoutRow;
                        }
                    }
                }


            }

            file.Close();

            //System.Console.WriteLine("There were {0} lines.", counter);



            //reading data file

            System.IO.StreamReader fileData = new System.IO.StreamReader(sDataFileName);//@"D:\docs\jobs\boardrige\2\Multi_Row_Test_Data.txt"

            List<String> dataFile = new List<String>();

            while ((line = fileData.ReadLine()) != null)
            {
                dataFile.Add(line);
            }

            fileData.Close();

            if (!oLayout.isSingleTableLayout)  //multiRawTables
            {
                foreach (LayoutTableDefinition tableDef in oLayout.TableDefinitions)
                {
                    String sForParse = dataFile.Where(a => a.IndexOf(tableDef.Name) > 0).FirstOrDefault();
                    DataTable dTable = new DataTable(tableDef);
                    List<DataItem> sharedPartDataItem = new List<DataItem>();
                    int iPos = 0;
                    foreach (LayoutRaw layoutRaw in tableDef.columns.Where(a => a.varName.FirstName != tableDef.Name))
                    {
                        DataItem dataItem = new DataItem(layoutRaw, sForParse.Substring(iPos, layoutRaw.dataLength));
                        iPos += layoutRaw.dataLength;
                        sharedPartDataItem.Add(dataItem);
                    }

                    String testForstop = "";

                    do
                    {
                        testForstop = "";
                        DataRaw dataRaw = new DataRaw();
                        //dataRaw.DataItems = dataRaw.DataItems.Concat(sharedPartDataItem);
                        foreach (DataItem dataItem in sharedPartDataItem)
                            dataRaw.AddDataItem(dataItem);



                        foreach (LayoutRaw layoutRaw in tableDef.columns.Where(a => a.isGroupPart))
                        {
                            DataItem dataItem = new DataItem(layoutRaw, sForParse.Substring(iPos, layoutRaw.dataLength));

                            testForstop = sForParse.Substring(iPos, layoutRaw.dataLength);


                            dataRaw.AddDataItem(dataItem);

                            iPos += layoutRaw.dataLength;
                        }

                        if (testForstop.Trim() == "")
                            continue;

                        dTable.AddRaw(dataRaw);
                    }
                    while ((iPos < tableDef.groupRawPartSize + tableDef.multiPartVolume - tableDef.fillerSize) && (testForstop.Trim() != ""));

                    DataTables.Add(dTable);
                }
            }
            else
            {
                ILayoutTableDefinition tableDef = oLayout.TableDefinitions.FirstOrDefault();
                                
                DataTable dTable = new DataTable(tableDef);
                foreach(String sForParse in dataFile)
                {                    
                    DataRaw dataRaw = new DataRaw();

                    int iPos = 0;
                    foreach (LayoutRaw layoutRaw in tableDef.columns)
                    {
                        DataItem dataItem = new DataItem(layoutRaw, sForParse.Substring(iPos, layoutRaw.dataLength));
                        iPos += layoutRaw.dataLength;
                        dataRaw.AddDataItem(dataItem);
                    }
                    dTable.AddRaw(dataRaw);
                }
                DataTables.Add(dTable);

            }

                        
        }



        public void ExportDataToCSV() 
        {            
            foreach (DataTable dataTable in DataTables)
            {
                string writePath = Directory.GetCurrentDirectory() + "\\" + dataTable.Name + ".csv";
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(dataTable.columnNameExport);

                    foreach (DataRaw dataRaw in dataTable.rows)
                        sw.WriteLine(dataRaw.DataForExport);
                }

            }
        }


    }
}
