namespace GameLibrary
{
    public interface ISetting
    {
        public SettingType Type { get; set; }
        public dynamic Value { get; set; }
        public dynamic DefaultValue { get; set; }

        public void Load();
        public void Save(dynamic newValue);
    }
}
