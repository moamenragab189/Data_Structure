using DataStructureProject.Classes;
//test
GenircClass<Person> LestPerson=new GenircClass<Person>(new PersonEquality());
LestPerson.Add(new Person() { Id = 1,Name="ahmed" }); 
LestPerson.Add(new Person() { Id = 2,Name="ali" });
Person Person1= new Person();
Person Person2= new Person();
Person1.Id = 3;
Person1.Name = "moamen";
Person1.Age = 20;
Person2.Id = 4;
Person2.Name = "mohamed";
Person2.Age = 30;
LestPerson.Insert(0,Person1);
LestPerson.Insert(1,Person2);
Person[] people = new Person[] { new Person {Name="mahmoud" },new Person() {Name="sayd" } };
LestPerson.InsertRange(1,people);