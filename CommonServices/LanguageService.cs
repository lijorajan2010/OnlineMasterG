using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class LanguageService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static List<Language> FetchList()
        {
            return DB
                .Languages
                 .AsNoTracking()
                .ToList();
        }
    }
}