namespace Monkey.Core.Constants
{
    public static class Regex
    {
        public const string AlphabetsOnly = @"^[a-zA-Z]+$";
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";
        public const string AlphaNumericSpaces = @"^[a-zA-Z0-9\s]*$";
        public const string Date = @"(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)";
        public const string EmailFormat = @"^[0-9a-zA-Z]+([0-9a-zA-Z]*[-._+])*[0-9a-zA-Z]+@[0-9a-zA-Z]+([-.][0-9a-zA-Z]+)*([0-9a-zA-Z]*[.])[a-zA-Z]{2,6}$";
        public const string NumericOnly = @"^\d+(\.\d{1,4})?$";
        public const string PasswordFormat = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d !@#$%^&*()_+]{8,}$";
    }
}