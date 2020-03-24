using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;
using System.ComponentModel;
namespace KtsDBHelper
{
    /// <summary>
    /// DataTable操作
    /// </summary>
    public abstract class DataTableHelper
    {
        /// <summary>
        /// 根据条件分组，保留所有列。List<DataTable> grouped = new List<DataTable>() ;  
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="groupByFields"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="schema"></param>
        public static void GroupDataRows(IEnumerable<DataRow> source, List<DataTable> destination,
            string[] groupByFields, int fieldIndex, DataTable schema)
        {
            if (fieldIndex >= groupByFields.Length || fieldIndex < 0)
            {
                DataTable dt = schema.Clone();
                foreach (DataRow row in source)
                {
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = row.ItemArray;
                    dt.Rows.Add(dr);
                }

                destination.Add(dt);
                return;
            }

            var results = source.GroupBy(o => o[groupByFields[fieldIndex]]);
            foreach (var rows in results)
            {
                GroupDataRows(rows, destination, groupByFields, fieldIndex + 1, schema);
            }

            fieldIndex++;
        }

 
        /// <summary>   
        /// 将DataTable按起始位置和移动及移动方向进行移动并返回新的DataTable   
        /// </summary>   
        /// <param name="dt">要移动的DataTable</param>   
        /// <param name="StartRow">要移动的行（索引从1开始）</param>   
        /// <param name="MoveCount">要移动的行数</param>   
        /// <param name="MoveUp">是否上移（true为上移false为下移）</param>           
        /// <returns>将移动完成后的DataTable返回，如果移动有误的话将返回原Table</returns>   
        static public DataTable GetNewTable(DataTable dt, int StartRow, int MoveCount, bool MoveUp)
        {
            #region 将DataTable按起始位置和移动及移动方向进行移动并返回新的DataTable

            DataRow dr = dt.NewRow();
            dr.ItemArray = dt.Rows[StartRow - 1].ItemArray;
            int RowCount = dt.Rows.Count;
            if (StartRow > RowCount)//移动的行在行数外面   
                return dt;
            if (MoveUp)//上移   
            {
                if (StartRow - MoveCount <= 0) { }
                else
                {
                    for (int i = 0; i < MoveCount; i++)
                        dt.Rows[StartRow - i - 1].ItemArray = dt.Rows[StartRow - i - 2].ItemArray;
                    dt.Rows[StartRow - MoveCount - 1].ItemArray = dr.ItemArray;
                }
            }
            else//下移   
            {
                if (StartRow + MoveCount > RowCount) { }
                else
                {
                    for (int i = 0; i < MoveCount; i++)
                        dt.Rows[StartRow + i - 1].ItemArray = dt.Rows[StartRow + i].ItemArray;
                    dt.Rows[StartRow + MoveCount - 1].ItemArray = dr.ItemArray;
                }
            }
            return dt;
            #endregion
        }

