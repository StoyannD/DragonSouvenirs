namespace DragonSouvenirs.Common
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
            public const int CityMinLength = 3;
            public const int CityMaxLength = 25;
            public const int NeighborhoodMinLength = 3;
            public const int NeighborhoodMaxLength = 25;
            public const int StreetMinLength = 5;
            public const int StreetMaxLength = 100;
            public const int StreetNumberMax = 1000;
            public const int StreetNumberMin = 1;
            public const int ApartmentBuildingMinLength = 1;
            public const int ApartmentBuildingMaxLength = 4;
            public const int EntranceMinLength = 1;
            public const int EntranceMaxLength = 4;
            public const int FloorMax = 1000;
            public const int FloorMin = 1;
            public const int ApartmentNumberMax = 1000;
            public const int ApartmentNumberMin = 1;

            public const string UserNameLengthError = "Username must be between {2} and {1} symbols.";
            public const string FullNameLengthError = "FullName must be between {2} and {1} symbols.";
            public const string DefaultShippingAddressLengthError = "Shipping Address must be between {2} and {1} symbols.";
            public const string CityError = "City length must be between {2} and {1} symbols.";
            public const string NeighborhoodError = "Neighborhood length must be between {2} and {1} symbols.";
            public const string StreetError = "Street length must be between {2} and {1} symbols.";
            public const string StreetNumberError = "Street number should be in range [{2}-{1}].";
            public const string ApartmentBuildingError = "Apartment building length must be between {2} and {1} symbols.";
            public const string EntranceError = "Entrance length must be between {2} and {1} symbols.";
            public const string FloorError = "Floor should be in range [{2}-{1}].";
            public const string ApartmentNumberError = "Apartment number should be in range [{2}-{1}].";

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
            public const int PerPageDefault = 12;
            public const int LatestProductsComponentsItemsPerSlide = 3;

            public const string NameLengthError = "Name must be between {2} and {1} symbols.";
            public const string TitleLengthError = "Title must be between {2} and {1} symbols.";
            public const string DescriptionLengthError = "Description must be between {2} and {1} symbols.";
            public const string PriceInRangeError = "Price should be in range {2} - {1}";
            public const string QuantityInRangeError = "Quantity should be in range {2} - {1}";
            public const string SizeInRangeError = "Size should be in range {2} - {1}";

            public const string ProductSuccessfullyDeleted = "Product {0} Deleted/Recovered successfully.";
            public const string ProductSuccessfullyEdited = "Product {0} Edited successfully.";
            public const string ProductNotFound = "Product with id {0} not found.";

            public const string ProductSuccessfullyCreated = "Product {0} Created successfully.";
            public const string OnCreateProductNotUniqueError = "Product with name {0} exists.";
        }

        public class Image
        {
            public const int ImagesPerProduct = 4;
        }

        public class Sessions
        {
            public const string CartSessionKey = "cart";
        }

        public class Order
        {
            public const int UserFullNameMaxLength = 60;
            public const int UserFullNameMinLength = 2;

            public const int CityMinLength = 3;
            public const int CityMaxLength = 25;
            public const int NeighborhoodMinLength = 3;
            public const int NeighborhoodMaxLength = 25;
            public const int StreetMinLength = 5;
            public const int StreetMaxLength = 100;
            public const int StreetNumberMax = 1000;
            public const int StreetNumberMin = 1;
            public const int ApartmentBuildingMinLength = 1;
            public const int ApartmentBuildingMaxLength = 4;
            public const int EntranceMinLength = 1;
            public const int EntranceMaxLength = 4;
            public const int FloorMax = 1000;
            public const int FloorMin = 1;
            public const int ApartmentNumberMax = 1000;
            public const int ApartmentNumberMin = 1;

            public const int NotesMaxLength = 100;
            public const int NotesMinLength = 5;

            public const int OfficeMaxLength = 100;
            public const int OfficeMinLength = 5;

            public const decimal DeliveryPrice = 10M;

            public const string OrderCreated = "Order Created Successfully!";

            public const string EmptyCart = "Your cart is empty! Please add a product to make an order.";

            public const string UserFullNameError = "Fullname must be between {2} and {1} symbols.";
            public const string UserEmailError = "Please enter a valid Email.";
            public const string InvoiceNumberError = "Please enter a valid phone number.";
            public const string ShippingAddressError = "Address length must be between {2} and {1} symbols.";
            public const string CityError = "City length must be between {2} and {1} symbols.";
            public const string NeighborhoodError = "Neighborhood length must be between {2} and {1} symbols.";
            public const string StreetError = "Street length must be between {2} and {1} symbols.";
            public const string StreetNumberError = "Street number should be in range [{2}-{1}].";
            public const string ApartmentBuildingError = "Apartment building length must be between {2} and {1} symbols.";
            public const string EntranceError = "Entrance length must be between {2} and {1} symbols.";
            public const string FloorError = "Floor should be in range [{2}-{1}].";
            public const string ApartmentNumberError = "Apartment number should be in range [{2}-{1}].";
            public const string NotesError = "Notes length must be between {2} and {1} symbols.";
            public const string OfficeError = "Office name length must be between {2} and {1} symbols.";
        }

        public class Offices
        {
            public const string RequestUrl =
                "https://demo.econt.com/ee/services/Nomenclatures/NomenclaturesService.getOffices.json";

            public const string Country = "България";

            public const string Econt = "Econt";

            public const string Speedy = "Speedy";
        }
    }
}
