# Giggle


Store Non-Sensitive Configuration in *appsettings.json* : configuration settings that are needed by the application.
Store Sensitive Information in Environment Variables using a *.env* file: JWT secrets, database connection strings, and API keys in environment variables. 

note
it is excluded from source control using .gitignore.

 RSA asymmetric encryption with JWT and role-based access control (RBAC) in a .NET 8 MVC application, follow these steps:



 Organize your project into the following folders to separate concerns and improve code readability:

Controllers: Contains all the controller classes responsible for handling HTTP requests and returning responses.
Models: Includes the data models, entities, and view models.
Views: Contains the Razor views (.cshtml files) used to render the UI.
Services: Houses business logic and service classes.
Repositories: Contains classes responsible for data access and repository patterns.
Helpers/Utilities: Utility classes and helper methods.
wwwroot: Static files such as CSS, JavaScript, and images.
Migrations: Entity Framework Core migrations for database versioning.
Middlewares: Custom middleware classes.


Use separate models for domain entities, data transfer objects (DTOs), and view models to clearly distinguish between different purposes:

Domain Models: Represent the core business objects.
DTOs: Used for data transfer between layers.
View Models: Used to pass data from controllers to views.

Implement global error handling to manage exceptions uniformly across the application.


Routing:
Ensure that your Blazor and MVC routes do not conflict. Use distinct route patterns for Blazor components and MVC controllers.

Dependency Injection:
Services added in Program.cs are available to both Blazor components and MVC controllers.

Static Files:
Static files (e.g., CSS, JavaScript) should be placed in the wwwroot folder and can be accessed in both Blazor and MVC views.



Razor:

When you need server-side rendering and strong SEO support.
For traditional web applications where server-side processing is preferred.
When you are already using ASP.NET MVC or Razor Pages and need to add some dynamic content.
Blazor:

For building modern, interactive, single-page applications (SPAs).
When you prefer using C# over JavaScript for client-side development.
For applications requiring rich client-side interactivity and real-time updates.
When you want to leverage .NET for both client and server development.
Both Razor and Blazor have their unique strengths and use cases, and the choice between them depends on the specific needs of your application.


Feature	Razor	Blazor
Rendering	Server-side	Client-side (WASM) or Server-side
Language	C# and HTML in Razor syntax	C# and HTML in Razor syntax
Component Model	Limited component model	Full-fledged component model
Interactivity	Limited, needs JavaScript for dynamic UI	Rich interactivity with C#
Use Case	Traditional server-rendered web apps	Single-page applications, dynamic UIs
SEO	Strong (server-side rendering)	Moderate (client-side), Strong (server-side)
Development Experience	Similar to traditional ASP.NET MVC	Modern SPA development experience
Data Binding	Basic data binding with model and view	Advanced data binding with components


Blazor
What is Blazor?

Blazor is a framework for building interactive web applications using C# and .NET.
It supports both client-side and server-side hosting models.
How it works:

Blazor Server: Runs on the server and communicates with the client over a SignalR connection.
Blazor WebAssembly (WASM): Runs directly in the browser using WebAssembly.
Key features:

Full-stack web development with .NET.
Interactive and dynamic client-side UI with C# instead of JavaScript.
Reusable components.
Supports both client-side and server-side rendering models.
Rich component model similar to frameworks like React or Angular.


Razor
What is Razor?

Razor is a markup syntax for embedding server-based code into webpages.
It's used primarily in ASP.NET MVC and Razor Pages to generate HTML dynamically on the server side.
How it works:

Razor syntax is integrated into .cshtml files.
It allows you to mix HTML and C# code.
Code runs on the server and generates HTML, CSS, and JavaScript that is sent to the client's browser.
Key features:

Server-side rendering.
Supports traditional web development paradigms.
Ideal for scenarios where SEO is important, as HTML is fully rendered on the server before being sent to the client.
Easy integration with existing ASP.NET MVC or Razor Pages projects.



https://cryptotools.net/rsagen
asymmetric


1: Encode RSA keys in Base64
[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes((Get-Content -Raw -Path "private_key.pem"))) > private_key_base64.txt
[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes((Get-Content -Raw -Path "public_key.pem"))) > public_key_base64.txt
