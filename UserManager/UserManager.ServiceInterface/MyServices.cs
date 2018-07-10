// <copyright file="MyServices.cs" company="TELoIP">
// Copyright (c) TELoIP. All rights reserved.
// </copyright>

namespace UserManager.ServiceInterface
{
    using System;
    using System.Linq;
    using System.Text;
    using Raven.Client.Documents;
    using Raven.Client.Documents.Queries;
    using ServiceModel;
    using ServiceModel.Types;
    using ServiceStack;
    using ServiceStack.Auth;

    /// <summary>
    /// REST Handler for Any HTTP method
    /// </summary>
    /// <inheritdoc cref="Service" />
    public class MyServices : Service
    {
        /// <summary>
        /// Populate database with sample data ONLY if it is already empty
        /// </summary>
        /// <param name="populateData">populateData has initial user and info for sample data if requested</param>
        /// <returns>HTTP response object</returns>
        public object Post(PopulateDataRequest populateData)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            var passwordHasher = new PasswordHasher();
            var newPasswordHash = passwordHasher.HashPassword(populateData.FirstName);

            var newUser = new User
            {
                FirstName = populateData.FirstName,
                LastName = populateData.LastName,
                EmailAddress = populateData.EmailAddress,
                Password = newPasswordHash,
                Active = true,
                Locked = false,
                Deleted = false,
            };
            var newUserId = Guid.NewGuid().ToString();

            using (var session = documentStore.OpenSession())
            {
                long count = session.Query<User>().LongCount();
                if (count == 0)
                {
                    if (populateData.Populate)
                    {
                        foreach (var user in SampleData.GetSampleData(populateData.DomainName))
                        {
                            session.Store(user, Guid.NewGuid().ToString());
                        }
                    }

                    session.Store(newUser, Guid.NewGuid().ToString());
                }

                session.SaveChanges();
            }

            return new PopulateResponse { Result = $"Populated database {populateData.DomainName}!" };
        }

        public void Get(PopulateCheckRequest request)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            long count;
            using (var session = documentStore.OpenSession())
            {
                count = session.Query<User>().LongCount();
            }

