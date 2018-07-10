// <copyright file="UserRest.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>


namespace UserManager.ServiceModel
{
    using System.Diagnostics.CodeAnalysis;
    using ServiceStack;
    using Types;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1649:FileMustMatchClassName", Justification = "Reviewed.")]
    [Route("/users", "POST")]
    public class CreateUser : IReturn<User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/update", "POST")]
    public class UpdateUser : IReturn<User>
    {
        /// <summary>
        /// Gets or sets return reference to the document
        /// </summary>
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public bool? Locked { get; set; }

        public bool? Active { get; set; }

        public bool Delete { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/{Id}", "GET")]
    public class GetUser : IReturn<User>
    {
        /// <summary>
        /// Get or Set id of user
        /// </summary>
        public string Id { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/withstatus", "GET")]
    public class GetUsersWhoAre : IReturn<User[]>
    {
        public string LockStatus { get; set; }

        public string ActiveStatus { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users", "GET")]
    public class GetUsers : IReturn<User[]>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/setlock/{Id}", "POST")]
    public class UserSetLockout : IReturn<User>
    {
        public string Id { get; set; }

        public bool LockedStatus { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/setactive/{Id}", "POST")]
    public class UserSetActive : IReturn<User>
    {
        public string Id { get; set; }

        public bool ActiveStatus { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/setpassword/{id}", "POST")]
    public class SetPassword : IReturn<User>
    {
        public string Id { get; set; }

        public string ActiveStatus { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/users/delete/{Id}", "POST")]
    public class UserDelete : IReturn<User>
    {
        public string Id { get; set; }
    }
}
