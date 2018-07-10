// <copyright file="ActivityLog.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceModel.Types
{
    using System;

    public class ActivityLog
    {
        public enum LoggedActivities
        {
            Login,
            Logout,
            Read,
            Update,
            Delete,
            Create,
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public string SubjectUserId { get; set; }

        public DateTime Time { get; set; }

        public LoggedActivities Activity { get; set; }

        public string Notes { get; set; }

        public string UserAgent { get; set; }

        public string RemoteIP { get; set; }
    }
}
