// <copyright file="User.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceModel.Types
{
    /// <summary>
    /// POCO for User Database
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier for this record
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets first Name of User
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last Name of User
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email Address of User
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets hashed password of User
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user status is active
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lock status is locked
        /// </summary>
        public bool? Locked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
