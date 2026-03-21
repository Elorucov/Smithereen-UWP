namespace SmithereenUWP.DataModels
{
    public class Entity
    {
        public int Id { get; private set; }
        public string Label { get; private set; }
        public string Description { get; private set; }

        public Entity(int id, string label, string description = null)
        {
            Id = id;
            Label = label;
            Description = description;
        }

        public override string ToString()
        {
            return Label;
        }
    }

    public class EntityWithIcon : Entity
    {
        public char Glyph { get; private set; }

        public EntityWithIcon(int id, char glyph, string label, string description = null) : base(id, label, description)
        {
            Glyph = glyph;
        }
    }
}
