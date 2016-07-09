namespace XamlIconMerger
{
    public class StringElementSource : IElementSource
    {
        public StringElementSource(string elementName, string value)
        {
            ElementName = elementName;
            ElementInfo = $"string {elementName}";
            Text = value;
        }

        public string Text { get; set; }

        public string ElementName { get; }
        public string ElementInfo { get; }

        public string GetContent()
        {
            return Text;
        }
    }
}