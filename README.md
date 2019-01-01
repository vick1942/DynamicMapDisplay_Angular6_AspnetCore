# DynamicMapDisplay_Angular6_AspnetCore
To display the dynamic google map and view the distance and location specified within the map with label details using Asp.Net Core 2.0 and Angular 6

This solution conlcuded of two projects 
1) Asp.Net Core 2.0 + Angular 6
2) Windows Service

Use these commands to update from Angular 4 to Angular 6<br />
npm uninstall -g @angular/cli<br />
npm cache clean<br />
npm install -g @angular/cli@latest<br />
npm install -g npm-check-updates<br />
ncu -u<br />

Run the following commands in Command Prompt before running the web application.

A. npm install<br />
B. npm run build<br />
C. npm run build:prod<br />

Functionalities that included in the Project are as follows:-
API:-
1) Dependency Injection
2) Event Logger which will capture all sort of LOGS and save to MongoDB using NLOG
3) Pagination with scroller
4) Advanced level Linq implementation for queries
5) Get the Results of all the docs at the first the when the user comes to the search page and save to the TEMPDB list and retrieve to the page from the next search instead of hitting to the DB.
6) Async Calls
7) Generic logic to get the location within range instead of hitting to the googleMap API
8) AutoMapper
9) Implemented the chunk logic for split the list records and map the objects to the BsonDocument.
10) Generic logic to the lat and long by hitting the googleMap API 
11) Achieved the repository pattern with ADO.NET using MongoDB drivers
12) Used BsonSerializer for filtering the MongoDB docs result in the repository.
13) Implemented all CRUD operations with MongoDB using ADO.NET
14) Used Bson query pattern to prepare the querySet for transaction.

Angular 6:-
1) Simple Grid implementation for displaying the query result with multiple check-box enable functionality
2) Used GlobalEventsManager for Emitting BehaviorSubject and Observable 
3) Implemented infinite-scroll feature for scrolling to achieve pagination
4) Displaying Map using agm-map attribute
5) Dynamic dropdown feature in the search filter <option [ngValue]="undefined">Choose a type...</option>
6) Search functionality with filters
7) Dsiplaying Labels in the map using agm-info-window attribute
8) USed Rxjx subscribe observable feature to call API's.
9) Used environment variables for Prod and DEV
10) Used Angular 4.2.5 + VisualStudio 2017 template for this App
11) We are using boot.Browser.ts file for compling,executing and running the application
12) Implemented NLOG
13) Used StaticFileOptions in startup.cs to copy the @"wwwroot", @"dist", @"assets", @"Images folders in the DIST
14) Check out the package.json for list of libraries used in this app.
15) Configure the Webpack.Config file for syncing with this wwwroot folder by using the above commands.

Attributes and events used in this app arr:-

*ngIf and [ELSE] <ng-template #providerType><br />
*ngFor<br />
(change)<br />
(click)<br />
glyphicon images<br />
[routerLink]<br />
form (ngSubmit)<br />
(input)<br />
[ngValue]<br />
[infiniteScrollDistance]<br />
[infiniteScrollThrottle]<br />
(scrolled) event<br />
 agm-map agm-marker agm-info-window for MAP<br />
<router-outlet><br />
[value]<br />
Bootstrap 3.3.7<br />
css in each component<br />
@media css<br />
 
Windows Service:- 

Purpose of this service is to read the files, validate the columns and data, chunk the list and save into the MongoDB. 
1) Nlog to capture the log event.
2) MailMessage using SmtpClient event to send the mail to the user by configuring the mail in appConfig
3) Prepared Email Template in method using razors
4) Used XSSFWorkbook HSSFWorkbook Package from Nuget to read and validate the .xlsx, .xls, .csv, .txt files
5) Validating each file as per the standard format while reading the file
6) Used two Timer events one is to check the new files every 5 seconds in the path and other is to check whether the Service is live or not.
7) Runs as a console but acts as a service
8) Dependency injection resolved using UnityContainer
9) Read the file to process it and then dump the record if successfully achieved in the success folder or dump to the failed folder if fails.
10) Hit to the googleMap Api to get the lat and long based on the address


Run this query to get the list of documents in RoboMongo or Studio 3T which are more than 50 records 
DBQuery.shellBatchSize = 11000;



Mongo DB connectionstring with Https
mongodb://UserName:Password@ServerAddress:Port/?ssl=true&replicaSet=databaseName

Create your own Temp file for importing files either PDF,XLS,.XLSX,CSV,TXT extensions.
<br />

Angular CLI useful commands<br />
npm install -g @angular/cli@latest<br />
cd <my-app><br />
<br />
 npm run build<br />
<br />
ng build --prod<br />
ng serve<br />
ng serve --open<br />
<br />
CTRL + C to kill the process<br />
<br />
https://scotch.io/courses/build-your-first-angular-website/adding-an-imagelogo-in-angular<br />