            if (count == 0)
            {
                throw HttpError.NotFound("Database is not populated");
            }
        }

        [Authenticate]
        public object Get(GetUser userById)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            User loadedUser;

            var log = this.CreateLog(
                ActivityLog.LoggedActivities.Read,
                userById.Id);

            using (var session = documentStore.OpenSession())
            {
                loadedUser = session.Load<User>(userById.Id);
                session.Store(log);
                session.SaveChanges();
            }

            return loadedUser;
        }

        [Authenticate]
        public object Get(GetUsers getUsers)
        {
            var authSession = this.SessionAs<AuthUserSession>();
            var documentStore = this.ResolveService<IDocumentStore>();
            User[] userList;
            using (var session = documentStore.OpenSession())
            {
                // Trim down the data sent since this is
                // just to show the user names and the basic states.
                userList = (from users in session.Query<User>()
                            where users.Deleted == false
                            select new User
                            {
                                FirstName = users.FirstName,
                                LastName = users.LastName,
                                Active = users.Active,
                                Locked = users.Locked
                            }).ToArray();
            }

            return userList;
        }

        [Authenticate]
        public object Post(UpdateUser userToUpdate)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            var hasher = new PasswordHasher();
            User loadedUser;
            var logNotes = new StringBuilder();
            var log = this.CreateLog(
                ActivityLog.LoggedActivities.Update,
                userToUpdate.Id);
            using (var session = documentStore.OpenSession())
            {
                loadedUser = session.Load<User>(userToUpdate.Id);
                if (!userToUpdate.FirstName.IsNullOrEmpty())
                {
                    if (!userToUpdate.FirstName.Equals(loadedUser.FirstName))
                    {
                        logNotes.AppendLine(
                            $"Modified FirstName from {loadedUser.FirstName} to {userToUpdate.FirstName}");
                        loadedUser.FirstName = userToUpdate.FirstName;
                    }
                }

                if (!userToUpdate.LastName.IsNullOrEmpty())
                {
                    if (!userToUpdate.LastName.Equals(loadedUser.LastName))
                    {
                        logNotes.AppendLine($"Modified LastName from {loadedUser.LastName} to {userToUpdate.LastName}");
                        loadedUser.LastName = userToUpdate.LastName;
                    }
                }

                if (!userToUpdate.EmailAddress.IsNullOrEmpty())
                {
                    if (!userToUpdate.EmailAddress.Equals(loadedUser.EmailAddress))
                    {
                        logNotes.AppendLine(
                            $"Modified EmailAddress from {loadedUser.EmailAddress} to {userToUpdate.EmailAddress}");
                        loadedUser.EmailAddress = userToUpdate.EmailAddress;
                    }
                }

                if (!userToUpdate.Password.IsNullOrEmpty())
                {
                    logNotes.AppendLine($"Modified Password");

                    loadedUser.Password = hasher.HashPassword(userToUpdate.Password);
                }

                if (userToUpdate.Locked != loadedUser.Locked)
                {
                    logNotes.AppendLine($"Modified {nameof(loadedUser.Locked)} from {loadedUser.Locked} to {userToUpdate.Locked}");
                    loadedUser.Locked = userToUpdate.Locked;
                }

                if (userToUpdate.Active != loadedUser.Active)
                {
                    logNotes.AppendLine($"Modified {nameof(loadedUser.Active)} from {loadedUser.Active} to {userToUpdate.Active}");
                    loadedUser.Active = userToUpdate.Active;
                }

                log.Notes = logNotes.ToString();
                session.Store(log);
                session.SaveChanges();
            }

            return loadedUser;
        }

        [Authenticate]
        public object Post(CreateUser userToAdd)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            var passwordHasher = new PasswordHasher();
            var newPasswordHash = passwordHasher.HashPassword(userToAdd.FirstName);
            var newUser = new User
            {
                FirstName = userToAdd.FirstName,
                LastName = userToAdd.LastName,
                EmailAddress = userToAdd.EmailAddress,
                Password = newPasswordHash,
                Active = true,
                Locked = false,
                Deleted = false,
            };
            var newUserId = Guid.NewGuid().ToString();
            var log = this.CreateLog(
                ActivityLog.LoggedActivities.Create,
                newUserId);

            log.Notes =
                $"Created {newUser.FirstName} {newUser.LastName} {newUser.EmailAddress} by default as active and unlocked with default password";

            using (var session = documentStore.OpenSession())
            {
                session.Store(newUser, newUserId);
                session.Store(log);
                session.SaveChanges();
            }

            return newUser;
        }

        [Authenticate]
        public object Post(UserDelete userToDelete)
        {
            var documentStore = this.ResolveService<IDocumentStore>();
            User loadedUser;
            var log = this.CreateLog(
                ActivityLog.LoggedActivities.Delete,
                userToDelete.Id);
            using (var session = documentStore.OpenSession())
            {
                loadedUser = session.Load<User>(userToDelete.Id);
                loadedUser.Deleted = true;
                session.Store(log);
                session.SaveChanges();
            }

            return loadedUser;
        }

        [Authenticate]
        public object Get(GetActivityRecords getLogs)
        {
            ActivityLog[] logs;
            var documentStore = this.ResolveService<IDocumentStore>();
            using (var session = documentStore.OpenSession())
            {
                logs = (from log in session.Query<ActivityLog>()
                        let u = RavenQuery.Load<User>(log.UserId)
                        let s = RavenQuery.Load<User>(log.SubjectUserId ?? log.UserId)
                        select new ActivityLog
                        {
                            Time = log.Time,
                            Notes = log.Notes,
                            RemoteIP = log.RemoteIP,
                            UserAgent = log.UserAgent,
                            UserId = $"{u.FirstName} {u.LastName}",
                            SubjectUserId = $"{s.FirstName} {s.LastName}",
                            Activity = log.Activity,
                        }).
                OrderByDescending(x => x.Time).Take(getLogs.Count ?? int.MaxValue).ToArray();
            }

            return logs;
        }

        private ActivityLog CreateLog(
            ActivityLog.LoggedActivities activity,
            string subjectUserId = null)
        {
            return new ActivityLog
            {
                Activity = activity,
                SubjectUserId = subjectUserId,
                Time = DateTime.Now,
                RemoteIP = this.Request.RemoteIp,
                UserAgent = this.Request.UserAgent,
                UserId = this.GetSession().UserAuthId,
            };
        }
    }
}