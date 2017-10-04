using Microsoft.AspNetCore.Mvc;
using Puppy.Core.DictionaryUtils;
using Puppy.DataTable;
using Puppy.DataTable.Attributes;
using Puppy.DataTable.Constants;
using Puppy.DataTable.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Monkey.Areas.Portal.Controllers
{
    [Route("portal")]
    public class HomeController : MvcController
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("users")]
        public DataTableActionResult<UserFacetRowViewModel> GetFacetedUsers([FromBody]DataTableParamModel dataTableParamModel)
        {
            var additionalData = dataTableParamModel.Data.GetValue<string>("newData");
            var query = FakeDatabase.Users.Select(
                user =>
                    new UserFacetRowViewModel
                    {
                        Name = user.Name,
                        Number = user.Number,
                        Email = user.Email,
                        Position = user.Position,
                        Hired = user.Hired,
                        IsAdmin = user.IsAdmin,
                        Content = "https://randomuser.me/api/portraits/thumb/men/" + user.Id + ".jpg"
                    });

            var response = query.GetDataTableResponse(dataTableParamModel);

            var result = response.GetDataTableActionResult<UserFacetRowViewModel>(
                row =>
                    new
                    {
                        Content = "<div>" +
                                  "  <div>Email: " + row.Email + (row.IsAdmin ? " (admin)" : "") +
                                  "</div>" +
                                  "  <div>Hired: " + row.Hired + "</div>" +
                                  "  <img src='" + row.Content + "' />" +
                                  "</div>",
                        Hired = row.Hired
                    });

            return result;
        }
    }

    public class UserFacetRowViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        [DataTable(DisplayName = "Is Admin")]
        public bool IsAdmin { get; set; }

        public FakeDatabase.PositionTypes? Position { get; set; }

        public FakeDatabase.Numbers Number { get; set; }

        [DataTable(DisplayName = "Hired Time")]
        public DateTime? Hired { get; set; }

        [DataTableFilter(FilterType.None)]
        public string Content { get; set; }
    }

    public static class FakeDatabase
    {
        private static readonly List<User> _users;

        static FakeDatabase()
        {
            var r = new Random();
            var domains = "gmail.com,yahoo.com,hotmail.com".Split(',').ToArray();
            var positions = new List<PositionTypes?>
            {
                null,
                PositionTypes.Engineer,
                PositionTypes.Tester,
                PositionTypes.Manager
            };
            _users = new List<User>(Enumerable.Range(1, 100).Select(i => new User()
            {
                Id = i,
                Email =
                    "user" + i + "@" + domains[i % domains.Length],
                Name = r.Next(6) == 3 ? null : "User" + i,
                Position = positions[i % positions.Count],
                IsAdmin = i % 11 == 0,
                Number = (Numbers)r.Next(4),
                Hired =
                    i % 8 == 0
                        ? null as DateTime?
                        : DateTime.UtcNow.AddDays(-1 * 365 * 3 * r.NextDouble()).AddHours(9).AddHours(r.Next(9)),
                Salary =
                    10000 + (DateTime.UtcNow.Minute * 1000) +
                    (DateTime.UtcNow.Second * 100) +
                    DateTime.UtcNow.Millisecond
            }));
        }

        public static IQueryable<User> Users => _users.AsQueryable();

        public enum Numbers
        {
            Zero,
            One,
            Two,
            Three,
            Four
        }

        public enum PositionTypes
        {
            [Display(Name = "Software Engineer")]
            Engineer,

            Tester,
            Manager
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public PositionTypes? Position { get; set; }

            public DateTime? Hired { get; set; }

            public Numbers Number { get; set; }

            public bool IsAdmin { get; set; }

            public decimal Salary { get; set; }
        }
    }
}