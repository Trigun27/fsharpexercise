using System;

namespace ConsoleApp1
{
    public class Person
    {
        public readonly string FullName;

        public Person(string firstName, string lastName)
        {
            FullName = $"{firstName} {lastName}";
        }
    }


    public class LazyPerson
    {
       

    }
}