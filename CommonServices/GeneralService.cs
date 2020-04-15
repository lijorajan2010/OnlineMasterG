﻿using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class GeneralService
    {
        public static OnlinemasterjiEntities DB { get; private set; }
        static GeneralService()
        {
            DB = new OnlinemasterjiEntities();
        }
        public static ServiceResponse SaveInstruction(List<GeneralInstruction> instruction, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();

            DB.GeneralInstructions.AddRange(instruction);
            DB.SaveChanges();

            return sr;
        }
        public static ServiceResponse SaveGreetings(Greeting greeting, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();

            DB.Greetings.Add(greeting);
            DB.SaveChanges();

            return sr;
        }

        
       
        
        internal static List<TestimonialsVM> TestimonialsVMList()
        {
            List<TestimonialsVM> list = new List<TestimonialsVM>();

            var MockTestAttemptsRatings = ExamService.GetAttemptListReviews();

            if (MockTestAttemptsRatings != null && MockTestAttemptsRatings.Count() > 0)
            {
                foreach (var item in MockTestAttemptsRatings)
                {
                    var UserDetails = UserService.Fetch(item.Login);
                    list.Add(new TestimonialsVM()
                    {
                        FirstName = UserDetails?.FirstName,
                        LastName = UserDetails?.LastName,
                        ImageDataFileId = UserDetails?.LogoDataFileId,
                        Rating = item.Rating,
                        Review = item.Review,
                        IsApproved = item.IsReviewApproved,
                        AttemptId = item.AttemptId,
                        TestName = TestService.Fetch(item.TestId)?.TestName
                    });
                }
            }

            return list;
        }
        internal static List<TestimonialsVM> TestimonialsVMApprovedList()
        {
            List<TestimonialsVM> list = new List<TestimonialsVM>();

            var MockTestAttemptsRatings = ExamService.GetAttemptListApprovedReviews();

            if (MockTestAttemptsRatings != null && MockTestAttemptsRatings.Count() > 0)
            {
                foreach (var item in MockTestAttemptsRatings)
                {
                    var UserDetails = UserService.Fetch(item.Login);
                    list.Add(new TestimonialsVM()
                    {
                        FirstName = UserDetails?.FirstName,
                        LastName = UserDetails?.LastName,
                        ImageDataFileId = UserDetails?.LogoDataFileId,
                        Rating = item.Rating,
                        Review = item.Review
                    });
                }
            }

            return list;
        }
        internal static List<LatestUpdate> LatestUpdatesList(string Lang, bool IsActive)
        {
            return DB.LatestUpdates
                 .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                 .ToList();
        }
        internal static List<Greeting> GreetingsList(string Lang, bool IsActive)
        {
            return DB.Greetings
                 .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                 .ToList();
        }
        
        internal static List<LatestUpdate> LatestAllUpdatesList(string Lang)
        {
            return DB.LatestUpdates
                 .Where(m => m.LanguageCode == Lang)
                 .ToList();
        }


        public static ServiceResponse DeleteRangeInstructions(List<GeneralInstruction> instruction)
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

        internal static ServiceResponse DeleteLatestUpdate(int? updateId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Update = FetchLatestUpdate(updateId);

                DB.Entry(Update).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        internal static ServiceResponse DeleteGreetings(int? greetingId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Greeting = FetchGreeting(greetingId);

                DB.Entry(Greeting).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }


        public static LatestUpdate FetchLatestUpdate(int? UpdateId)
        {
            return DB.LatestUpdates
                     .Where(m => m.UpdateId == (UpdateId.HasValue ? UpdateId.Value : 0))
                     .FirstOrDefault();
        }
        public static Greeting FetchGreeting(int? GreetingId)
        {
            return DB.Greetings
                     .Where(m => m.GreetingsId == (GreetingId.HasValue ? GreetingId.Value : 0))
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


        public static ServiceResponse SaveLatestUpdates(LatestUpdate update, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (update.UpdateId == 0)
            {
                DB.LatestUpdates.Add(update);
                DB.SaveChanges();
            }
            else
            {
                var dbUpdate = FetchLatestUpdate(update.UpdateId);
                if (dbUpdate == null)
                {
                    sr.AddError($"UpdateId for {update.UpdateDescription} was not found.");
                    return sr;
                }
                else
                {
                    dbUpdate.UpdateDescription = update.UpdateDescription;
                    dbUpdate.LanguageCode = update.LanguageCode;
                    dbUpdate.Sequence = update.Sequence;
                    dbUpdate.Isactive = update.Isactive;
                    dbUpdate.EditBy = auditlogin;
                    dbUpdate.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbUpdate.UpdateId;
                    sr.ReturnName = dbUpdate.UpdateDescription;

                    return sr;
                }
            }
            return sr;
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
