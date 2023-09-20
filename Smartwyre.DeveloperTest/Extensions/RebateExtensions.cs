using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Extensions
{
    public static class RebateExtensions
    {
        public static bool IsValid(this Rebate rebate) 
            => rebate != null;
    }
}
