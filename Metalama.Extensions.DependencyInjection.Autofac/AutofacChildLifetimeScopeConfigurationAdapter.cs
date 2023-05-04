using Autofac;

namespace Metalama.Extensions.DependencyInjection.Autofac;

/// <summary>
/// Configuration adapter for <see cref="AutofacChildLifetimeScopeConfigurationAdapter"/>.
/// </summary>
public class AutofacChildLifetimeScopeConfigurationAdapter
{
    private readonly List<Action<ContainerBuilder>> _configurationActions = new();

    /// <summary>
    /// Adds a configuration action that will be executed when the child <see cref="ILifetimeScope"/> is created.
    /// </summary>
    /// <param name="configurationAction">Action on a <see cref="ContainerBuilder"/> that adds component registrations to the container.</param>
    /// <exception cref="ArgumentNullException">Throws when the passed configuration-action is null.</exception>
    public void Add(Action<ContainerBuilder> configurationAction)
    {
        if (configurationAction == null)
        {
            throw new ArgumentNullException(nameof(configurationAction));
        }

        _configurationActions.Add(configurationAction);
    }

    /// <summary>
    /// Gets the list of configuration actions to be executed on the <see cref="ContainerBuilder"/> for the child <see cref="ILifetimeScope"/>.
    /// </summary>
    public IReadOnlyList<Action<ContainerBuilder>> ConfigurationActions => _configurationActions;
}