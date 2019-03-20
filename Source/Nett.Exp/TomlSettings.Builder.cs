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

        public static IConversionSettingsBuilder<TCustom, TomlFloat> ToToml<TCustom>(
                    this IConversionSettingsBuilder<TCustom, TomlFloat> cb, Func<TCustom, double> conv, Func<TCustom, string> unit)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlFloat>)cb).AddConverter(
                new TomlConverter<TCustom, TomlFloat>((root, customValue) =>
                {
                    return new TomlFloat(root, conv(customValue))
                    {
                        Unit = unit(customValue),
                    };
                }));
            return cb;
        }

    }
}
