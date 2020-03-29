using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class GeneralService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static ServiceResponse SaveInstruction(List<GeneralInstruction> instruction, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();

            DB.GeneralInstructions.AddRange(instruction);
            DB.SaveChanges();

            return sr;
        }

        public static ServiceResponse DeleteRangeInstructions (List<GeneralInstruction> instruction)
        {
            ServiceResponse sr = new ServiceResponse();
            DB.GeneralInstructions.RemoveRange(instruction);
            DB.SaveChanges();
            return sr;
        }

        public static GeneralInstruction Fetch(int? InstructionId)
        {
            return DB.GeneralInstructions
                     .Where(m => m.InstructionId == (InstructionId.HasValue ? InstructionId.Value : 0))
                     .FirstOrDefault();
        }
        public static GeneralInstruction FetchByTestIdAndSubjectId(int? TestId, int? SubjectId)
        {
            return DB.GeneralInstructions
                     .Where(m => m.TestId == (TestId.HasValue ? TestId.Value : 0) && m.SubjectId == (SubjectId.HasValue ? SubjectId.Value : 0))
                     .FirstOrDefault();
        }

        public static List<GeneralInstruction> GeneralInstructionList(string Lang, bool IsActive)
        {
            return DB.GeneralInstructions
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }

        public static ServiceResponse DeleteGeneralInstruction(int instructionId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Instruction = Fetch(instructionId);

                DB.Entry(Instruction).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
    }
}