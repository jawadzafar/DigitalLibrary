using System.ArrayExtensions;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Data;

namespace System
{

    public class DontInsert : Attribute{}
    public class DontUpdate : Attribute{}
    public class DontLoad : Attribute { }
    public class Column: Attribute
    {
        public string Name { get; set; }
        public Column(string Name)
        {
            this.Name = Name;
        }
    }
    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(String)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }

        public static Object Copy(this Object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
        }
        private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
        {
            if (originalObject == null) return null;
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) return originalObject;
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T Copy<T>(this T original)
        {
            return (T)Copy((Object)original);
        }
    }
    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    namespace ArrayExtensions
    {
        public static class ArrayExtensions
        {
            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.LongLength == 0) return;
                ArrayTraverse walker = new ArrayTraverse(array);
                do action(array, walker.Position);
                while (walker.Step());
            }
        }

        internal class ArrayTraverse
        {
            public int[] Position;
            private int[] maxLengths;

            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    maxLengths[i] = array.GetLength(i) - 1;
                }
                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < Position.Length; ++i)
                {
                    if (Position[i] < maxLengths[i])
                    {
                        Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }


    public class Database_Helpers
    {

        //Properties//
        public string Query { get; set; }
        public Dictionary<string, string> values = new Dictionary<string, string>();
        public SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
        public Dictionary<string, string> LogEntry = new Dictionary<string, string>();
        //End Properties//

        #region QueryGenerators
        private string GetColumnName(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(Column)))
            {
                Column k = (Column)Attribute.GetCustomAttribute(pi, typeof(Column));
                return k.Name;
            }
            else
            {
                return pi.Name;
            }

        }
        private string ValueReader(Object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else if (value.GetType() == typeof(DateTime))
            {
                DateTime _date = (DateTime)value;
                return _date.ToString("yyyy-MM-dd hh:mm:ss");
            }
            else
            {
                return value.ToString().Replace("\'", "'\'\'");
            }
        }

        public string InsertQuery(string Table, Object _Object)
        {
            List<PropertyInfo> _properties = _Object.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontInsert)))).ToList();
            string query = string.Empty;
            string ColumnNames = string.Empty;
            string Values = string.Empty;
            query = "INSERT INTO [" + Table + "] ";
            ColumnNames = "(";
            Values = " VALUES (";
            foreach (var pi in _properties)
            {
                ColumnNames += "[" + GetColumnName(pi) + "], ";
                Values += "'" + ValueReader(pi.GetValue(_Object)) + "', ";
            }
            ColumnNames = ColumnNames.TrimEnd(' ').TrimEnd(',');
            ColumnNames += ")";
            Values = Values.TrimEnd(' ').TrimEnd(',') + ")";
            query += ColumnNames;
            query += Values;
            return query;
        }
        public string InsertQuery(string Table, Dictionary<string, string> dataset)
        {
            string query = string.Empty;
            query += "Insert into dbo.[" + Table + "] (";
            foreach (var col in dataset)
            {
                query += col.Key.ToString() + ",";
            }
            query = query.TrimEnd(',');
            query += ") VALUES(";
            foreach (var val in dataset)
            {
                query += "'" + val.Value.Replace("\'", "\'\'").ToString() + "',";
            }
            query = query.TrimEnd(',');
            query += ");";
            return query;

        }

        
        public string UpdateQuery(string Table, Object _Object, string WhereClause)
        {
            List<PropertyInfo> _properties = _Object.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly).ToList().Where(x => !(Attribute.IsDefined(x, typeof(DontUpdate)))).ToList();
            string query = "UPDATE [" + Table + "] SET ";
            foreach (var pi in _properties)
            {
                query += "[" + GetColumnName(pi) + "] = '" + ValueReader(pi.GetValue(_Object)) + "', ";
            }
            query = query.TrimEnd(' ').TrimEnd(',');
            query += " " + WhereClause;
            return query;
        }
        public string UpdateQuery(string Table, Dictionary<string, string> dataset, string where_clause)
        {
            string query = string.Empty;
            query += "Update [" + Table + "] SET ";
            int count = 0;
            foreach (var col in dataset)
            {
                query += col.Key + " = '" + col.Value.Replace("\'", "\'\'") + "'";

                count++;
                if (count < dataset.Values.Count)
                {
                    query = query + " , ";
                }

            }
            query = query.TrimEnd(',');
            query += " " + where_clause + ";";
            return query;
        }
        #endregion
        #region InsertFunctions
        public bool Insert(string Table, Object _Object)
        {
            string query = InsertQuery(Table, _Object);
            return this.ExecuteQuery(query);

        }
        public bool insert(string table_name, Dictionary<string, string> dataset)
        {
            string query = InsertQuery(table_name, dataset);
            dataset.Clear();
            return this.ExecuteQuery(query);

        }
        public void TransInsert(string Table, Object _Object)
        {
            Query += InsertQuery(Table, _Object);
        }
        public bool BulkInsert(string Table, List<Object> myList)
        {
            string query = string.Empty;
            foreach (var element in myList)
            {
                query += InsertQuery(Table, element);
            }
            return ExecuteQuery(query);
        }

        public bool BulkInsert(string Table, List<Dictionary<string,string>> myList)
        {
            string query = string.Empty;
            foreach (var element in myList)
            {
                query += InsertQuery(Table, element);
            }
            return ExecuteQuery(query);
        }

        public void TransInsert(string table_name, Dictionary<string, string> dataset)
        {
            Query += InsertQuery(table_name, dataset);
            dataset.Clear();

        }
        public string insert_and_get_id(string table_name, Dictionary<string, string> dataset)
        {
            string query = InsertQuery(table_name, dataset);
            query += " Select Scope_Identity()";
            try
            {
                int id = 0;
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                Connection.Close();
                return id.ToString();
            }
            catch (Exception ex)
            {
                Connection.Close();
                return "failed " + ex.Message;
            }

        }

        #endregion
        #region UpdateFunctions
        public bool Update(string Table, Object _Object, string WhereClause)
        {
            string query = UpdateQuery(Table, _Object, WhereClause);
            return this.ExecuteQuery(query);
        }
        public bool update(string table_name, Dictionary<string, string> dataset, string where_clause)
        {
            string query = UpdateQuery(table_name, dataset, where_clause);
            dataset.Clear();
            return this.ExecuteQuery(query);
        }
        public void TransUpdate(string table_name, Dictionary<string, string> dataset, string where_clause)
        {
            Query += UpdateQuery(table_name, dataset, where_clause);
            dataset.Clear();
        }
        public void TransUpdate(string Table, Object _Object, string WhereClause)
        {
            Query += UpdateQuery(Table, _Object, WhereClause);
        }
        #endregion
        #region DeleteFunctions
        public void TransDelete(string table_name, string where_clause)
        {
            Query += "Delete from [" + table_name + "] " + where_clause + ";";
        }

        public bool delete(string table_name, string where_clause)
        {
            string query = "Delete from [" + table_name + "] " + where_clause + ";";
            return this.ExecuteQuery(query);
        }
        #endregion
        #region OtherFunctions
        public static void Initialize(Object obj)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(obj);
                    if (value == null)
                    {
                        pi.SetValue(obj, "");
                    }
                }
            }
        }
        public void BeginTrans()
        {
            Query = string.Empty;
            Query += "BEGIN TRY ";
            Query += "BEGIN TRANSACTION ";
        }
        public bool CommitTrans()
        {
            Query += "COMMIT ";
            Query += "END TRY ";
            Query += "BEGIN CATCH ";
            Query += "declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;";
            Query += "select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();";
            Query += "rollback transaction;";
            Query += "raiserror(@ErrorMessage, @ErrorSeverity, @ErrorState);";
            Query += "END CATCH ";
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(Query, Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();
                Query = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                Query = string.Empty;
                return false;
            }

        }
        public bool TransLog(string type, string description, string performed_by, string performed_on)
        {
            Query += "Insert into log(type,description,performed_by,performed_on) Values('" + type + "','" + description + "','" + performed_by + "','" + performed_on + "')";
            return true;
        }
        public bool ExecuteQuery(string query)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }
        }

        public bool log(string type, string description, string performed_by, string performed_on)
        {
            try
            {
                string query = "Insert into log(type,description,performed_by,performed_on) Values(@type,@desc,@pb,@po)";
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@pb", performed_by);
                cmd.Parameters.AddWithValue("@po", performed_on);
                cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }
        }
        #endregion

        #region ReadFunctions
        public static List<Dictionary<string, string>> QueryList(string query, SqlConnection cn)
        {
            List<Dictionary<string, string>> _appList = new List<Dictionary<string, string>>();
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Dictionary<string, string> _app = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        _app.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                    }
                    _appList.Add(_app);
                }
            }
            reader.Close();
            return _appList;
        }
        public static List<Dictionary<string, string>> QueryList(string query)
        {
            List<Dictionary<string, string>> _appList = new List<Dictionary<string, string>>();
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> _app = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            _app.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        _appList.Add(_app);
                    }
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Connection.Close();
            }
            return _appList;
        }

        public static List<Dictionary<string, string>> PagedQuery(string query, string SortBy, string Order, int NumberOfRecords, int PageNumber)
        {
            int offset = NumberOfRecords * PageNumber;
            query += " order by " + SortBy;
            query += " desc ";
            query += " offset " + offset + " rows fetch next " + NumberOfRecords + " rows only";

            List<Dictionary<string, string>> _appList = new List<Dictionary<string, string>>();
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> _app = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            _app.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        _appList.Add(_app);
                    }
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Connection.Close();
            }
            return _appList;
        }

        public static Dictionary<string, string> QueryRecord(string query)
        {
            Dictionary<string, string> _appList = new Dictionary<string, string>();
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            _appList.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        break;
                    }
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Connection.Close();
            }
            return _appList;
        }

        public static List<string> QueryColumn(string query)
        {
            List<string> _List = new List<string>();
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _List.Add(reader[reader.GetName(0)].ToString());
                    }
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Connection.Close();
            }
            return _List;


        }

        public bool Load(Object _Object, string query)
        {

           List<PropertyInfo> _properties = _Object.GetType().GetProperties().Where(x => !(Attribute.IsDefined(x, typeof(DontLoad)))).ToList();

            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        foreach (PropertyInfo pi in _properties)
                        {
                            if (reader[pi.Name] == null)
                            {
                                pi.SetValue(_Object, null);
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                string value = reader[pi.Name].ToString();
                                pi.SetValue(_Object, value);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(_Object, (bool)reader[pi.Name]);
                            }
                            else if (pi.PropertyType == typeof(double))
                            {
                                pi.SetValue(_Object, double.Parse(reader[pi.Name].ToString()));
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                pi.SetValue(_Object, (int)reader[pi.Name]);
                            }
                            else if (pi.PropertyType == typeof(DateTime))
                            {
                                DateTime _date = new DateTime();
                                DateTime.TryParse(reader[pi.Name].ToString(), out _date);
                                pi.SetValue(_Object, _date);
                            }
                        }
                    }

                    Connection.Close();
                    return true;
                }
                else
                {
                    Connection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }
        }
        public List<Object> LoadList(Object _Object, string query)
        {
            List<Object> _newList = new List<Object>();
            PropertyInfo[] _properties = _Object.GetType().GetProperties();
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        foreach (PropertyInfo pi in _properties)
                        {
                            if (pi.PropertyType == typeof(string))
                            {
                                string value = reader[pi.Name].ToString();
                                pi.SetValue(_Object, value);
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                pi.SetValue(_Object, (int)reader[pi.Name]);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(_Object, (bool)reader[pi.Name]);
                            }
                            else if (pi.PropertyType == typeof(DateTime))
                            {
                                DateTime _date = new DateTime();
                                DateTime.TryParse(reader[pi.Name].ToString(), out _date);
                                pi.SetValue(_Object, _date);
                            }
                        }
                        _newList.Add(_Object.Copy());
                    }
                }
                Connection.Close();
                return _newList;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return _newList;
            }
        }
        public int get_scalar(string query)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return 0;
            }
        }





        public string get_scalar_string(string query)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                string result = cmd.ExecuteScalar().ToString();
                Connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return "";
            }
        }

        public DataTable ReadDataTable(string query)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
            finally
            {
                Connection.Close();
                sda.Dispose();
                Connection.Dispose();
            }
        }
        #endregion
    }
}