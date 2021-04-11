namespace GameLibrary
{
    public interface ISetting
    {
        public SettingType Type { get; set; }
        public dynamic Value { get; set; }

        public void Load();
        public void Save(dynamic newValue);
    }
}
