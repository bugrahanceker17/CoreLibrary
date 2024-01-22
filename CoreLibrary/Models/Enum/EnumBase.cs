using System;

namespace CoreLibrary.Models.Enum
{
    public abstract class EnumBase : IComparable
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Guid? UniqueKey { get; set; }

        protected EnumBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        protected EnumBase(int id, string name, Guid uniqueKey)
        {
            Id = id;
            Name = name;
            UniqueKey = uniqueKey;
        }
        
        public int CompareTo(object obj) => Id.CompareTo(((EnumBase)obj).Id);
    }
}