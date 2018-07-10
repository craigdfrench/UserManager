// <copyright file="PopulateRest.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceModel
{
    using System.Diagnostics.CodeAnalysis;
    using ServiceStack;

    /// <summary>
    /// Populate REST service class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1649:FileMustMatchClassName", Justification = "Reviewed.")]
    [Route("/populate", "POST")]
    public class PopulateDataRequest : IReturn<PopulateResponse>
    {
        /// <summary>
        /// Gets or sets name of contacted user
        /// </summary>
        public string DomainName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool Populate { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    [Route("/populate", "GET")]
    public class PopulateCheckRequest : IReturnVoid
    {
    }

    /// <summary>
    /// Populate Response service Class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class PopulateResponse
    {
        /// <summary>
        /// Gets or sets result of Populate Query
        /// </summary>
        public string Result { get; set; }
    }
}