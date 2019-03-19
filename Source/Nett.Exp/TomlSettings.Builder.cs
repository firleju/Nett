using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Nett.TomlSettings;

namespace Nett
{
    public interface IEnableFeatureBuilder
    {
        IEnableFeatureBuilder ValuesWithUnit();
    }

    public static class TomlSettingsBuilderExtensions
    {
        public static ITomlSettingsBuilder EnableExperimentalFeatures(
            this ITomlSettingsBuilder b, Action<IEnableFeatureBuilder> builder)
        {
            var expBuilder = (IExpSettingsBuilder)b;
            var featureBuilder = new EnableFeaturesBuilder(expBuilder);
            builder(featureBuilder);
            return b;
        }

        private class EnableFeaturesBuilder : IEnableFeatureBuilder
        {
            private readonly IExpSettingsBuilder nettBuilder;

            public EnableFeaturesBuilder(IExpSettingsBuilder nettBuilder)
            {
                this.nettBuilder = nettBuilder;
            }

            public IEnableFeatureBuilder ValuesWithUnit()
            {
                this.nettBuilder.EnableExperimentalFeature(ExperimentalFeature.ValueWithUnit, true);
                return this;
            }
        }
    }
}
