using JetBrains.Annotations;

namespace Feofun.Localization
{
    public class LocalizableText
    {
        public string Id { get; }

        [CanBeNull]
        public object[] Args { get; }

        private LocalizableText(string id, object[] args)
        {
            Id = id;
            Args = args;
        }
        public static LocalizableText Create(string id) => Create(id, null);
        
        public static LocalizableText Create(string id, object arg) => Create(id, new[] {arg});
        public static LocalizableText Create(string id, object[] args) => new LocalizableText(id, args);
   
    }
}