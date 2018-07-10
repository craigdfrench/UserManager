// <copyright file="UserDataCredentialsAuthProvider.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceInterface
{
    using Raven.Client.Documents;
    using ServiceStack;
    using ServiceStack.Auth;
    using ServiceStack.Web;
    using System.Collections.Generic;
    using System.Linq;
    using UserManager.ServiceModel.Types;

    /// <summary>
    /// Uses email and passwords from User Manager data to provide authorization
    /// </summary>
    public class UserDataCredentialsAuthProvider : CredentialsAuthProvider
    {
        /// <inheritdoc />
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            var documentStore = authService.ResolveService<IDocumentStore>();
            bool authenticationResponse = false;
            using (var session = documentStore.OpenSession())
            {
                var userRecord = session.Query<User>().SingleOrDefault(x => x.EmailAddress.Equals(userName));
                if (userRecord != null)
                {
                    var hasher = new PasswordHasher();
                    bool needsRehash;
                    authenticationResponse = hasher.VerifyPassword(userRecord.Password, password, out needsRehash);
                }
            }

            return authenticationResponse;
        }

        /// <inheritdoc/>
        public override IHttpResult OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            var documentStore = authService.ResolveService<IDocumentStore>();

            User userRecord;
            using (var dbSession = documentStore.OpenSession())
            {
                userRecord = dbSession.Query<User>()
                    .SingleOrDefault(x => x.EmailAddress.Equals(session.UserAuthName));
            }

            if (userRecord != null)
            {
                session.UserAuthId = userRecord.Id;
                session.DisplayName = $"{userRecord.FirstName} {userRecord.LastName}";
                session.FirstName = userRecord.FirstName;
                session.LastName = userRecord.LastName;
                session.Email = userRecord.EmailAddress;
                session.UserName = userRecord.EmailAddress;
            }

            authService.SaveSession(session);

            return base.OnAuthenticated(authService, session, tokens, authInfo);
        }
    }
}