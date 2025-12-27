namespace SmithereenUWP.DataModels
{
    public class Entity
    {
        public int Id { get; private set; }
        public string Label { get; private set; }

        public Entity(int id, string label)
        {
            Id = id;
            Label = label;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
