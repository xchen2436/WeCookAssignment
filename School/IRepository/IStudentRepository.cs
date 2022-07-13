using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.IRepository
{
    public interface IStudentRepository
    {
        Task<Student> Save(Student obj);
        Task<List<Student>> Gets();
        Task<Student> GetStudent(Student student);
        Task<string> Delete(Student obj);

    }
}
