using Armut.Api.Core;

namespace Armut.Tests.Common.Seeders
{
    public interface ISeeder
    {
        void Seed(ArmutContext context);
    }
}