// <copyright file="ActivityRest.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceModel
{
    using ServiceStack;
    using Types;

    [Route("/activity", "GET")]
    public class GetActivityRecords : IReturn<ActivityLog[]>
    {
        public int? Count { get; set; }

        private string SubjectId { get; set; }

        private string UserId { get; set; }
    }
}
