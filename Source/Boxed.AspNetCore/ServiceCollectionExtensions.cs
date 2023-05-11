namespace Boxed.AspNetCore;

using System;
using Boxed.AspNetCore.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the HTTP exception handling middleware.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection AddHttpException(this IServiceCollection services) =>
        services.AddSingleton<HttpExceptionMiddleware>();

    /// <summary>
    /// Adds the server timing middleware.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection AddServerTiming(this IServiceCollection services) =>
        services.AddSingleton<ServerTimingMiddleware>();

    /// <summary>
    /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
    /// used to conditionally configure the MVC services.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="condition">If set to <c>true</c> the action is executed.</param>
    /// <param name="action">The action used to configure the MVC services.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection AddIf(
        this IServiceCollection services,
        bool condition,
        Func<IServiceCollection, IServiceCollection> action)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            services = action(services);
        }

        return services;
    }

    /// <summary>
    /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
    /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally
    /// configure the MVC services.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
    /// <paramref name="elseAction"/> is executed.</param>
    /// <param name="ifAction">The action used to configure the MVC services if the condition is <c>true</c>.</param>
    /// <param name="elseAction">The action used to configure the MVC services if the condition is <c>false</c>.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection AddIfElse(
        this IServiceCollection services,
        bool condition,
        Func<IServiceCollection, IServiceCollection> ifAction,
        Func<IServiceCollection, IServiceCollection> elseAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(ifAction);
        ArgumentNullException.ThrowIfNull(elseAction);

        if (condition)
        {
            services = ifAction(services);
        }
        else
        {
            services = elseAction(services);
        }

        return services;
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services
            .Configure<TOptions>(configuration)
            .AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services
            .Configure<TOptions>(configuration, configureBinder)
            .AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation using the default failure message on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(validation);

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .Validate(validation)
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation using the default failure message on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(validation);

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations()
            .Validate(validation)
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="failureMessage">The failure message to use when validation fails.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        string failureMessage)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(validation);
        ArgumentNullException.ThrowIfNull(failureMessage);

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .Validate(validation, failureMessage)
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation on application startup.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="failureMessage">The failure message to use when validation fails.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        string failureMessage,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(validation);
        ArgumentNullException.ThrowIfNull(failureMessage);

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations()
            .Validate(validation, failureMessage)
            .ValidateOnStart();
        return services.AddSingleton(static x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }
}
