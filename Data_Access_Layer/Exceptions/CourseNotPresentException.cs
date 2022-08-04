using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Exceptions
{
    public class CourseNotPresentException : Exception
    {
        public override string Message
        {
            get
            {
                return "No such course is present";
            }
        }
    }
}
