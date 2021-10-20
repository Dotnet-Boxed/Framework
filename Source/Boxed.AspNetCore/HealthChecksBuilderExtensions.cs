namespace Boxed.AspNetCore
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="IHealthChecksBuilder"/> extension methods.
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the health check builder.
        /// </summary>
        /// <param name="healthChecksBuilder">The health checks builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">The action used to add to the health check builder.</param>
        /// <returns>The same health checks builder.</returns>
        public static IHealthChecksBuilder AddIf(
            this IHealthChecksBuilder healthChecksBuilder,
            bool condition,
            Func<IHealthChecksBuilder, IHealthChecksBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(healthChecksBuilder);
            ArgumentNullException.ThrowIfNull(action);

            if (condition)
            {
                healthChecksBuilder = action(healthChecksBuilder);
            }

            return healthChecksBuilder;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the health check builder.
        /// </summary>
        /// <param name="healthChecksBuilder">The health checks builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the health check builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the health check builder if the condition is <c>false</c>.</param>
        /// <returns>The same health checks builder.</returns>
        public static IHealthChecksBuilder AddIfElse(
            this IHealthChecksBuilder healthChecksBuilder,
            bool condition,
            Func<IHealthChecksBuilder, IHealthChecksBuilder> ifAction,
            Func<IHealthChecksBuilder, IHealthChecksBuilder> elseAction)
        {
            ArgumentNullException.ThrowIfNull(healthChecksBuilder);
            ArgumentNullException.ThrowIfNull(ifAction);
            ArgumentNullException.ThrowIfNull(elseAction);

            if (condition)
            {
                healthChecksBuilder = ifAction(healthChecksBuilder);
            }
            else
            {
                healthChecksBuilder = elseAction(healthChecksBuilder);
            }

            return healthChecksBuilder;
        }
    }
}
