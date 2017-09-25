using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Puppy.DataTable;
using Puppy.DataTable.Attributes;
using Puppy.DataTable.Constants;
using Puppy.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using Puppy.DataTable.Models;

namespace Monkey.Controllers.Api
{
    [Route(Endpoint)]
    public class TestApiController : ApiController
    {
        public const string Endpoint = AreaName + "/test";

        /// <summary>
        ///     Device Info 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("deviceinfo")]
        public IActionResult DeviceInfo()
        {
            var deviceInfo = HttpContext.Request.GetDeviceInfo();
            return Ok(deviceInfo);
        }

        /// <summary>
        ///     Logged In User 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("userinfo")]
        [Auth]
        public IActionResult LoggedInUser()
        {
            return Ok(new
            {
                HttpContext.User.Identity.IsAuthenticated,
                UserInfo = Core.LoggedInUser.Current
            });
        }

        [HttpPost]
        [Route("users")]
        public DataTablesResult<UserFacetRowViewModel> GetFacetedUsers([FromBody]DataTableParamModel dataTableParamModel)
        {
            var result =
                DataTablesResult.Create(
                    FakeDatabase.Users.Select(user =>
                        new UserFacetRowViewModel
                        {
                            Email = user.Email,
                            Position = user.Position == null ? "" : user.Position.ToString(),
                            Hired = user.Hired,
                            IsAdmin = user.IsAdmin,
                            Content = "https://randomuser.me/api/portraits/thumb/men/" + user.Id + ".jpg"
                        }), dataTableParamModel,
                    rowViewModel =>
                        new
                        {
                            Content = "<div>" +
                                      "  <div>Email: " + rowViewModel.Email + (rowViewModel.IsAdmin ? " (admin)" : "") +
                                      "</div>" +
                                      "  <div>Hired: " + rowViewModel.Hired + "</div>" +
                                      "  <img src='" + rowViewModel.Content + "' />" +
                                      "</div>"
                        });

            return result;
        }
    }

    public class UserFacetRowViewModel
    {
        [DataTables(DisplayName = "E-Mail")]
        [DataTablesFilter(DataTablesFilterType.Text)]
        public string Email { get; set; }

        [DataTables(DisplayName = "Is Admin")]
        [DataTablesFilter(DataTablesFilterType.Select)]
        public bool IsAdmin { get; set; }

        [DataTables(DisplayName = "Position")]
        [DataTablesFilter(DataTablesFilterType.Text)]
        public string Position { get; set; }

        [DataTables(DisplayName = "Hired Time")]
        [DataTablesFilter(DataTablesFilterType.DateRange)]
        public DateTime? Hired { get; set; }

        [DataTables(DisplayName = "Content")]
        [DataTablesFilter(DataTablesFilterType.None)]
        public string Content { get; set; }
    }

    internal static class FakeDatabase
    {
        private static List<User> _users;

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

        public static IQueryable<User> Users
        {
            get { return _users.AsQueryable(); }
        }

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