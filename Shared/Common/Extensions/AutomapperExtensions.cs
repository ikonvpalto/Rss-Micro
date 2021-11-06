using AutoMapper;

namespace Common.Extensions
{
    public static class AutomapperExtensions
    {
        public static void AssertConfigurationIsValid(this Profile profile)
        {
            var profileCopy = profile;
            var cfg = new MapperConfiguration(x => x.AddProfile(profileCopy));

            cfg.AssertConfigurationIsValid();
        }
    }
}
