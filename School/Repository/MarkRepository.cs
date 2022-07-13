using Dapper;
using Microsoft.Extensions.Configuration;
using School.IRepository;
using School.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace School.Repository
{
    public class MarkRepository : IMarkRepository
    {
        string _connectionString = "";
        Mark _mark = new Mark();
        List<Mark> listMark = new List<Mark>();

        public MarkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SchoolDB");
        }
        public async Task<string> Delete(Mark obj)
        {
            string Message = "";
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var Student = await con.QueryAsync<Mark>("SP_Mark", this.SetParameters(obj, (int)OperationType.Delete), commandType: CommandType.StoredProcedure);
                    Message = "Deleted successful";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        public async Task<List<Mark>> Get(int StudentID)
        {
            //throw new NotImplementedException();
            listMark = new List<Mark>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Marks = await con.QueryAsync<Mark>(string.Format(@"SELECT * FROM Marks WHERE StudentId = {0}", StudentID));
                if (Marks != null && Marks.Count() > 0)
                {
                    listMark = Marks.ToList();
                }
            }
            return listMark;
        }


        public async Task<List<Mark>> Gets()
        {
            listMark = new List<Mark>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Marks = await con.QueryAsync<Mark>(string.Format(@"SELECT * FROM Marks"));
                if (Marks != null && Marks.Count() > 0)
                {
                    listMark = Marks.ToList();
                }
            }
            return listMark;
        }

        public async Task<Mark> Save(Mark obj)
        {
            _mark = new Mark();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var marks = await con.QueryAsync<Mark>("SP_Mark", this.SetParameters(obj, (int)OperationType.Insert), commandType: CommandType.StoredProcedure);
                if (marks != null && marks.Count() > 0)
                {
                    _mark = marks.FirstOrDefault();
                }
            }

            return _mark;
        }

        private DynamicParameters SetParameters(Mark mark, int OperateType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StudentId", mark.StudentId);
            parameters.Add("@Subject", mark.Subject);
            parameters.Add("@Marks", mark.Marks);
            parameters.Add("@OperationType", OperateType);
            return parameters;
        }
    }
}
