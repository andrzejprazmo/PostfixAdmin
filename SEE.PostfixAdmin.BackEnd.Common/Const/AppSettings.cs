using SEE.PostfixAdmin.BackEnd.Common.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common.Const
{
    public static class AppSettings
    {
        public static string IdentityInstanceCookieName { get; private set; } = "PostfixAdminIdentityInstanceCookie";

        public static int DefaultPageSize { get; private set; } = 10;

        public static SortDirection DefaultSortDirection { get; set; } = SortDirection.Ascending;
    }
}
