The base frontend design reuses https://github.com/ServiceStackApps/EmailContacts and modified for use in this application. 

When started up with a blank database, the user is prompted to create an initial user database.  
![Create initial user database](https://github.com/craigdfrench/UserManager/blob/master/documentation/populate.PNG?raw=true "Create initial user database")

After populating the database, the login screen is presented with credit to Bootstrap Sample.

![Login screen](https://github.com/craigdfrench/UserManager/blob/master/documentation/login.PNG?raw=true "Login screen")

When login is complete you are presented with the main screen. 

![Main screen](https://github.com/craigdfrench/UserManager/blob/master/documentation/mainscreen.PNG?raw=true "Main screen")

Notice the activity log has aleady been updated with with the user login.

The next order of business is to add a new user, Donald Duck. This is done by clicking on the plus sign beside the user and you will be presented with the create user screen.  

![Create user screen](https://github.com/craigdfrench/UserManager/blob/master/documentation/new-user.PNG?raw=true "Create user screen")

After doing this note how it showed up in the user list.

![New User In List](https://github.com/craigdfrench/UserManager/blob/master/documentation/added-new-user.PNG?raw=true "New user in list")

There is more work to be done, now let's edit Donald Duck by clicking the name on the list.  Donald got a new email address and wanted the J in his name, so we made this this happen. He also wanted to set a password.  We don't want a duck using this system so we will mark this account as locked out and inactive. 

![Edit user](https://github.com/craigdfrench/UserManager/blob/master/documentation/edit-user.PNG?raw=true "Edit user")

After making these changes there are some noticable changes on the screen.  In the user list, our changes are shown beside Donald J Duck's name  that the user is locked out and inactive. 

![Locked out and inactive symbols](https://github.com/craigdfrench/UserManager/blob/master/documentation/edited-user-mainscreen.PNG?raw=true "Locked out and inactive symbols")

As well, the detail for activity record for the edit can be expanded to see what what changed.  Note that in the activity logs the current name is what is shown since it refers to the user records rather than copying the actual text. 

![Activity detail for changes](https://github.com/craigdfrench/UserManager/blob/master/documentation/edited-user-activity-expanded.PNG?raw=true "Activity detail for changes")

On second thought, maybe creating an account for Donald Duck was a bad idea.  Let's delete the record by selecting from the user list and then clicking on the delete button as shown. 

![Edit user screen](https://github.com/craigdfrench/UserManager/blob/master/documentation/edit-user.PNG?raw=true "Edit user screen")

In fact the user record is not actually deleted from the database, it is marked as deleted but is no longer shown in the user list but is still shown correctly in the activity logs. 

![Edit user screen](https://github.com/craigdfrench/UserManager/blob/master/documentation/delete-user.PNG?raw=true "Edit user screen")

At this point, it may just be best to logout.

![Logout](https://github.com/craigdfrench/UserManager/blob/master/documentation/delete-user.PNG?raw=true "Logout")