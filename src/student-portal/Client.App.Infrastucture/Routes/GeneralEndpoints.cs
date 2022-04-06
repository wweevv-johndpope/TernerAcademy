using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Routes
{
    public static class GeneralEndpoints
    {
        public const string GetCourseData = "api/general/coursedata";
        public const string GetTx = "api/general/transaction/{0}";
        public const string GetTFuelPrice = "api/general/price/tfuel";
    }
}
