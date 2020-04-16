using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public class LanguageService : ServiceBase
    {
        public static List<Language> FetchList()
        {
            return DB
                .Languages
                 .AsNoTracking()
                .ToList();
        }
    }
}