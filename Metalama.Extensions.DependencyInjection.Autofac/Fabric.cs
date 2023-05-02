//using System;
//using System.Collections.Generic;
//using System.Text;
//using Metalama.Framework.Fabrics;

//namespace Metalama.Extensions.DependencyInjection.Autofac;

//internal class Fabric : TransitiveProjectFabric
//{
//    /// <summary>
//    /// The user can implement this method to analyze types in the current project, add aspects, and report or suppress diagnostics.
//    /// </summary>
//    public override void AmendProject(IProjectAmender amender)
//    {
//        amender.Project.DependencyInjectionOptions().RegisterFramework(new AutofacDependencyInjectionFramework());
//    }
//}