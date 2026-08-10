using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enumarables
{
    public enum Enum_PageError
    {
        PAGE_NOT_FOUND = 404,
        ACCESS_DENIED = 403,
        SERVER_ERROR = 500,
        DB_VALIDATION = 501,
        CAN_NOT_DELETE = 502
    }
}
