# phonebook-angular-core-example
Phonebook angular 4 and .net core 2.0 example
ASP.NET Core 2.0 and C# for cross-platform server-side code
Angular 4 and TypeScript for client-side code
Webpack for building and bundling client-side resources
Bootstrap for layout and styling
Serilog for logging
FluentValiation for server side validation.
Autofac for DI.
LiteDB for data storage.

# Prerequisites

* The solution is built using VS2017 15.3 version and you will need similar version or higher to run as .net core 2 was officially released with this version.
* Please also make sure that nuget and npm packages are restored successfully when running, no additional actions needed. I removed node_modules so the file doesn't get big.
The project directories are self explanatory like: DB, logs...
The DB will be created automatically and no additional config needed.
The solution has a docker image and docker project included, you will need Docker for windows (https://docs.docker.com/docker-for-windows/install/) to run it. But if the docker project failed to load you are able to run the web project.
Functional details

# Functional specs brief

Each contact in the phonebook has a name and a list of phone number/s entries.
If you submit a contact that already exists, it will add a new entry to his list i.e update, otherwise it will create new contact and entry.
If entry already exists for the contact, a 409 error will be raised and user will be notified.
The search works only on the contact name.
Click on the contact name to view its list of entries.
