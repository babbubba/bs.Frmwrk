# bs.Frmwrk
Base framework for NET CORE 6 WEB API applications

## Dependency Injection

All class injection are managed by Dependency Injection.

Not all of the class you implements must be register in the service provider because there is a bootstrap function of the framework that will try to register services and repository that implements specified interfaces and classes of the framework.

### BsService class

This is the service base class that all application services of your application must to implement.

Every service class that you implements in this way will be automatically registered in DI container at the framework Bootstrap.

> Pay attention to the ILogger object that you must to pass in the base class constructor.
>
> This is the Microsoft.Extensions.Logging abstaction logger implementation so you must to remeber that it is a generic class that need the type of the current class as generic parameter.
>
> For example in the AuthService class constructor you will see something like:
>
> ```
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
> ```
> ILogger<AuthService>
> ```



## Authentication

### Models

The *User* model must to implements the interface **IUserModel** to be used in *AuthService* authentication process.

If you want to enable user's role you must  to implement **IRoledUser** interface in your *User* model.

If you want to enable Keepalive you must to implements **IKeepedAliveUser** interface in your *User* model.

### Repository

You must to implement the **IAuthRepository** interface in your repository to handle the persistence of the user.

