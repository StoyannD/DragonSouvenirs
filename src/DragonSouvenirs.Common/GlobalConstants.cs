﻿namespace DragonSouvenirs.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "DragonSouvenirs";

        public const string AdministratorRoleName = "Administrator";

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

        public class Category
        {
            public const int NameMaxLength = 40;
            public const int NameMinLength = 2;
            public const int TitleMaxLength = 40;
            public const int TitleMinLength = 2;
            public const int ContentMaxLength = 300;
            public const int ContentMinLength = 4;

            public const string NameLengthError = "Name must be between {2} and {1} symbols.";
            public const string TitleLengthError = "Title must be between {2} and {1} symbols.";
            public const string ContentLengthError = "Content must be between {2} and {1} symbols.";

            public const string CategorySuccessfullyDeleted = "Category {0} Deleted/Recovered successfully.";
            public const string CategorySuccessfullyEdited = "Category {0} Edited successfully.";
            public const string CategorySuccessfullyCreated = "Category {0} Created successfully.";
            public const string OnCreateCategoryNotUniqueError = "Category with name {0} exists.";
        }

        public class Product
        {
            public const int NameMaxLength = 40;
            public const int NameMinLength = 2;
            public const int TitleMaxLength = 40;
            public const int TitleMinLength = 2;
            public const int DescriptionMaxLength = 300;
            public const int DescriptionMinLength = 4;
            public const decimal PriceMax = decimal.MaxValue;
            public const decimal PriceMin = 0m;
            public const int QuantityMax = int.MaxValue;
            public const int QuantityMin = 0;
            public const int SizeMax = int.MaxValue;
            public const int SizeMin = 0;

            public const string NameLengthError = "Name must be between {2} and {1} symbols.";
            public const string TitleLengthError = "Title must be between {2} and {1} symbols.";
            public const string DescriptionLengthError = "Description must be between {2} and {1} symbols.";
            public const string PriceInRangeError = "Price should be in range {2} - {1}";
            public const string QuantityInRangeError = "Quantity should be in range {2} - {1}";
            public const string SizeInRangeError = "Size should be in range {2} - {1}";

            public const string ProductSuccessfullyDeleted = "Product {0} Deleted/Recovered successfully.";
            public const string ProductSuccessfullyEdited = "Product {0} Edited successfully.";
            public const string ProductNotFound = "Product with id {0} not found.";
        }

        public class Image
        {
            public const int ImagesPerProduct = 4;
        }
    }
}