        /// <summary>   
        /// 根据条件过滤表   
        /// </summary>   
        /// <param name="dt">未过滤之前的表</param>   
        /// <param name="filter">过滤条件</param>   
        /// <returns>返回过滤后的表</returns>   
        static public DataTable GetNewTable(DataTable dt, string filter)
        {
            #region 根据条件过滤表
            DataTable newTable = dt.Clone();
            DataRow[] drs = dt.Select(filter);
            foreach (DataRow dr in drs)
            {
                object[] arr = dr.ItemArray;
                DataRow newrow = newTable.NewRow();
                for (int i = 0; i < arr.Length; i++)
                    newrow[i] = arr[i];
                newTable.Rows.Add(newrow);
            }
            return newTable;
            #endregion
        }
        // <summary>   
        /// 根据条件过滤表   
        /// </summary>   
        /// <param name="dt">未过滤之前的表</param>   
        /// <param name="filter">过滤条件</param>   
        /// <returns>返回过滤后的表</returns>   
        static public DataTable GetNewTable(DataTable dt, string filter, string sort)
        {
            #region 根据条件过滤表
            DataTable newTable = dt.Clone();
            DataRow[] drs = dt.Select(filter, sort);
            foreach (DataRow dr in drs)
            {
                object[] arr = dr.ItemArray;
                DataRow newrow = newTable.NewRow();
                for (int i = 0; i < arr.Length; i++)
                    newrow[i] = arr[i];
                newTable.Rows.Add(newrow);
            }
            return newTable;
            #endregion
        }
        /// <summary>   
        /// 根据条件过滤表   
        /// </summary>   
        /// <param name="dt">未过滤之前的表</param>   
        /// <param name="filter">过滤条件</param>   
        /// <returns>返回过滤后的表</returns>   
        static public DataTable GetNewTableByDataView(DataTable dt, string filter)
        {
            #region 根据条件过滤表
            DataView dv = new DataView(dt);
            dv.RowFilter = filter;
            return dv.Table;
            #endregion
        }
        /// <summary>
        /// 返回两个表的关联数据，关联后的表中只包含第一个表的字段和第二个表需要的字段
        /// </summary>
        /// <param name="FirstTable">第一个表（左表）</param>
        /// <param name="SecondTable">第二个表（右表）</param>
        /// <param name="FJC">第一个表要与第二个表关联的字段</param>
        /// <param name="SJC">第二个表要与第一个表关联的字段</param>
        /// <param name="SJCNeed">第二个表中需要保留的字段</param>
        /// <param name="IsLeftOuter">是否是左外连接，否则为内连接</param>
        /// <returns></returns>
        private static DataTable LeftOuterOrJoin(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC, DataColumn[] SJCNeed, bool IsLeftOuter)
        {

            //创建一个新的DataTable

            DataTable table = new DataTable("LJoin");
            // Use a DataSet to leverage DataRelation
            using (DataSet ds = new DataSet())
            {

                //把DataTable Copy到DataSet中
                //ds.Tables.AddRange(new DataTable[]{First.Copy(),Second.Copy()});
                DataTable First = FirstTable.Copy();
                DataTable Second = SecondTable.Copy();
                First.TableName = "FirstTable";
                Second.TableName = "SecondTable";
                
                for (int i = 0; i < Second.Columns.Count; i++)//删除第二个表中不需要的字段
                {
                    bool Conten = false;
                    for (int j = 0; j < SJC.Length; j++)
                    {
                        if (SJC[j].ColumnName.ToUpper().Trim() == Second.Columns[i].ColumnName.ToUpper().Trim())
                        {
                            Conten = true;
                            break;
                        }
                    }
                    if (!Conten)
                    {
                        for (int j = 0; j < SJCNeed.Length; j++)
                        {
                            if (SJCNeed[j].ColumnName.ToUpper().Trim() == Second.Columns[i].ColumnName.ToUpper().Trim())
                            {
                                Conten = true;
                                break;
                            }
                        }
                    }
                    if (!Conten)
                    {
                        Second.Columns.RemoveAt(i);
                        i--;
                    }
                }
                ds.Tables.AddRange(new DataTable[] { First, Second });
                DataColumn[] parentcolumns = new DataColumn[FJC.Length];
                for (int i = 0; i < parentcolumns.Length; i++)
                {
                    parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
                }

                DataColumn[] childcolumns = new DataColumn[SJC.Length];
                for (int i = 0; i < childcolumns.Length; i++)
                {
                    childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
                }

                //创建关联
                DataRelation r = new DataRelation(string.Empty, parentcolumns, childcolumns, false);
                ds.Relations.Add(r);
                //为关联表创建列
                for (int i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                for (int i = 0; i < Second.Columns.Count; i++)
                {
                    //看看有没有重复的列，如果有在第二个DataTable的Column的列明后加_Second
                    if (!table.Columns.Contains(Second.Columns[i].ColumnName))
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
                    }
                    else
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
                    }
                }
                table.BeginLoadData();
                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //得到行的数据
                    DataRow[] childrows = firstrow.GetChildRows(r);
                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;
                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;
                            object[] joinarray = new object[parentarray.Length + secondarray.Length];
                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                            table.LoadDataRow(joinarray, true);
                        }
                    }
                    else
                    {
                        if (IsLeftOuter)
                        {
                            object[] parentarray = firstrow.ItemArray;
                            DataRow secondrow = Second.NewRow();
                            {
                                object[] secondarray = secondrow.ItemArray;
                                object[] joinarray = new object[parentarray.Length + secondarray.Length];
                                Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                                Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                                table.LoadDataRow(joinarray, true);
                            }
                        }
                    }
                }
                table.EndLoadData();
            }
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName.EndsWith("_Second"))
                {
                    table.Columns.RemoveAt(i);
                    i--;
                }
            }
            return table;

        }
        /// <summary>
        /// 返回两个表的关联数据
        /// </summary>
        /// <param name="FirstTable">第一个表（左表）</param>
        /// <param name="SecondTable">第二个表（右表）</param>
        /// <param name="FJC">第一个表要与第二个表关联的字段</param>
        /// <param name="SJC">第二个表要与第一个表关联的字段</param>
        /// <param name="IsLeftOuter">是否是左外连接，否则为内连接</param>
        /// <returns></returns>
        private static DataTable LeftOuterOrJoin(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC, bool IsLeftOuter)
        {

            //创建一个新的DataTable

            DataTable table = new DataTable("Join");
            // Use a DataSet to leverage DataRelation
            using (DataSet ds = new DataSet())
            {

                //把DataTable Copy到DataSet中
                //ds.Tables.AddRange(new DataTable[]{First.Copy(),Second.Copy()});

                DataTable First = FirstTable.Copy();
                DataTable Second = SecondTable.Copy();
                First.TableName = "FirstTable";
                Second.TableName = "SecondTable";
                ds.Tables.AddRange(new DataTable[] { First, Second });
                DataColumn[] parentcolumns = new DataColumn[FJC.Length];
                for (int i = 0; i < parentcolumns.Length; i++)
                {
                    parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
                }

                DataColumn[] childcolumns = new DataColumn[SJC.Length];
                for (int i = 0; i < childcolumns.Length; i++)
                {
                    childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
                }

                //创建关联
                DataRelation r = new DataRelation(string.Empty, parentcolumns, childcolumns, false);
                ds.Relations.Add(r);
                //为关联表创建列
                for (int i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                for (int i = 0; i < Second.Columns.Count; i++)
                {
                    //看看有没有重复的列，如果有在第二个DataTable的Column的列明后加_Second
                    if (!table.Columns.Contains(Second.Columns[i].ColumnName))
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
                    }
                    else
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
                    }
                }
                table.BeginLoadData();
                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //得到行的数据
                    DataRow[] childrows = firstrow.GetChildRows(r);
                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;
                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;
                            object[] joinarray = new object[parentarray.Length + secondarray.Length];
                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                            table.LoadDataRow(joinarray, true);
                        }
                    }
                    else if (IsLeftOuter)
                    {
                            object[] parentarray = firstrow.ItemArray;
                            DataRow secondrow = Second.NewRow();
                            {
                                object[] secondarray = secondrow.ItemArray;
                                object[] joinarray = new object[parentarray.Length + secondarray.Length];
                                Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                                Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                                table.LoadDataRow(joinarray, true);
                            }
                    }
                }
                table.EndLoadData();
            }
            return table;

        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="FirstTable"></param>
       /// <param name="SecondTable"></param>
       /// <param name="FJC"></param>
       /// <param name="SJC"></param>
       /// <returns></returns>
        public static DataTable Join(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC)
        {
            return LeftOuterOrJoin(FirstTable, SecondTable, FJC, SJC, false);
        }
        public static DataTable Join(DataTable First, DataTable Second, DataColumn FJC, DataColumn SJC)
        {
            return Join(First, Second, new DataColumn[] { FJC }, new DataColumn[] { SJC });
        }
        public static DataTable Join(DataTable First, DataTable Second, string FJC, string SJC)
        {
            return Join(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { Second.Columns[SJC] });
        }
        public static DataTable Join(DataTable First, DataTable Second, string []FJC, string []SJC)
        {
            if (First == null || Second == null || FJC == null || SJC == null )
            {
                return null;
            }
            DataColumn[] FJColumns = new DataColumn[FJC.Length];
            for (int i = 0; i < FJC.Length; i++)
            {
                FJColumns[i] = First.Columns[FJC[i].Trim()];
            }
            DataColumn[] SJColumns = new DataColumn[SJC.Length];
            for (int i = 0; i < SJC.Length; i++)
            {
                SJColumns[i] = Second.Columns[SJC[i].Trim()];
            }
            return Join(First, Second, FJColumns, SJColumns);
        }


       
        public static DataTable Join(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC, DataColumn[] SJCNeed)
        {
            return LeftOuterOrJoin(FirstTable, SecondTable, FJC, SJC, SJCNeed, false );
        }
        public static DataTable Join(DataTable First, DataTable Second, string FJC, string SJC, params  string[] SecondNeedS)
        {
            if (First == null || Second == null || FJC == null || SJC == null || SecondNeedS == null)
            {
                return null;
            }
            DataColumn[] SecondNeedScolumns = new DataColumn[SecondNeedS.Length];
            for (int i = 0; i < SecondNeedS.Length; i++)
            {
                SecondNeedScolumns[i] = Second.Columns[SecondNeedS[i].Trim()];
            }
            return Join(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { Second.Columns[SJC] }, SecondNeedScolumns);
        }
        public static DataTable Join(DataTable First, DataTable Second, string[] FJC, string[] SJC ,string[] SecondNeedS)
        {
            if (First == null || Second == null || FJC == null || SJC == null||SecondNeedS==null )
            {
                return null;
            }
            DataColumn[] FJColumns = new DataColumn[FJC.Length];
            for (int i = 0; i < FJC.Length; i++)
            {
                FJColumns[i] = First.Columns[FJC[i].Trim()];
            }
            DataColumn[] SJColumns = new DataColumn[SJC.Length];
            for (int i = 0; i < SJC.Length; i++)
            {
                SJColumns[i] = Second.Columns[SJC[i].Trim()];
            }
            DataColumn[] SecondNeedScolumns = new DataColumn[SecondNeedS.Length];
            for (int i = 0; i < SecondNeedS.Length; i++)
            {
                SecondNeedScolumns[i] = Second.Columns[SecondNeedS[i].Trim()];
            }
            return Join(First, Second, FJColumns, SJColumns, SecondNeedScolumns);
        }


        public static DataTable LeftOuterJoin(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC)
        {
            return LeftOuterOrJoin(FirstTable, SecondTable, FJC, SJC, true );
        }
        public static DataTable LeftOuterJoin(DataTable First, DataTable Second, string FJC, string SJC)
        {
            return LeftOuterJoin(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { Second.Columns[SJC] });
        }
        public static DataTable LeftOuterJoin(DataTable First, DataTable Second, string[] FJC, string[] SJC)
        {
            if (First == null || Second == null || FJC == null || SJC == null)
            {
                return null;
            }
            DataColumn[] FJColumns = new DataColumn[FJC.Length];
            for (int i = 0; i < FJC.Length; i++)
            {
                FJColumns[i] = First.Columns[FJC[i].Trim()];
            }
            DataColumn[] SJColumns = new DataColumn[SJC.Length];
            for (int i = 0; i < SJC.Length; i++)
            {
                SJColumns[i] = Second.Columns[SJC[i].Trim()];
            }
            return LeftOuterJoin(First, Second, FJColumns, SJColumns);
        }

      
        public static DataTable LeftOuterJoin(DataTable FirstTable, DataTable SecondTable, DataColumn[] FJC, DataColumn[] SJC, DataColumn[] SJCNeed)
        {
            return LeftOuterOrJoin(FirstTable, SecondTable, FJC, SJC, SJCNeed, true);
        }
        public static DataTable LeftOuterJoin(DataTable First, DataTable Second, string FJC, string SJC, params  string[] SecondNeedS)
        {
            if (First == null || Second == null || FJC == null || SJC == null || SecondNeedS == null)
            {
                return null;
            }
            DataColumn[] SecondNeedScolumns = new DataColumn[SecondNeedS.Length];
            for (int i = 0; i < SecondNeedS.Length; i++)
            {
                SecondNeedScolumns[i] = Second.Columns[SecondNeedS[i].Trim()];
            }
            return LeftOuterJoin(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { Second.Columns[SJC] }, SecondNeedScolumns);
        }
        public static DataTable LeftOuterJoin(DataTable First, DataTable Second, string[] FJC, string[] SJC, string[] SecondNeedS)
        {
            if (First == null || Second == null || FJC == null || SJC == null || SecondNeedS == null)
            {
                return null;
            }
            DataColumn[] FJColumns = new DataColumn[FJC.Length];
            for (int i = 0; i < FJC.Length; i++)
            {
                FJColumns[i] = First.Columns[FJC[i].Trim()];
            }
            DataColumn[] SJColumns = new DataColumn[SJC.Length];
            for (int i = 0; i < SJC.Length; i++)
            {
                SJColumns[i] = Second.Columns[SJC[i].Trim()];
            }
            DataColumn[] SecondNeedScolumns = new DataColumn[SecondNeedS.Length];
            for (int i = 0; i < SecondNeedS.Length; i++)
            {
                SecondNeedScolumns[i] = Second.Columns[SecondNeedS[i].Trim()];
            }
            return LeftOuterJoin(First, Second, FJColumns, SJColumns, SecondNeedScolumns);
        }

        /// <summary>
        /// 将Datatable封装成泛型
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<DataRow> TableToIEnumerable(DataTable table)
        {
            return new LinqList<DataRow>(table.Rows);
        }
        private static bool RowEqual(DataRow rowA, DataRow rowB, DataColumnCollection columns)
        {
          //  bool result = true;
            for (int i = 0; i < columns.Count; i++)
            {
                //result &= ColumnEqual(rowA[columns[i].ColumnName], rowB[columns[i].ColumnName]);
                if (!ColumnEqual(rowA[columns[i].ColumnName], rowB[columns[i].ColumnName]))
                {
                    return false;
                }
            }
            return true ;
        }

        private static bool ColumnEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value && objectB == DBNull.Value)
            {
                return true;
            }
            if (objectA == DBNull.Value || objectB == DBNull.Value)
            {
                return false;
            }
            return (objectA.Equals(objectB));
        }


        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName1,fieldName2,fieldNamen from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public static DataTable SelectDistinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            DataTable dt = new DataTable(tableName);
            object[] values = new object[fieldNames.Length];
            string fields = "";
            for (int i = 0; i < fieldNames.Length; i++)
            {
                dt.Columns.Add(fieldNames[i], sourceTable.Columns[fieldNames[i]].DataType);
                fields += fieldNames[i] + ",";
            }
            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;
            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values[i] = dr[fieldNames[i]];
                    }
                    dt.Rows.Add(values);
                }
            }
            return dt;
        }
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName1,fieldName2,fieldNamen from sourceTable 
        /// </summary> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public static DataTable SelectDistinct( DataTable sourceTable, string[] fieldNames)
        {
            DataTable dt = new DataTable();
            object[] values = new object[fieldNames.Length];
            string fields = "";
            for (int i = 0; i < fieldNames.Length; i++)
            {
                dt.Columns.Add(fieldNames[i], sourceTable.Columns[fieldNames[i]].DataType);
                fields += fieldNames[i] + ",";
            }
            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;
            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values[i] = dr[fieldNames[i]];
                    }
                    dt.Rows.Add(values);
                }
            }
            return dt;
        }

        /// <summary>
        /// 根据表的三个个字段 去掉重复行 2011-12-12
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filed1"></param>
        /// <param name="filed2"></param>
        /// <param name="filed3"></param>
        static public DataTable GetTable(DataTable dt, string filed1, string filed2, string filed3)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            bool Have = false;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Have = false;
                    for (int j = i + 1; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[i][filed1].ToString().Trim() == dt.Rows[j][filed1].ToString().Trim() && dt.Rows[i][filed2].ToString().Trim() == dt.Rows[j][filed2].ToString().Trim() && dt.Rows[i][filed3].ToString().Trim() == dt.Rows[j][filed3].ToString().Trim())
                        {
                            Have = true;
                            break;
                        }
                    }
                    if (!Have)
                    {
                        newdt.ImportRow(dt.Rows[i]);
                    }
                }

            }
            return newdt;
        }

        /// <summary>
        /// 查询DataTable中符合条件的内容给新的dt
        /// </summary>
        /// <param name="srcdt"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static DataTable dt_Select(DataTable srcdt, string conditions)
        {
            try
            {
                DataView dv = new DataView(srcdt);
                dv.RowFilter = conditions;
                return dv.ToTable();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcdt"></param>
        /// <param name="conditions"></param>
        /// <param name="newTableName"></param>
        /// <returns></returns>
        public static DataTable dt_Select(DataTable srcdt, string conditions, string newTableName)
        {
            try
            {
                DataView dv = new DataView(srcdt);
                dv.RowFilter = conditions;
                string tableName = srcdt.TableName;
                DataTable dt = dv.ToTable();
                dt.TableName = newTableName;
                return dt;
            }
            catch
            {
                return null;
            }
        }

    }
    public class LinqList<T> : IEnumerable<T>, IEnumerable
    {
        IEnumerable items;
        internal LinqList(IEnumerable items)
        {
            this.items = items;
        }
        #region IEnumerable<DataRow> Members
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (T item in items) yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable<T> ie = this;
            return ie.GetEnumerator();
        }
        #endregion

    }

    public static class Projecttable
    {
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                // if (!dp.IsReadOnly)
                ret.Columns.Add(dp.Name, dp.PropertyType);
            foreach (T item in array)
            {
                var Row = ret.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                    // if (!dp.IsReadOnly)
                    Row[dp.Name] = dp.GetValue(item);
                ret.Rows.Add(Row);
            }
            return ret;
        }
    }
}
