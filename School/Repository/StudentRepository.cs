using Dapper;
using Microsoft.Extensions.Configuration;
using School.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace School.Repository
{
    public class StudentRepository : IStudentRepository
    {
        string _connectionString = "";
        Student _student = new Student();
        List<Student> liststudent = new List<Student>();

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SchoolDB");
        }
        public async Task<string> Delete(Student obj)
        {
            //throw new NotImplementedException();
            string Message = "";
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var Student = await con.QueryAsync<Student>("SP_Student", this.SetParameters(obj,(int)OperationType.Delete), commandType: CommandType.StoredProcedure);
                    Message = "Deleted successful";
                }
            }catch(Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        

        public async Task<List<Student>> Gets()
        {
            //throw new NotImplementedException();
            liststudent = new List<Student>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Students = await con.QueryAsync<Student>(string.Format(@"SELECT * FROM Students"));
                if (Students != null && Students.Count() > 0)
                {
                    liststudent = Students.ToList();
                }
            }
            return liststudent;
        }

        public async Task<Student> GetStudent(Student student)
        {
            //throw new NotImplementedException();
            _student = new Student();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string sql = string.Format(@"SELECT * FROM [Students] WHERE Username = '{0}' AND Password = '{1}'", student.Username, student.Password);
                var Students = await con.QueryAsync<Student>(sql);
                if (Students != null && Students.Count() > 0)
                {
                    _student = Students.SingleOrDefault();
                }
            }
            return _student;
        }

        public async Task<Student> Save(Student obj)
        {
            //throw new NotImplementedException();
            _student = new Student();
            int operation = Convert.ToInt32(obj.StudentId == 0 ? OperationType.Insert : OperationType.Update);
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Students = await con.QueryAsync<Student>("SP_Student", this.SetParameters(obj, operation),commandType:CommandType.StoredProcedure);
                if (Students != null && Students.Count() > 0)
                {
                    _student = Students.FirstOrDefault();
                }
            }

            return _student;
        }
        private DynamicParameters SetParameters(Student student,int OperateType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StudentId", student.StudentId);
            parameters.Add("@Name", student.Name);
            parameters.Add("@Username", student.Username);
            parameters.Add("@Password", student.Password);
            parameters.Add("@Class", student.Class);
            parameters.Add("@OperationType", OperateType);
            return parameters;
        }
    }
}
