using System.Collections.Generic;
using Monkey.Core.Constants;

namespace Monkey.Core.Models.Auth
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Enums.Permission> Permissions { get; set; }
    }
}