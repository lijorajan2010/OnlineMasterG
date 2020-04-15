using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class LanguageService
    {

        public static OnlinemasterjiEntities DB { get; private set; }
        static LanguageService()
        {
            DB = new OnlinemasterjiEntities();
        }
        public static List<Language> FetchList()
        {
            return DB
                .Languages
                 .AsNoTracking()
                .ToList();
        }
    }
}