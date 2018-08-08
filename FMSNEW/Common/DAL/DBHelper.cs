using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Common.DAL
{
    public class DBHelper
    {
        #region
        SqlConnection Conn;
        SqlCommand Cmd;
        SqlDataReader dr;
        SqlTransaction oratran;
        #endregion

        /// <summary>
        /// SQL命令
        /// </summary>
        public string strCmd
        {
            get;
            set;
        }

        public DBHelper()
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            Cmd = Conn.CreateCommand();
            Cmd.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">结果集元素类型</typeparam>
        /// <returns>List结果集</returns>
        public List<T> Reader<T>() where T : new()
        {
            Cmd.CommandText = strCmd;
            Open();
            dr = Cmd.ExecuteReader();
            T t = new T();
            List<T> lst = new List<T>();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            List<string> cols = dr.GetSchemaTable().AsEnumerable().Select(r => r.Field<string>("ColumnName")).ToList();
            while (dr.Read())
            {
                t = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    if (cols.Contains(pi.Name) && pi.CanWrite)
                    {
                        object value = dr[pi.Name.ToUpper()];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);
                        }
                    }
                }
                lst.Add(t);
            }
            dr.Close();
            Close();
            return lst;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns>DataReader</returns>
        public DbDataReader Reader()
        {
            Cmd.CommandText = strCmd;
            Open();
            return Cmd.ExecuteReader();
        }

        /// <summary>
        /// 读取单列数据
        /// </summary>
        /// <typeparam name="T">结果集元素类型</typeparam>
        /// <param name="colName">需读取数据的字段名</param>
        /// <returns>List结果集</returns>
        public List<T> Singel_Colume_Reader<T>(string colName)
        {
            Cmd.CommandText = strCmd;
            Open();
            dr = Cmd.ExecuteReader();
            T t;
            List<T> lst = new List<T>();
            while (dr.Read())
            {
                t = (T)dr[colName];
                lst.Add(t);
            }
            dr.Close();
            Close();
            return lst;
        }

        /// <summary>
        /// 无结果集操作
        /// </summary>
        public void NonQuery()
        {
            Cmd.CommandText = strCmd;
            Open();
            if (oratran != null)
            {
                Cmd.Transaction = oratran;
            }
            Cmd.ExecuteNonQuery();
            Close();
        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <returns>object结果</returns>
        public object Scalar()
        {
            object obj;
            Cmd.CommandText = strCmd;
            Open();
            obj = Cmd.ExecuteScalar();
            Close();
            return obj;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="type">数据类型</param>
        /// <param name="dir"> 参数方向</param> 
        /// <param name="size">参数值长度(0)</param>
        /// <param name="value">参数值(null)</param>
        public void AddPare(string name, SqlDbType type, ParameterDirection dir, int size, object value)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = name;
            para.SqlDbType = type;
            para.Direction = dir;
            if (size != 0)
            {
                para.Size = size;
            }
            if (value != null)
            {
                para.Value = value;
            }
            Cmd.Parameters.Add(para);
        }

        public void AddPare(string name, SqlDbType type, int size, object value)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = name;
            para.SqlDbType = type;
            para.Direction = ParameterDirection.Input;
            if (size != 0)
            {
                para.Size = size;
            }
            if (value != null)
            {
                para.Value = value;
            }
            Cmd.Parameters.Add(para);
        }

        /// <summary>
        /// 清空参数
        /// </summary>
        public void CleanPara()
        {
            Cmd.Parameters.Clear();
        }

        /// <summary>
        /// 修改参数值
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public void ChangeParaValue(string paraName, object value)
        {
            Cmd.Parameters[paraName].Value = value;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="pname">参数名</param>
        /// <returns>参数值</returns>
        public object GetParaValue(string pname)
        {
            return Cmd.Parameters[pname].Value;
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns>Bool</returns>
        public bool TestConnection()
        {
            try
            {
                Open();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        private void Open()
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close()
        {
            if (Conn.State != ConnectionState.Closed && oratran == null)
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            Open();
            oratran = Conn.BeginTransaction();

        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            oratran.Commit();
            Conn.Close();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTran()
        {
            oratran.Rollback();
            Conn.Close();
        }
    }
}
