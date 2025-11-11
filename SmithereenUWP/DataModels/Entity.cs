using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
