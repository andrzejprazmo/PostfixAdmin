using SEE.PostfixAdmin.BackEnd.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.List
{
    public class DataRequest
    {
        #region paging

        /// <summary>
        /// Total number of records found
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Number of records on single page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Current page number (zero based)
        /// </summary>
        public int PageIndex { get; set; }

        // Number of pages
        public int NumOfPages
        {
            get
            {
                if (PageSize > 0)
                {
                    return (int)Math.Ceiling((decimal)TotalRecords / PageSize);
                }
                return 1;
            }
        }

        /// <summary>
        /// Current offset
        /// </summary>
        public int Offset
        {
            get
            {
                if (PageSize > 0)
                {
                    return PageIndex * PageSize;
                }
                return 0;
            }
        }

        /// <summary>
        /// Number of records on current page
        /// </summary>
        public int RecordsOnPage
        {
            get
            {
                if (PageSize > 0)
                {
                    return PageIndex * PageSize + PageSize > TotalRecords ? TotalRecords : PageIndex * PageSize + PageSize;
                }
                return TotalRecords;
            }
        }

        /// <summary>
        /// Determine if paging is enabled
        /// </summary>
        public bool IsPaged
        {
            get
            {
                return PageSize > 0 && TotalRecords > PageSize;
            }
        }

        /// <summary>
        /// Current direction of sort (1 = Ascending, 2 = Descending)
        /// </summary>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// Current sorted by column name
        /// </summary>
        public string SortColumn { get; set; }

        #endregion

        public DataRequest()
        {
            PageSize = AppSettings.DefaultPageSize;
            SortDirection = AppSettings.DefaultSortDirection;
        }
    }

    public class DataRequest<TFilter> : DataRequest where TFilter : class
    {
        public TFilter Filter { get; set; }
    }
}
