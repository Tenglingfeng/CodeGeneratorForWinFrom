﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dapper;
using MySql.Data.MySqlClient;

namespace CodeGenerator.DbHelper
{
    public static class DbHelper
    {
        public static IEnumerable<string> GetTables(string dbName, string connString)
        {
            using IDbConnection conn = new MySqlConnection(connString);
            var sql = $"select table_name from information_schema.tables where table_schema='{dbName}' and table_type='base table';";
            var result = conn.Query<string>(sql);
            conn.Close();
            conn.Dispose();
            return result;
        }

        /// <summary>
        /// 获取表字段,注释,描述,类型信息
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<InformationSchema> GetInformationSchema(string connString, string tableName)
        {
            using IDbConnection conn = new MySqlConnection(connString);
            var sql = $@"
                                SELECT
	                                `information_schema`.`COLUMNS`.`COLUMN_NAME`,
	                                `information_schema`.`COLUMNS`.`DATA_TYPE`,
	                                `information_schema`.`COLUMNS`.`COLUMN_COMMENT`
                                FROM
	                                `information_schema`.`COLUMNS`
                                WHERE
                                table_name = '{tableName}'
                            ";
            var sql2 = $@"SELECT
	                                information_schema.`TABLES`.TABLE_COMMENT
                                FROM
	                                `information_schema`.`TABLES`
                                WHERE
	                                table_name = '{tableName}'";

            var tableInfoList = new List<InformationSchema>();
            var reader = conn.ExecuteReader(sql);

            while (reader.Read())
            {
                var tableInfo = new InformationSchema()
                {
                    ColumnName = (reader["COLUMN_NAME"].ToString()),
                    DataType = (reader["DATA_TYPE"].ToString()),
                    ColumnComment = reader["COLUMN_COMMENT"].ToString()
                };
                tableInfoList.Add(tableInfo);
            }
            reader.Close();
            reader.Dispose();
            var tableComment = conn.QueryFirstOrDefault<string>(sql2);
            tableInfoList.ForEach(x => { x.TableComment = tableComment; });
            reader.Close();
            reader.Dispose();
            return tableInfoList;
        }
    }
}