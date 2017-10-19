using Monkey.Core.Constants;
using System.Collections.Generic;

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