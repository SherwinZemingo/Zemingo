namespace Zemingo_Tests.TestData
{
    public class Person : IComparable<Person>
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int TotalMark { get; set; }

        public Person(string name, int id, int totalMark)
        {
            Name = name;
            Id = id;
            TotalMark = totalMark;
        }

        public int CompareTo(Person person) => this.TotalMark.CompareTo(person.TotalMark);
    }
}
