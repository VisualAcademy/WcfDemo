﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DotNetSale.Wcf.Models
{
    /// <summary>
    /// CategoryBases 테이블에 대한 CRUD 코드 구현 리포지토리 클래스
    /// </summary>
    public class CategoryBaseRepository : ICategoryBaseRepository
    {
        // 데이터베이스 연결 문자열 
        private static string _connectionString = @"
            Server=(localdb)\mssqllocaldb;
            Database=DotNetSaleCom;
            Integrated Security=True;
        ";

        /// <summary>
        /// Add(): 입력
        /// </summary>
        public CategoryBase Add(CategoryBase model)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Insert Into CategoryBases Values(@CategoryName);";
            cmd.CommandType = CommandType.Text;

            //[1] SqlParameter 클래스의 인스턴스 생성
            SqlParameter categoryName =
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
            //[2] Value 속성으로 값 지정
            categoryName.Value = model.CategoryName;
            //[3] 커멘트 개체에 매개 변수 추가
            cmd.Parameters.Add(categoryName);

            cmd.ExecuteNonQuery();

            con.Close();

            return model;
        }

        /// <summary>
        /// Browse(): 상세
        /// </summary>
        public CategoryBase Browse(int id)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"
                Select CategoryId, CategoryName 
                From CategoryBases 
                Where CategoryId = @CategoryId
            ";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CategoryId", id);

            //[1] SqlDataReader 형식의 개체로 결괏값 받기
            SqlDataReader dr = cmd.ExecuteReader();

            //[2] Read() 메서드로 데이터 있는만큼 반복
            CategoryBase category = null;
            if (dr.Read())
            {
                var categoryId = dr.GetInt32(0);
                var categoryName =
                    (dr["CategoryName"] == DBNull.Value) ? "" : dr.GetString(1);

                // 개체 리터럴을 통해서 값 채우기
                category = new CategoryBase
                {
                    CategoryId = categoryId,
                    CategoryName = categoryName
                };
            }

            //[3] Close() 메서드로 연결된 리더 개체 종료
            dr.Close();

            con.Close();

            return category;
        }

        /// <summary>
        /// Delete(): 삭제
        /// </summary>
        public bool Delete(int id)
        {
            bool result = false;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    string sql = 
                        "Delete CategoryBases Where CategoryId = @CategoryId";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CategoryId", id);

                    int count = cmd.ExecuteNonQuery(); // 영향받은 레코드 수 반환 
                    if (count > 0)
                    {
                        result = true; // 삭제 완료
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Edit(): 수정
        /// </summary>
        public bool Edit(CategoryBase model)
        {
            bool result = false;

            string sql = @"
                Update CategoryBases 
                Set CategoryName = @CategoryName 
                Where CategoryId = @CategoryId
            ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);

                int count = cmd.ExecuteNonQuery(); // 영향받은 레코드 수 반환 
                if (count > 0)
                {
                    result = true; // 업데이트 완료
                }
            }

            return result;
        }
        
        /// <summary>
        /// Has(): 건수
        /// </summary>
        public int Has()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();

            //[1] SqlCommand의 인스턴스 생성
            SqlCommand cmd = new SqlCommand();
            //[2] Connection 속성 지정
            cmd.Connection = con;
            //[3] CommandText 속성 설정: DDL + DML
            cmd.CommandText = "Select Count(*) From CategoryBases";
            //[4] CommandType 속성 지정
            cmd.CommandType = CommandType.Text;
            //[5] ExecuteXXX() 메서드로 실행
            object result = cmd.ExecuteScalar(); // 스칼라 값(단일 값) 반환

            con.Close();

            if (int.TryParse(result.ToString(), out int count))
            {
                return count;
            }
            return 0;
        }

        /// <summary>
        /// Log(): 로그
        /// </summary>
        public void Log(string message)
        {
            var table = "LogBases";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();

            //[1] SqlCommand의 인스턴스 생성
            SqlCommand cmd = new SqlCommand();
            //[2] Connection 속성 지정
            cmd.Connection = con;
            //[3] CommandText 속성 설정: 모든 SQL(DDL + DML) 또는 저장 프로시저 이름
            cmd.CommandText = // 동적으로 모든 SQL 문을 만들어 호출 가능함
                $"Insert Into {table} (Message) Values(N'" + message + "');";
            //[4] CommandType 속성 지정
            cmd.CommandType = CommandType.Text;
            //[5] ExecuteXXX() 메서드로 실행
            cmd.ExecuteNonQuery(); // 실행 완료

            con.Close();
        }

        /// <summary>
        /// Ordering(): 정렬
        /// </summary>
        public IEnumerable<CategoryBase> Ordering(OrderOption orderOption)
        {
            List<CategoryBase> categories = new List<CategoryBase>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string sql = @" Select Top 1000 * From CategoryBases ";

                switch (orderOption)
                {
                    case OrderOption.Ascending:
                        sql += " Order By CategoryName Asc ";
                        break;
                    case OrderOption.Descending:
                        sql += " Order By CategoryName Desc ";
                        break;
                    default:
                        sql += "";
                        break;
                }

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;

                // CommandBehavior.CloseConnection: 데이터리더 닫힐 때 커넥션도 닫힘
                SqlDataReader dr = 
                    cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //[!] Read() 메서드로 데이터 있는만큼 반복
                while (dr.Read())
                {
                    var categoryId = dr.GetInt32(0);
                    var categoryName =
                        (dr["CategoryName"] == DBNull.Value) ? "" : dr.GetString(1);

                    // 개체 리터럴을 통해서 값 채우기
                    categories.Add(new CategoryBase
                    {
                        CategoryId = categoryId,
                        CategoryName = categoryName
                    });
                }

                dr.Close();
            }

            return categories;
        }

        /// <summary>
        /// Paging(): 페이징
        /// </summary>
        public List<CategoryBase> Paging(int pageNumber, int pageSize)
        {
            List<CategoryBase> categories = new List<CategoryBase>();

            string sql = " Select * From CategoryBases Order By CategoryId Asc ";

            sql += @"
                Offset ((@PageNumber - 1) * @PageSize) Rows 
                Fetch Next @PageSize Rows Only
            ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@PageNumber", pageNumber);
                da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);
                DataSet ds = new DataSet();
                da.Fill(ds, "CategoryBases");

                //[!] 출력
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var categoryId = Convert.ToInt32(
                        ds.Tables[0].Rows[i]["CategoryId"].ToString());
                    var categoryName = 
                        ds.Tables[0].Rows[i]["CategoryName"].ToString();

                    categories.Add(new CategoryBase {
                        CategoryId = categoryId, CategoryName = categoryName });
                }
            }

            return categories;
        }
        
        /// <summary>
        /// Read(): 출력
        /// </summary>
        public List<CategoryBase> Read()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = 
                "Select Top 1000 CategoryId, CategoryName From CategoryBases";
            cmd.CommandType = CommandType.Text;

            //[1] DataAdapter
            SqlDataAdapter da = new SqlDataAdapter();
            //[2] SelectCommand 지정
            da.SelectCommand = cmd;
            //[3] DataSet: 메모리상의 데이터베이스
            DataSet ds = new DataSet();
            //[4] Fill() 메서드로 DataSet 채우기
            da.Fill(ds, "CategoryBases");

            //[!] 출력
            List<CategoryBase> categories = new List<CategoryBase>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var categoryId = 
                    Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryId"].ToString());
                var categoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();

                categories.Add(new CategoryBase {
                    CategoryId = categoryId, CategoryName = categoryName });
            }

            con.Close(); // DataAdapter + DataSet 클래스 사용할 때에는 생략 가능

            return categories;
        }

        /// <summary>
        /// Search(): 검색
        /// </summary>
        public List<CategoryBase> Search(string query)
        {
            List<CategoryBase> categories = new List<CategoryBase>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string sql = @"
                    Select * From CategoryBases 
                    Where CategoryName Like '%' + @SearchQuery + '%'
                ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SearchQuery", query);

                SqlDataReader dr = cmd.ExecuteReader();
                //[!] Read() 메서드로 데이터 있는만큼 반복
                while (dr.Read())
                {
                    var categoryId = dr.GetInt32(0);
                    var categoryName =
                        (dr["CategoryName"] == DBNull.Value) ? "" : dr.GetString(1);

                    // 개체 리터럴을 통해서 값 채우기
                    categories.Add(new CategoryBase
                    {
                        CategoryId = categoryId,
                        CategoryName = categoryName
                    });
                }

                dr.Close();
            }

            return categories;
        }
    }
}
