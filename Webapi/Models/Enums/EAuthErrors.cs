namespace Webapi.Models.Enums
{
    public class EAuthErrors
    {
        public const string RegisterMissingFields = "RegisterMissingFields";
        public const string RegisterPasswordMissmatch = "RegisterPasswordMissmatch";
        public const string RegisterUnexpectedError = "RegisterUnexpectedError";
        public const string LoginIncorrect = "LoginIncorrect";
    }
}