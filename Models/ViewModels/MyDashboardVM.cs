using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class MyDashboardVM
    {
        public List<GivenMockTests> GivenMockTests { get; set; }
        public List<GivenQuizes> GivenQuizes { get; set; }
        public List<Subscriptions> Subscriptions { get; set; }
        public Profile profile { get; set; }

    }
    public class GivenMockTests
    {
        public int AttemptId { get; set; }
        public string TestName { get; set; }
        public string TimeUsed { get; set; }
    }
    public class GivenQuizes
    {
        public int AttemptId { get; set; }
        public string QuizName { get; set; }
        public string TimeUsed { get; set; }
    }
    public class Subscriptions
    {

    }
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int? ImgDataFileId { get; set; }
    }
}