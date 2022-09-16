using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class PagedRequest
    {
        private const int maxPageSize = 50; // to restrict our API to a maximum of 50 records.

        // We have two public properties – pageNumber and pageSize. If not set by the caller, pageNumber will be set to 1, and pageSize to 10.

        public int pageNumber = 1;
        public int PageNumber
        {
            get { return pageNumber; }

            set { pageNumber = (value == default(int)) ? pageNumber : value; }
        }


        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }

            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
