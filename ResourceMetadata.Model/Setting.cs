using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Model
{
    public class Setting: BaseEntity
    {
        /// <summary>
        /// Key field of setting
        /// </summary>
        public string Name { get; set; }

        public int OrderNumber { get; set; }

        /// <summary>
        /// Text field of setting
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Value field of setting
        /// </summary>
        public string Value { get; set; }

        public Setting() { }
    }
}