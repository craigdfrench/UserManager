// <copyright file="LogAuthEvents.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceInterface
{
    using System;
    using System.Collections.Generic;
    using Raven.Client.Documents;
    using ServiceStack;
    using ServiceStack.Auth;
    using ServiceStack.Web;
    using ServiceModel.Types;

    /// <summary>
    /// Log all authorization eents
    /// </summary>
    public class LogAuthEvents : AuthEvents
    {
        public override void OnAuthenticated(
            IRequest httpReq,
            IAuthSession session,
            IServiceBase authService,
            IAuthTokens tokens,
            Dictionary<string, string> authInfo)
        {
            var documentStore = authService.ResolveService<IDocumentStore>();
            using (var dbSession = documentStore.OpenSession())
            {
                var log = new ActivityLog
                {
                    Activity = ActivityLog.LoggedActivities.Login,
                    UserId = session.UserAuthId,
                    Time = DateTime.Now,
                    Notes = $"Referrer:{httpReq.UrlReferrer.ToString()}",
                    RemoteIP = httpReq.RemoteIp,
                    UserAgent = httpReq.UserAgent
                };
                dbSession.Store(log);
                dbSession.SaveChanges();
            }

            base.OnAuthenticated(httpReq, session, authService, tokens, authInfo);
        }

        public override void OnLogout(IRequest httpReq, IAuthSession session, IServiceBase authService)
        {
            var documentStore = authService.ResolveService<IDocumentStore>();
            using (var dbSession = documentStore.OpenSession())
            {
                var log = new ActivityLog
                {
                    Activity = ActivityLog.LoggedActivities.Logout,
                    UserId = session.UserAuthId,
                    Time = DateTime.Now,
                    RemoteIP = httpReq.RemoteIp,
                    UserAgent = httpReq.UserAgent,
                    Notes = $"Referrer:{httpReq.UrlReferrer.ToString()}"
                };
                dbSession.Store(log);
                dbSession.SaveChanges();
            }

            base.OnLogout(httpReq, session, authService);
        }
    }
}
