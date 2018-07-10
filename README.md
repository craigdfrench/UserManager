# User Manager Coding Sample

  * User Manager is a single page app with CRUD for users and with live activity logging. 
  * Uses REST backend. 
  * Tech stack is C#, .NET, ASP.NET, ServiceStack, RavenDB, Boostrap, and jQuery.
  * Frontend UI built on top of [Service Stack sample application EmailContacts](https://github.com/ServiceStackApps/EmailContacts)
  
1. [Application Walkthrough](#application-walkthrough)
2. [Design Notes](#design-notes)
3. [Developer Reflection](#developer-reflection)

The base frontend design reuses https://github.com/ServiceStackApps/EmailContacts and modified for use in this application. 

<a id="AppWalkthrough">
  
## Application Walkthrough

So you want to manage some users?  You have come to the right place. 

After performing a git clone to your machine, you can compile and run the project by using the startme.bat file which uses a previously installed version of Visual Studio 2017 Professional Version - may also work for other versions.

After the code is compiled and run, chrome browser will be launched and you will see the the application - once it loads. 

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

![Logout](https://github.com/craigdfrench/UserManager/blob/master/documentation/logout.PNG?raw=true "Logout")

# Design Notes 

The requirements for this application called for similar functionality as the [Service Stack sample application EmailContacts](https://github.com/ServiceStackApps/EmailContacts) so I used the base UI as the starting point to maximize code reuse.

There is [excellent documentation for this app found on their Github page]( 
https://github.com/ServiceStackApps/EmailContacts/blob/master/README.md) and the most relevant section to the User Interface piece is the [API First Development section](https://github.com/ServiceStackApps/EmailContacts/blob/master/README.md#api-first-development). 

From the project description, I was unsure if authenticated users were users under management or if they were a different set of users. I made the assumption that the set of authenticated users are the same as those under management.  The current design grants all users the same level of access, as there was no requirement for different permission levels. 

The major extensions made to the EmailContacts UI were the following aspects:
  * [changes to data](#data-schema-changes) 
  * [authentication](#authentication)
  * [database initial data population](#data-population)
  * [modifying the UI to support specific modes of operation](#user-interface-modes)
      
## Data Schema Changes   

This system uses RavenDb to store the user data in a Database called UserData. It assumes it is installed and running on HTTP on port 8080 with no authentication. On a production system this would not be appropriate.

The sample application had contacts and sent emails. This application has users and activity logs.  By mapping contacts to users, and sent emails to activity logs, we are able to reuse much of the core functionality of the front end.  

The services were written from scratch since they were quite simple. Additional DTOs for changing lock status, active status, passwords and checking user account for authentication were defined but not implemented because these use cases were covered off in the update user page or were not requirements. 

The User Interface was changed from the sample application which had a delete button associated with each contact.  I wanted to use the space to highlight unusual states for users: locked and/or inactive so I moved the delete "button" to the edit screen. When using the query REST api to populate the list of users some data is excluded because it is not being logged despite returning User DTO objects.

The activity logging is a cross-cutting concern, however, the nature of activity logging in this instance requires deep integration with the changes made to the data, especially when identifying Subjects of their activities and creating appropriate notes for change records.  

The identification of the users and subject user refers to the record id so that subsequent changes are immediately reflected in the logs. Users are not deleted from the database but marked with the deleted flag and excluded from query searches. This allows the records generated by or relating to relating to deleted users to continute to exist.  

The record id schema was changed from RavenDb assigned Ids like User/2 to a randomly generated Guid to make it easier to use the Ids in HTTP URLs without having to quote them. 

The activities logging also collects the User Agent, Remote IP Address which are not shown. 

## Authentication 

Rather than duplicating records under management into RavenDbAuth I chose to implement a custom authorizer that used the data under management directly.

## Data Population

## User Interface Modes 

    * unpopulated database
    * login / unauthenticated
    * logged in / authenticated
    * create new user
    * edit user

# Developer Reflection

## Highlights

  * Demonstrates developer ability to reuse existing application framework not require greenfield coding, and not suffer from "not invented here syndrome".

  * Single page design is attractive and modern however requires considerable more effort to get it to work correctly.

  * Design made use of deep integration with ServiceStack - and required understanding of this.

  * Advantage is that there is a well written document describing the design application allowing faster developer ramp.

  * Used some promises to chain together a series of async events as a result of checking database status, to authenticiation.

## Challenges Overcomed

  * The bindForms ss-utility library caused some challenges while trying to implement multiple submit buttons.  The serializer refused to use value set by button, or honor the formaction element, so had to workaround by using anchors with a data-click action.

  * Sample app didn't have a container for the whole document for the grid layout. Once I realized this was the case, I was able to lay out portions of the screen that could be set css display:block or display:none to appear and when removed will take no space, while using css visibility: visible or hidden could hide elements that needed to have space reserved but just not shown. 
 
  * Failed to use version control when developing initial draft of this project, and it got out of control with the Nuget packages when one didn't work out and dragged a whole pile of other dependencies. I didn't think it was a big setback but I depend on it for my workflow and not losing work. 

## 
Introduced some duplication in the UI and.there may have been easier ways to do some things.
