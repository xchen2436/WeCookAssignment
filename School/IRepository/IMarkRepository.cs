using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.IRepository
{
    public interface IMarkRepository
    {
        Task<Mark> Save(Mark obj);
        Task<List<Mark>> Get(int obj);
        Task<List<Mark>> Gets();
        Task<string> Delete(Mark obj);
    }
}
