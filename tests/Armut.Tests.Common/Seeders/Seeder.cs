using System;
using System.Linq;
using Armut.Api.Core;

namespace Armut.Tests.Common.Seeders
{
    public static class Seeder
    {
        public static void Seed(ArmutContext context)
        {
            var installers = typeof(UserSeeder).Assembly.ExportedTypes
                .Where(m => typeof(ISeeder).IsAssignableFrom(m) && !m.IsInterface && !m.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ISeeder>()
                .ToList();

            installers.ForEach(m => m.Seed(context));
        }
    }
}