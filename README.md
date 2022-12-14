# bs.Frmwrk
Base framework for NET CORE 6 WEB API applications.

The purpose of this object is to manage most of the common part of developments needed by a new web api project.

It manages Authentication, Exception Handling, Security, Data Access, Model Mappings, etc...

It use different thirdy part components like: nHibernate and Automapper.

## Getting started

To use the framework in your entry point class (usually the program.cs file or the startup.cs file) you must to bootstrap the framework using the WebApplicationBuilder extension function like this:

```
builder.BootstrapFrmwrk();
```

The bootstrap function will execute the initialization in this order:

1. Configures the Configuration Manager setting the config file to load (see Configuration File);
2. Read the configuration file section and register in the DI the settings models: *LoggingSettings, DbContext, CoreSettings, SecuritySettings, FileSystemSettings, EmailSettings*;
3. Initializes the logging framework (Serilog) and register it in the DI (it will be used by Microsoft Logger Abstraction Logger Factory);
4. Configures the localization;
5. Initializes the O.R.M. (nHibernate);
6. Configures the Authorization and Token services and registers them, sets the JWT token handler for API and SIgnalR requests and registers the authorization policy profiles for roles;
7. Configures the File System for file uploading and downloading and sets the upload limits;
8. Loads external dll files for pluginable applications;
9. Registers all the repositories in the DI;
10. Registers all the Service in the DI;
11. Configures the mapper (Automapper) and load profiles form the Current Domain Assemblies;
12. Sets the CORS policy;
13. Registers the API controllers;
14. Initialize the Swagger;
15. Registers Signla R;

## Configuration File

The configuration file is loaded at startup by the bootstrap function and the file fall to be in the root foolder of the project and has to be named: 'configuration.ENVIRONMENT.json' where environment is the environment variable settend by the compiler (usually 'development' in debug and 'production' in release).

### Sections

[WIP]

## Dependency Injection

The injection is managed by Microsoft Dependency Injection.

When you implement yuor service or reposotory classe you dont need to register them in the service provider because there is a bootstrap function of the framework that will try to register the classes that implements specified interfaces of the framework.

### BsService class

This is the service base class that all application services class of your application have to implement.

Every service class that you implements in this way will be automatically registered in DI container during framework Bootstrap.

> Pay attention to the ILogger object that you must to pass in the base class constructor.
>
> This is the Microsoft.Extensions.Logging abstaction logger implementation so you must to remeber that it is a generic class that need the type of the current class as generic parameter.
>
> For example in the AuthService class constructor you will see something like:
>
> ```c#
> public class AuthService : BsService, IAuthService
> {
>     protected readonly IAuthRepository authRepository;
>     private readonly ITokenService tokenService;
>     private readonly ISecurityService securityService;
> 
>     public AuthService(ILogger<AuthService> logger, ITranslateService translateService, IMapper mapper, IUnitOfWork unitOfWork,
>         IAuthRepository authRepository, ITokenService tokenService, ISecurityService securityService) : base(logger, translateService, mapper, unitOfWork)
>     {
>         this.authRepository = authRepository;
>         this.tokenService = tokenService;
>         this.securityService = securityService;
>     }
>     
>     [...]
> ```
>
> The factory logger of the application registered in the DI container will be resolve the right instance of the logger here because we set the generic parameter to the class name:
>
> ```c#
> ILogger<AuthService>
> ```

The service needs the instance of Logger, Translate Service, Mapper and Unit Of Work (for data transactions) that will be accessible in your class by the fields named: logger, translateService, mapper and unitOfWork;

### Repository class

When you implement a repository class you have to implements this base class and its constructor (that simply needs the Unit of Work inerface provided by DI nad already registered by bootstrap function).

The class implementing this base class will be automatically registered in DI during boortstrap.

The base class implements the common methods to access data like:

- *Create<T>* and *CreateAsync<T>* that save an entity model in the current ORM session (if the entity model implements the *IAuditableEntity* interface it will populates the CreationDate property of the model);
- *Delete<T>* and *DeleteAsync<T>*;
- *DeleteLogically<T>* and *DeleteLogicallyAsync<T>* for entity models that implements the *ILogicallyDeletableEntity* interface;
- *RestoreLogically<T>* and *RestoreLogicallyAsync<T>* the opposite action of DeleteLogically for entity models that implements the *ILogicallyDeletableEntity* interface;
- *GetById<T>* and *GetByIdAsync<T>* thet retrieve the entity model by its identifier;
- *Query<T>* that returns the IQueryable<T> object for the desired entity model;
- *QueryLogicallyDeleted<T>* the same as Query function but already filtered for logically deleted entities;
- *Update<T>* and *UpdateAsync<T>* that update an entity model in the current ORM session (if the entity model implements the *IAuditableEntity* interface it will populates the LastUpdateDate property of the model);

If you need to use the advanced fuction of nHibernate you can acces to the nHibernate Session object using the unitOfWork object instanced by the DI in the constructor (but you need to assign it to a field in your repository class because the base reference to instance is not public) then you will find the *Session* object in the Session property of UnitOfWork instance (for ex.: *unitOfwork.Session*).

N.B.: All entity model class need to implements the IPersistentEntity interface to be used in the repositories and to be registered by the O.R.M. bootstrap.

## Authentication

### How to implement authentication

To implement the framework autentication you need to implement the models for the users (and eventually the roles and the permissions). You have to implement one or more repository too. You have to implements specified interface to do that in order to make the entire application consistent and allow all the framework features.

#### Models

You have to implement your user model class in the application to use the framework authentication process.

The *User* model must to implements the interface **IUserModel**.

If you want to enable user's roles you must  to implement **IRoledUser** interface in your *User* model.

If you want to enable user's permissions  you must  to implement **IPermissionedUser** interface in your *User* model.

If you want to enable Keepalive you must to implements **IKeepedAliveUser** interface in your *User* model.

#### Repository

You must to implement the **IAuthRepository** interface in your repository to handle the persistence of the user.

You need to implement the **ISecurityRepository** interface too (it is better if you use the same class for both IAuthRepository and ISecurityRepository to avoid some method duplication, by the way you are free to choose your preferred implementation).

### Using authentication

Every action in the controler that you decorate with the attribute **Authorize** will automatically active the process of authentication that check the *JWT access token* passed in the request header. If the *access token* is not valid or it is expired the autentication  middleware automatically return a response with status code 401 to the consumer.

The cosumer that receive the 401 response should, at this point, request authentication again if the *refresh token* is expired too or can request the token refresh to get a new valid access token.

N.B.: if an action has the Authorize attribute and the request header dont contain the token it will reply with the 401 status code too.

[WIP]
