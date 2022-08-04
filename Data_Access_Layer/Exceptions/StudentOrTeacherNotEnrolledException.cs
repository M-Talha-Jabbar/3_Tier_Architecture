using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Exceptions
{
    public class StudentOrTeacherNotEnrolledException : Exception
    {
        public override string Message
        {
            get
            {
                return "No such person is enrolled";
            }
        }
    }
}
