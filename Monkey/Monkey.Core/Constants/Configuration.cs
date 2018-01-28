using EnumsNET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Monkey.Core.Constants
{
    public static class Configuration
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class ConfigurationKeyAttribute : Attribute
        {
            public string Value { get; set; }

            public KeyType Type { get; set; } = KeyType.String;

            public KeyGroup Group { get; set; } = KeyGroup.None;

            public string Description { get; set; }
        }

        public enum KeyType
        {
            String,
            Integer,
            DateTime,
            Double,
            TimeSpan,
            Decimal,
            Boolean
        }

        public enum KeyGroup
        {
            [Display(Name = "Email", Description = "Email sender configuration")]
            Email,

            [Display(Name = "None", Description = "Un-categorize")]
            None
        }

        public static List<Key> GetKeys(KeyGroup group)
        {
            List<Key> listKey = new List<Key>();

            foreach (EnumMember<Key> member in EnumsNET.Enums.GetMembers<Key>())
            {
                DisplayAttribute displayAttribute = member.Value.GetAttributes().Get<DisplayAttribute>();
                if (displayAttribute.GetGroupName() != group.ToString())
                {
                    continue;
                }

                listKey.Add(member.Value);
            }

            listKey = listKey.Distinct().ToList();

            return listKey;
        }

        public enum Key
        {
            #region Email

            [ConfigurationKey(Value = "ITS", Type = KeyType.String, Group = KeyGroup.Email, Description = "")]
            DisplayName,

            [ConfigurationKey(Value = "mailing.idwf@gmail.com", Type = KeyType.String, Group = KeyGroup.Email, Description = "")]
            EmailAddress,

            [ConfigurationKey(Value = "mailing.idwf@gmail.com", Type = KeyType.String, Group = KeyGroup.Email, Description = "")]
            UserName,

            [ConfigurationKey(Value = "1234@Abcd", Type = KeyType.String, Group = KeyGroup.Email, Description = "")]
            Password,

            [ConfigurationKey(Value = "smtp.gmail.com", Type = KeyType.String, Group = KeyGroup.Email, Description = "")]
            ServerHost,

            [ConfigurationKey(Value = "587", Type = KeyType.Integer, Group = KeyGroup.Email, Description = "")]
            ServerPort,

            [ConfigurationKey(Value = "4", Type = KeyType.Integer, Group = KeyGroup.Email, Description = "")]
            ServerSecureOption,

            #endregion Email
        }
    }
}