// <copyright file="SampleData.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceInterface
{
    using ServiceStack.Auth;
    using ServiceModel.Types;

    /// <summary>
    /// Provide sample data to the program
    /// </summary>
    public class SampleData
    {
        private static readonly string[] Names =
        {
            "Sreekanth/Karukonda",
            "Gisela/Diano",
            "Srinath/Jayaraman",
            "Brianne/Buffett",
            "Bhavin/Poladia",
            "Yanglong/Cui",
        };

        /// <summary>
        /// Returns sample data as User array
        /// Default Password is first name reversed
        /// </summary>
        /// <param name="domainName">Domain name to be used for email addresses</param>
        /// <returns>Sample data formatted as User Elements</returns>
        public static User[] GetSampleData(string domainName)
        {
            var myUsers = new User[Names.Length];
            var count = 0;
            var hasher = new PasswordHasher();
            foreach (var name in Names)
            {
                var nameParts = name.Split('/');
                var firstNamePart = nameParts[0];
                var lastNamePart = nameParts[1];

                // Hash password using same method ASP.NET Identity v3
                // Default password is first name
                var password = hasher.HashPassword(firstNamePart);

                myUsers[count] = new User
                {
                    FirstName = firstNamePart,
                    LastName = lastNamePart,
                    EmailAddress = $"{firstNamePart}.{lastNamePart}@{domainName}",
                    Active = true,
                    Locked = false,
                    Password = password,
                };
                count++;
            }

            return myUsers;
        }
    }
}
