using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace ChromeLogCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeLog chrlog = new ChromeLog();
            chrlog.getChromeLog();
        }
    }
}
