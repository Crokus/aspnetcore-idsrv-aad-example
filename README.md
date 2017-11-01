# aspnetcore-idsrv-aad-example

Example of ASP.NET Core 2 MVC and Web API apps secured by [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) using Azure Active Directory as external auth provider.

### Configuration

1. Install [.NET Core 2.0](https://www.microsoft.com/net/download/windows#/current)
1. In IdSrv's [Startup.cs](https://github.com/Crokus/aspnetcore-idsrv-aad-example/blob/master/Wolnik.IdSrv/Startup.cs#L37) add your Azure AD Application ID.

```csharp
services.AddAuthentication()
                .AddOpenIdConnect("AAD", "Azure Active Directory", options =>
                {
                    {...}
                    options.ClientId = "[Your Azure AD Application ID]";
                    {...}
                });
        }
```
3. Set __API__, __Client__ and __IdSrv__ projects to start at startup and hit run.
