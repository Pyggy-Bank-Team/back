using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PiggyBank.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<TContext>(this IServiceCollection services, Type userType) where TContext : DbContext
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException();
            }

            var keyType = identityUserType.GenericTypeArguments[0];
            
            var contextType = typeof(TContext);
            Type userStoreType = null;
            var identityContext = FindGenericBaseType(contextType, typeof(IdentityUserContext<,,,,>));
            if (identityContext == null)
            {
                // If its a custom DbContext, we can only add the default POCOs
                userStoreType = typeof(UserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
            }
            else
            {
                userStoreType = typeof(UserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                    identityContext.GenericTypeArguments[1],
                    identityContext.GenericTypeArguments[2],
                    identityContext.GenericTypeArguments[3],
                    identityContext.GenericTypeArguments[4]);
            }
            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        }

        public static void AddIdentityServices<TUser>(this IServiceCollection services) where TUser : class
        {
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<UserManager<TUser>>();
        }
        
        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}