#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain </Project>
//     <File>
//         <Name> Constants </Name>
//         <Created> 08/04/2017 11:31:32 PM </Created>
//         <Key> dc620a3e-88e2-4916-b998-b44e6c48db03 </Key>
//     </File>
//     <Summary>
//         Constants
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

namespace Monkey
{
    public static class Constants
    {
        /// <summary>
        ///     Configuration Endpoint for Area and Controller 
        /// </summary>
        public static class Endpoint
        {
            public static class RootArea
            {
                public const string Root = "";
                public const string Home = "";
            }

            public static class DevelopersArea
            {
                public const string Root = "developers";
                public const string Developers = "developers";
            }

            public static class PortalArea
            {
                public const string Root = "portal";
                public const string Home = "portal";
            }
        }

        public static class ViewDataKey
        {
            public const string Title = nameof(Title);
        }
    }
}