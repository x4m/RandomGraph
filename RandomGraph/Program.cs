using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RandomGraph
{
    class Program
    {
        static List<Person> population;
        static void Main(string[] args)
        {
            population = Enumerable.Range(0,10).Select(i=>GetRandomN()).ToList();

            for (int i = 0; i < 1e7; i++)
                MarrySomeone();


            foreach (var person in population)
            {
                Console.WriteLine(person);
            }
            Console.ReadLine();
        }

        private static void MarrySomeone()
        {
            var man = GetRandomSymbol(population);
            if(man.Woman)
                return;
            if(man.Spouse !=null)
                return;

            var woman = GetRandomSymbol(population);

            if (!woman.Woman)
                return;

            if(woman.Spouse!=null)
                return;

            if(IsIncest(man,woman))
                return;

            var dateDiff = Math.Abs((woman.BirthDay - man.BirthDay).TotalDays);
            if(dateDiff>rnd.Next(30*365))
                return;//too different age
            man.Spouse = woman;
            woman.Spouse = man;

            woman.MaidenName = woman.F;
            if (rnd.Next(10) != 2)
            {
                woman.F = man.F+'а';
            }

            //omen
            for (int i = 0; i < rnd.Next(5); i++)
            {
                ProduceAChild(man, woman);
            }
        }

        private static bool IsIncest(Person man, Person woman)
        {
            if (woman.Mother == null || man.Mother == null)
                return false; // we do not know
            HashSet<Person> relatives = new HashSet<Person>();
            if (man.Father != null)
            {
                relatives.Add(man.Father);
                relatives.Add(man.Mother);
                if (man.Father.Father != null)
                {
                    relatives.Add(man.Father.Father);
                    relatives.Add(man.Father.Mother);
                }

                if (man.Mother.Father != null)
                {
                    relatives.Add(man.Mother.Father);
                    relatives.Add(man.Mother.Mother);
                }
            }
            if (woman.Father != null)
            {
                if (relatives.Contains(woman.Mother))
                    return true;
                if (relatives.Contains(woman.Father))
                    return true;

                if (woman.Father.Father != null)
                {
                    if (relatives.Contains(woman.Father.Mother))
                        return true;
                    if (relatives.Contains(woman.Father.Mother))
                        return true;
                }

                if (woman.Mother.Father != null)
                {
                    if (relatives.Contains(woman.Mother.Mother))
                        return true;
                    if (relatives.Contains(woman.Mother.Mother))
                        return true;
                }
            }
            return false;
        }

        private static void ProduceAChild(Person father, Person mother)
        {
            var person = GetRandomN();
            person.F = father.F;
            person.BirthDay =
                (mother.BirthDay > father.BirthDay ? mother.BirthDay : father.BirthDay).AddYears(16)
                    .AddDays(rnd.Next(40*365));
            var indexOf = Array.IndexOf(names,father.I);
            if (person.Woman)
            {
                person.O = wpatronimics[indexOf];
                person.F += 'а';
            }
            else
            person.O = patronimics[indexOf];
            population.Add(person);
        }


        /// <summary>
        /// Словарь имён
        /// </summary>
        static string[] names = new[]{
                "Андрей",
                "Иван",
                "Пётр",
                "Серегей",
                "Василий",
                "Фёдор",
                "Евгений",
                "Никита",
                "Денис",
                "Константин",
                "Владимир",
                "Олег",
                "Дмитрий"
            };

        static string[] wnames = new[]{
                "Алёна",
                "Елена",
                "Мария",
                "Анна",
                "Татьяна",
                "Ксения",
                "Ольга",
                "София",
                "Александра",
                "Алиса",
                "Надежда",
                "Дарья",
                "Галина"
            };

        /// <summary>
        /// Словарь отчеств
        /// </summary>
        static string[] patronimics = new[]{
                "Андреевич",
                "Иванович",
                "Пётрович",
                "Серегеевич",
                "Васильевич",
                "Фёдорович",
                "Евгеньевич",
                "Константинович",
                "Денисович",
                "Никитович",
                "Владимирович",
                "Олегович",
                "Дмитриевич"
            };

        static string[] wpatronimics = new[]{
                "Андреевна",
                "Ивановна",
                "Пётровна",
                "Серегеевна",
                "Васильевна",
                "Фёдоровна",
                "Евгеньевна",
                "Константиновна",
                "Денисовна",
                "Никитична",
                "Владимировна",
                "Олеговна",
                "Дмитриевна"
            };

        /// <summary>
        /// Словарь согласных букв
        /// </summary>
        static string[] consonants = new[]{
            "в","г","д","ж","з","к","л","м","н","п","р","с","т","ф","х","ц","ч","ш","щ","б"
        };

        /// <summary>
        /// Словарь гласных букв
        /// Частоты подогнаны для правдоподобия фамилий
        /// </summary>
        static string[] vowels = new[]{
            "о","а","у","и","е",
            "о","а","у","и","е",
            "о","а","у","и","е",

            "ы","э","я","ю","ё",
        };

        /// <summary>
        /// окончания фамилий
        /// </summary>
        static string[] endings = new[]{
            "ов","ин","ев","ин"
        };

        private static Person GetRandomN()
        {
            var sb = new StringBuilder();

            bool woman = rnd.Next(2) == 0;


            var name = GetRandomSymbol(woman?wnames:names);


            var surname = GetRandomSymbol(woman?wpatronimics:patronimics);


            int bigramCount = rnd.Next(3) + 1;//выбор количества слогов в фамилии (от 1 до 3)

            for (int i = 0; i < bigramCount; i++)
            {
                if (i == 0)
                    sb.Append(GetRandomSymbol(consonants).ToUpper());//Заглавная буква фамилии - большая
                else
                    sb.Append(GetRandomSymbol(consonants));

                sb.Append(GetRandomSymbol(vowels));//добавление гласной буквы слога
            }


            sb.Append(GetRandomSymbol(consonants));//завершающая согласная и окончание
            sb.Append(GetRandomSymbol(endings));
            if (woman) sb.Append('а');


            return new Person(sb.ToString(),name,surname,woman,new DateTime(1800,1,1).AddDays(rnd.Next(365)));
        }



        static Random rnd = new Random(1);

        /// <summary>
        /// Функция выбора из массива случайного элемента
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static T GetRandomSymbol<T>(T[] array)
        {
            return array[rnd.Next(array.Length)];
        }
        private static T GetRandomSymbol<T>(List<T> array)
        {
            return array[rnd.Next(array.Count)];
        }
    }

    internal class Person
    {
        private string f, i, o;
        private readonly bool _woman;
        private DateTime _birthDay;

        public Person(string f, string i, string o, bool woman, DateTime birthDay)
        {
            this.f = f;
            this.i = i;
            this.o = o;
            _woman = woman;
            _birthDay = birthDay;
        }

        public string F
        {
            get { return f; }
            set { f = value; }
        }

        public string I
        {
            get { return i; }
        }

        public string O
        {
            get { return o; }
            set { o = value; }
        }

        public bool Woman
        {
            get { return _woman; }
        }

        public DateTime BirthDay
        {
            get { return _birthDay; }
            set { _birthDay = value; }
        }

        public Person Spouse { get; set; }
        public Person Father { get; set; }
        public Person Mother { get; set; }
        public string MaidenName { get; set; }

        public override string ToString()
        {
            var format = string.Format("F: {0}, I: {1}, O: {2}, Woman: {3}, BirthDay: {4}", f, i, o, _woman, _birthDay);
            
            return format;
        }
    }
}
