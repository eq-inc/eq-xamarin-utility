using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eq.Utility
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    abstract public class AbstractTranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;

        protected string Namespace
        {
            get
            {
                return this.GetType().Namespace;
            }
        }

        abstract protected string[] ResFolders { get; }

        abstract protected string ResFileName { get; }

        protected string ResourceId {
            get
            {
                System.Text.StringBuilder resourceIdBuilder = new System.Text.StringBuilder();

                resourceIdBuilder.Append(Namespace).Append(".");
                foreach(string resFolderName in ResFolders)
                {
                    resourceIdBuilder.Append(resFolderName).Append(".");
                }
                string resFileName = ResFileName;
                int resxPos = 0;
                if((resxPos = resFileName.LastIndexOf(".resx")) > 0)
                {
                    resFileName = resFileName.Substring(0, resxPos);
                }
                resourceIdBuilder.Append(resFileName);

                return resourceIdBuilder.ToString();
            }
        }

        public AbstractTranslateExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = DependencyServiceManager.Instance.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager temp = new ResourceManager(ResourceId, typeof(AbstractTranslateExtension).GetTypeInfo().Assembly);

            var translation = temp.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
