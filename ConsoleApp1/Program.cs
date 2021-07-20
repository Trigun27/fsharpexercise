using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace ConsoleApp1
{

    public static class Fp
    {
        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g) => (n) => g(f(n));

    }
    
    sealed class Program
    {

        public class PartnerInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string PhoneNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime? BirthDay { get; set; }
        }
        
        public class UrlTest
        {
            public string Url { get; set; }
        }

        public class UrlNew
        {
            public string Url { get; set; }
            
            public PartnerInfo MainPartnerInfo { get; set; }
        }

        public void JsonTest()
        {
            var test = @"{
                ""url"": ""http://debit-card-registry-front-v2-ft.omni.homecredit.ru/?sessionId=9770d81a-6540-4a40-89a5-f2fe09f00704#eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjdXJyZW50TGV2ZWw6MyBtYXhMZXZlbDo0IG9zVHlwZTppT1MiLCJjdWlkIjoiNTM1ODkzOTUiLCJzY29wZSI6WyIvYXBwbGljYXRpb24vdjEiLCIvYmlrZXIvdjEiLCIva2xhZHIvdjEiLCIvZXNpYS92MSIsIi9jYXNjYWRlci92MSIsIi9jb21taXNzYXIvdjEiLCIvdGVtcGxhdG9yL3YxIiwiL3Bhc3Nwb3J0LWlzc3Vlci92MSIsIi9maW9uYS92MSIsIi9rbGFkci1wZy1zZWFyY2gvdjEiLCIvcTVmL3YxIiwiL2NoZWNrcy92MSIsIi9jYy1zaWduL3YxIiwiL3NlcnZpY2UtbWFuYWdlbWVudC92MSIsIi9waW4vdjEvIiwiL2RlY2FyZC92MSIsIi9hYnRlc3Rlci92MSIsIi9kZWJpdC1jYXJkLXJlZ2lzdHJ5L3YxIiwiL2JvcHJvZHVjdHMvdjEiLCIvZGViaXQtY2FyZC1yZWdpc3RyeS92MiIsIi9mb3JtcmVmZXJlbmNlL3YxIiwiL2RlY2FyZC92MiIsIi9wYW5maW5kZXIvdjEiLCIvdmlzYS1waG9uZS10cmFuc2Zlci92MSIsIi91bm1hc2svdjEiLCIvY2FyZDJwaG9uZS92MSIsIi9zZWFyY2gvdjEiLCIvcGF5cGF5L3YxIiwiL2NhcmRyZWdpc3RyeS92MSIsIi9jYXJkcmVnaXN0cnkvdjEvcDJwY3JlZGl0IiwiL2NhcmRyZWdpc3RyeS92MS9zdGF0dXMiLCIvY2FyZHJlZ2lzdHJ5L3YxL2NhcmRpbmZvIiwiL2NoYW5nL3YxIiwiL29mZmVybWFuYWdlci92MSIsIi9maW4tcHJvdGVjdC1tYW5hZ2VyL3YxIiwiL2Zpbi1wcm90ZWN0LW1hbmFnZXIvdjIiLCIvZGVwb3NpdG8vdjEiLCIvc25pdGNoL3YxIiwiL3Bpbi92Mi8iLCIvdHJhbnNmZXJzL3YxIiwiL3RyYW5zZmVyL3YxIiwiL29zcy92MSIsIi9yZXBvcnRpbmctc2VydmljZSIsIi91cmxzaG9ydGVyL3YxIiwiL3RyYW5zL3YxIiwiL3NjYW5zL3YxIiwiL2RpZ2l0YWwtY2FyZC92MSIsIi9pc3N1ZS1jYXJkL3YxIiwiL3Bob25lLXZlcmlmaWNhdGlvbi92MSIsIi9lc2lhLWFjY291bnQtaW5mby92MSIsIi9waG9uZWJvb2svdjEiLCIvZG9jdW1lbnQtcGFyc2VyL3YxIiwiL2RvYy1lbmdpbmUvdjEiLCIvaW1zaS92MSIsIi90dy1zZXJ2aWNlcy1tYW5hZ2VyL3YxIiwiL3N1cHBvcnRub3RpZi92MSIsIi9qb3VybmFsaXN0L3YxIiwia2Fma2EiLCIvcGF5bWVudHMvdjEiLCIvZW1pc3Npby92MSIsIi9QYXNzQ29kZS92MSIsIi9zYmVyLXRyYW5zZmVyL3YxIiwiL3Bpbi92MyIsIi9jcmVjYXJkL3YyIiwiL2NvbnRyYWN0LWluZm8vdjEiLCIvY29vbC1jb2RlL3YyIiwiY2hhdCIsIi9saW1pdGF0b3IvdjEiLCIvbWJyL2RlbGl2ZXJ5L3YxIiwiL2Rib3NjYW4vdjEiLCIvbWJyL3BhY2thZ2VzL3YxIiwiY3VycmVudGxldmVsMCIsImN1cnJlbnRsZXZlbDEiLCJjdXJyZW50bGV2ZWwyIiwiL2licy9yZXN0IiwiY3VycmVudGxldmVsMyIsIi9pYnMvcmVzdC9hY3RpdmUiXSwiZXhwIjoxNjIxOTQyMTExLCJqdGkiOiI4ZGZiNzZhOC02ZTBmLTRhYzEtYWQ5Ny04M2IyMTJkNWIzYzciLCJjbGllbnRfaWQiOiJteV9jcmVkaXQifQ.hmMRCtd5P7CswNg1BZmAu20aJb6krxusqOtAGxrx-MaXCROYumJddxASKLqOJWoZMw-LSxmI_MEDuqS4os4-m49I44Y3w_BkN37c9znZ3GVUvCzdEGhExRhbGG_j7zqI5IyUff4hTfj0VU8theUwCoirVmNZu0IkKsXoFvsaa3QU5f8vXQdBgojf8xywNbGld05YHhMAmCmo1pcpgrnphwMg8jdWaj0WxKCnY6L6m0uQ8dJNX6Q76FqDXCbV-f71zmZ-jrHTEYghKV1JE9jX17mNB5feH4KJw8_6Xcy7wn-s6ecOobOHbflbeV72ezdyQHYL9KFZlCDwDMeaoz6wTQ"",
                ""mainPartnerInfo"": {
                    ""phoneNumber"": ""9264615912"",
                        ""birthDay"": ""1998-11-03T00:00:00+03:00""
                }
            }";
        }


        public class CoffeeBeans {}

        public class CoffeeGround
        {
            public CoffeeGround(CoffeeBeans coffeeBeans) {}
        }

        public class Espresso
        {
            public Espresso(CoffeeGround coffeeGround) { }
        }

        private Func<CoffeeBeans, CoffeeGround> grindCoffee = coffeeBeans => new CoffeeGround(coffeeBeans);
        private Func<CoffeeGround, Espresso> brewCoffee = coffeeGround => new Espresso(coffeeGround);
        
        private void CoffeeTest()
        {
            var coffeeBeans = new CoffeeBeans();
            var test = brewCoffee(grindCoffee(coffeeBeans));

            var test2 = grindCoffee.Compose(brewCoffee);
        }
        
        private static void Closer(string valueTest)
        {
            string freeVariable = "Test Fro closer";
            
            string Lambda(string value) => freeVariable + " " + value;

            Console.WriteLine(Lambda(valueTest));
            
            Console.WriteLine(Lambda("22222"));
        }

        private static void TestLazy()
        {
            Person[] freds = new Person[5];

            Lazy<Person> fredFlintstone = new Lazy<Person>(() => new Person("Fred", "Flintstone"), true);
            
            for (int i = 0; i < freds.Length; i++)
            {
                freds[i] = fredFlintstone.Value;
            }
        }

        static void Main(string[] args)
        {
            //Closer("test test");

            TestLazy();



        }
    }
}