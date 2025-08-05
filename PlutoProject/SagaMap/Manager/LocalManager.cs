using SagaLib;
using SagaMap.Localization;
using SagaMap.Localization.Languages;

namespace SagaMap.Manager
{
    public class LocalManager : Singleton<LocalManager>
    {
        public enum Languages
        {
            English,
            Chinese,
            TChinese,
            Japanese
        }

        private Languages lan = Languages.English;

        public Languages CurrentLanguage
        {
            get => lan;
            set
            {
                switch (value)
                {
                    case Languages.English:
                        Strings = new English();
                        break;
                    case Languages.Chinese:
                        Strings = new Chinese();
                        break;
                    case Languages.TChinese:
                        Strings = new TChinese();
                        break;
                    case Languages.Japanese:
                        Strings = new Japanese();
                        break;
                    default:
                        Strings = new English();
                        break;
                }

                lan = value;
            }
        }

        public Strings Strings { get; private set; } = new English();

        public override string ToString()
        {
            return string.Format("{0}({1})", Strings.LocalName, Strings.EnglishName);
        }
    }
}