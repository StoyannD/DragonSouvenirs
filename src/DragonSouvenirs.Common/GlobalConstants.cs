namespace DragonSouvenirs.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "DragonSouvenirs";

        public const string AdministratorRoleName = "Administrator";

        // public const string CategoryAlreadyDeleted = "Category {0} is already Deleted/Recovered.";
        public const string CategorySuccessfullyDeleted = "Category {0} Deleted/Recovered successfully.";

        public class User
        {
            public const string UserNameDisplay = "Username";
            public const string FullNameDisplay = "Fullname";
            public const string DefaultShippingAddressDisplay = "Shipping Address";
            public const int UserNameMaxLength = 30;
            public const int UserNameMinLength = 4;
            public const int FullNameMaxLength = 30;
            public const int FullNameMinLength = 10;
            public const int DefaultShippingAddressMaxLength = 60;
            public const int DefaultShippingAddressMinLength = 10;

            public const string UserNameLengthError = "Username must be between {2} and {1} symbols.";
            public const string FullNameLengthError = "FullName must be between {2} and {1} symbols.";
            public const string DefaultShippingAddressLengthError = "Shipping Address must be between {2} and {1} symbols.";

            public const string EmailAddressError = "Invalid Email address.";
            public const string UserSuccessfullyBannedMessage = "User {0} Banned successfully.";
            public const string UserSuccessfullyUnBannedMessage = "User {0} Unbanned successfully.";
            public const string UserAlreadyBannedMessage = "User {0} is already banned.";
            public const string UserNotBannedMessage = "User {0} is not banned.";
            public const string UserSuccessfullyEdited = "User {0} Edited successfully.";
        }
    }
}
