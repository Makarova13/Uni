using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Lab2
{
    public enum Action { Add, Remove, Property };

    interface IDateAndCopy
    {
        object DeepCopy();
        DateTime Date { get; set; }
    }

    public enum Post
    {
        Assistant, AssociateProfessor, Professor
    }

    class Person : IDateAndCopy, IComparable, IComparer<Person>
    {
        protected string Fname;
        protected string Sname;
        protected System.DateTime Bdate;

        public Person(string name, string Sname, DateTime date)
        {
            this.Fname = name;
            this.Sname = Sname;
            this.Bdate = date;
        }

        public Person()
        {
            Fname = "Маша";
            Sname = "Кашталян";
            Bdate = new DateTime(2000, 3, 26);
        }

        public string Firstname
        {
            get
            {
                return Fname;
            }
            set
            {
                Fname = value;
            }
        }

        public string Surname
        {
            get
            {
                return Sname;
            }
            set
            {
                Sname = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return Bdate;
            }
            set
            {
                Bdate = value;
            }
        }

        public int Year
        {
            get
            {
                return Bdate.Year;
            }
            set
            {
                Bdate = new DateTime(value, Bdate.Month, Bdate.Day);
            }
        }

        public override string ToString()
        {
            return "Имя: " + Fname + "\n" + "Фамилия: " + Sname + "\n" + "Дата рождения: " + Bdate.ToShortDateString();
        }

        public virtual string ToShortString()
        {
            return "Имя: " + Fname + "\n" + "Фамилия: " + Sname;
        }

        public virtual object DeepCopy()
        {
            Person person = new Person(Fname, Sname, Bdate);
            return person;
        }

        public override bool Equals(Object obj)
        {
            if (obj != null)
            {
                return this.ToString() == obj.ToString();
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(Person p1, Person p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Person p1, Person p2)
        {
            return (!p1.Equals(p2));
        }

        interface IDateAndCopy
        {
            object DeepCopy();
            DateTime Date { get; set; }
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj != null && obj is Person)
            {
                // Person person = obj as Person;
                return string.Compare(this.Surname, ((Person)obj).Surname, StringComparison.Ordinal);
            }
            else
            {
                throw new ArgumentException("It's not Person\n");
            }
        }

        int IComparer<Person>.Compare(Person a, Person b)
        {
            Person p1 = a;
            Person p2 = b;
            if (p1 != null && p2 != null)
            {
                return p1.Bdate.CompareTo(p2.Bdate);
            }
            else
            {
                throw new ArgumentException("One of the params doesn't match\n");
            }
        }

    }

    class LecturerListHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; set; }

        public string Changes { get; set; }

        public Lecturer LecturerRef { get; set; }

        public LecturerListHandlerEventArgs(string collectionName, string changes, Lecturer lecturerRef)
        {
            CollectionName = collectionName;
            Changes = changes;
            LecturerRef = lecturerRef;
        }

        public override string ToString()
        {
            return String.Format("Collection name {0}. Changed information :{1}.", CollectionName, Changes);
        }
    }

    delegate void LecturerListHandler(object source, LecturerListHandlerEventArgs args);

    class Lecturer : Person, IDateAndCopy, INotifyPropertyChanged
    {
        private Post lpost;

        private string Pulpit;

        private int rating;

        public string NameOfChangedIt { get; set; }

        private List<Subject> SubList;

        private List<Theme> ThemeList;

        public string Department
        {
            get { return Pulpit; }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Кафедра"));
                Pulpit = value;
            }
        }

        public System.Collections.Generic.List<Theme> ThemeL
        {
            get { return ThemeList; }
            set { ThemeList = value; }
        }

        public System.Collections.Generic.List<Subject> Sbj
        {
            get { return SubList; }
            set { SubList = value; }
        }

        public Post Lpost
        {
            get { return lpost; }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Должность"));
                lpost = value;
            }
        }

        public int Rating
        {
            get { return rating; }
            set
            {
                if (value <= 0 || value >= 500)
                    throw new Exception("Рейтинг не может быть меньше 0 или больше 500!");
                rating = value;
            }
        }

        public Lecturer(Person P, string Pulpit, Post post, int rating)
            : base(P.Firstname, P.Surname, P.Date)
        {
            this.Lpost = post;
            this.Pulpit = Pulpit;
            this.Rating = rating;
            SubList = new List<Subject>();
            ThemeList = new List<Theme>();
        }

        public Lecturer()
        {
            this.lpost = Post.Assistant;
            this.Pulpit = "Кафедра";
            this.rating = 6;
            SubList = new List<Subject>();
            ThemeList = new List<Theme>();
        }

        public Person Person  //свойство типа Person; метод get свойства возвращает объект типа Person, 
        //данные которого совпадают с данными подобъекта базового класса, метод set присваивает значения полям из подобъекта базового класса
        {
            get
            {
                Person pr = new Person(base.Fname, base.Sname, base.Bdate);
                return pr;
            }
            set
            {
                Fname = value.Firstname;
                Sname = value.Surname;
                Bdate = value.Date;
            }
        }

        public void AddSubject(params Subject[] Sub)
        {
            for (int i = 0; i < Sub.Length; i++)
                SubList.Add(Sub[i]);
        }

        public int HoursSum
        {
            get
            {
                int sum = 0;
                int size = SubList.Count;
                if (size == 0)
                    return 0;
                for (int i = 0; i < SubList.Count; i++)
                {
                    sum = sum + SubList[i].Hours;
                }
                return sum;
            }
        }

        public override string ToString()
        {
            string a = "";
            string b = "";
            int i;

            for (i = 0; i < SubList.Count; i++)
                a = a + SubList[i].ToString();
            for (i = 0; i < ThemeList.Count; i++)
                b = b + ThemeList[i].ToString();
            return "Имя: " + Fname + "\n" + "Фамилия: " + Sname + "\n" + "Дата рождения: " + Bdate.ToShortDateString() + "\nКафедра: "
                + Department + ".\nПост: " + lpost + ".\nРейтинг: ";
        }

        public override string ToShortString()
        {
            return base.ToString() + ".\nРейтинг: " + Rating;
        }

        public override object DeepCopy()
        {
            Lecturer l = new Lecturer(this.Person, this.Pulpit, this.Lpost, this.Rating);
            for (int i = 0; i < SubList.Count; i++)
            {
                l.SubList.Add((Subject)SubList[i].DeepCopy());
            }
            for (int i = 0; i < ThemeList.Count; i++)
            {
                l.ThemeList.Add((Theme)ThemeList[i]);
            }
            return l;
        }

        public IEnumerable<object> Subj(int n)
        {
            foreach (var s in SubList)
            {
                if (s.Hours < n)
                    yield return s;
            }
        }

        public IEnumerable<object> Themel()
        {
            foreach (var t in ThemeList)
                yield return t;
        }

        interface IDateAndCopy
        {
            object DeepCopy();
            DateTime Date { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NameOfChangedIt = e.PropertyName;
        }
    }

    class LecturerCollection
    {
        private List<Lecturer> LectList;

        public string CollectionName { get; set; }

        public void AddDefaults()
        {
            if (LectList == null)
            {
                LectList = new List<Lecturer>();
            }
            LectList.Add(new Lecturer());
            foreach (var el in LectList)
            {
                LecturerListHandlerEventArgs e = new LecturerListHandlerEventArgs(CollectionName, "The lecturer was recently deleted.", el);
                OnLecturerChanges(e);
            }
        }

        public void AddLecturers(params Lecturer[] l)
        {
            if (LectList == null)
            {
                LectList = new List<Lecturer>();
            }
            LectList.AddRange(l);
            foreach (var el in l)
            {
                LecturerListHandlerEventArgs e = new LecturerListHandlerEventArgs(CollectionName, "The lecturer was recently added.", el);
                OnLecturerChanges(e);
            }
        }

        public override string ToString()
        {
            string a = " ";
            foreach (Lecturer l in LectList)
                a += l.ToString() + "\n########\n";
            return "\n " + a;
        }

        public string ToShortString()
        {
            string a = " ";
            foreach (Lecturer l in LectList)
                a += l.ToShortString() + "\n########\n";
            return " \n" + a;
        }

        public void SortSurName()
        {
            LectList.Sort();
        }

        public void SortDate()
        {
            LectList.Sort(new Lecturer());
        }

        public void SortRating()
        {
            LectList.Sort(new LecComparer());
        }

        public event LecturerListHandler LecturersCountChanged;

        protected virtual void OnLecturerChanges(LecturerListHandlerEventArgs e)
        {
            LecturersCountChanged?.Invoke(this, e);
        }

        public event LecturerListHandler LecturerReferenceChanged;

        protected virtual void OnLecturerReferenceChanged(LecturerListHandlerEventArgs e)
        {
            LecturerReferenceChanged?.Invoke(this, e);
        }

        public bool Remove(int j)
        {
            if (j <= 0 && j >= LectList.Count)
            {
                return false;
            }
            LectList.RemoveAt(j);
            LecturerListHandlerEventArgs e = new LecturerListHandlerEventArgs(CollectionName, "The lecturer was recently deleted.", LectList[j]);
            OnLecturerChanges(e);
            return true;
        }

        public Lecturer this[int index]
        {
            get
            {
                return LectList.ElementAt(index);
            }
            set
            {
                LecturerListHandlerEventArgs e = new LecturerListHandlerEventArgs(CollectionName, "Item of collection was changed.", value);
                OnLecturerReferenceChanged(e);
                LectList[index] = value;
            }
        }

    }

    class Raiting : IComparer<Lecturer>
    {
        public int Compare(Lecturer obj1, Lecturer obj2)
        {
            Lecturer lec1 = obj1 as Lecturer;
            Lecturer lec2 = obj2 as Lecturer;
            if (lec1 != null && lec2 != null)
            {
                if (lec1.Rating > lec2.Rating) return 1;
                else if (lec1.Rating < lec2.Rating) return -1;
                else return 0;
            }
            else throw new Exception("Imposible");
        }
    }

    static class ExctensionLecturer
    {
        public static List<Lecturer> SoringDictionary(this LecturerCollection<string> lecturerCollection)
        {
            var temp = new Lecturer();
            List<Lecturer> lecturers = lecturerCollection.LectDictionary.Values.ToList();
            lecturers.Sort((a, b) => a.Rating.CompareTo(b.Rating) * -1);
            return lecturers;
        }
    }

    class LecturerCollection<TKey>
    {
        public string CollectionName { get; set; }

        public Dictionary<TKey, Lecturer> LectDictionary = new Dictionary<TKey, Lecturer>();

        public delegate void LecturerChangedHandler<TKey>(object source, LecturersChangedEventArgs<TKey> args);
        public event LecturerChangedHandler<TKey> LecturerChanged;

        public LecturerCollection(string collection)
        {
            this.CollectionName = collection;
        }

        public LecturerCollection()
        {
            CollectionName = "First collection";
        }

        public void LecturerChangedIn(object sender, PropertyChangedEventArgs e)
        {
            TKey key = LectDictionary.FirstOrDefault(i => i.Value.Equals(sender)).Key;
            LecturerChanged?.Invoke(this, new LecturersChangedEventArgs<TKey>(CollectionName, Action.Property, e.PropertyName, key));
        }

        public bool Remove(Lecturer lec)
        {
            if (!LectDictionary.ContainsValue(lec)) return false;
            else
            {
                foreach (var item in LectDictionary.Where(kpv => kpv.Value.Equals(lec)).ToList())
                {
                    TKey key = item.Key;
                    LectDictionary.Remove(key);
                    item.Value.PropertyChanged -= this.LecturerChangedIn;
                    LecturerChanged?.Invoke(this, new LecturersChangedEventArgs<TKey>(CollectionName, Action.Remove, " ", key));
                }
                return true;
            }
        }

        public void AddDefaults(TKey key)
        {
            Lecturer lecturer = new Lecturer();
            lecturer.PropertyChanged += LecturerChangedIn;
            LectDictionary.Add(key, lecturer);
            LecturerChanged?.Invoke(this, new LecturersChangedEventArgs<TKey>(CollectionName, Action.Add, " ", key));
        }

        public void AddLecturer(TKey key, Lecturer list)
        {
            LectDictionary.Add(key, list);
            list.PropertyChanged += LecturerChangedIn;
            LecturerChanged?.Invoke(this, new LecturersChangedEventArgs<TKey>(CollectionName, Action.Add, " ", key));
        }

    }

    public class JournalEntry
    {
        public string CollectionName { get; set; }

        public Action Reason { get; set; }

        public string SourceOfChange { get; set; }

        public string Tkey { get; set; }

        public JournalEntry(string collectionName, Action reason, string source, string key)
        {
            this.CollectionName = collectionName;
            this.Reason = reason;
            this.SourceOfChange = source;
            this.Tkey = key;
        }
        public override string ToString()
        {
            return "Название коллекции:" + CollectionName +
                "\nПричина вызова события:" + Reason + 
                "\nСвойство, являющееся источником изменений:" + SourceOfChange + 
                "\nКлюч:" + Tkey + "\n\n";
        }
    }

    public class Journal<TKey>           // для накопления информации об изменениях в коллекциях типа LecturerCollection<TKey>
    {
        private List<JournalEntry> journalEntries;

        public Journal()
        {
            journalEntries = new List<JournalEntry>();
        }

        public override string ToString()
        {
            string list = null;
            for (int i = 0; i < journalEntries.Count; i++)
            {
                list += journalEntries[i];
            }
            return "Cобытия: \n" +
                list + "\n";
        }

        public void LecturerChanged(object sender, LecturersChangedEventArgs<TKey> e)
        {
            journalEntries.Add(new JournalEntry(e.CollectionName, e.Reason, e.SourceOfChange, e.EventKey.ToString()));
        }
    }

    public class LecturersChangedEventArgs<TKey>: EventArgs
    {
        public string CollectionName { get; set; }

        public Action Reason { get; set; }

        public string SourceOfChange { get; set; }

        public TKey EventKey { get; set; }

        public LecturersChangedEventArgs(string collectionName, Action reason, string source, TKey key)
        {
            this.CollectionName = collectionName;
            this.Reason = reason;
            this.SourceOfChange = source;
            this.EventKey = key;
        }

        public override string ToString()
        {
            return "Название коллекции:" + CollectionName +
                "\nПричина вызова события:" + Reason +
                "\nСвойство, являющееся источником изменений:" + SourceOfChange +
                "\nКлюч:" + EventKey + "\n\n";
        }
    }
     
    class Theme
    {
        public string Diptheme { get; set; }
        public bool Check { get; set; }

        public Theme(string theme, bool check)
        {
            this.Diptheme = theme;
            this.Check = check;
        }

        public Theme()
        {
            Diptheme = "Diplom theme";
            Check = false;
        }

        public override string ToString()
        {
            return "\nТема: " + Diptheme + "\nЗанята: " + Check;
        }
    }

    class Subject : Person
    {
        public string SbjName { get; set; }

        public string Specialty { get; set; }

        public int Hours { get; set; }

        public Subject(string SbjName, string Specialty, int Hours)
        {
            this.SbjName = SbjName;
            this.Specialty = Specialty;
            this.Hours = Hours;
        }

        public Subject()
        {
            SbjName = "Предмет";
            Specialty = "Специальность";
            Hours = 0;
        }

        public override string ToString()
        {
            return "\nНазвание предмета: " + SbjName + "\n" + "Специальность: " + Specialty + "\n" + "Количество часов: " + Hours;
        }

        public override object DeepCopy()
        {
            Subject subject = new Subject(SbjName, Specialty, Hours);
            return subject;
        }
    }

    class LecComparer : IComparer<Lecturer>
    {
        public int Compare(Lecturer l1, Lecturer l2)
        {
            if (l1 != null && l2 != null)
            {
                return l1.Rating.CompareTo(l2.Rating);
            }
            else
            {
                throw new ArgumentException("One of the params doesn't match\n");
            }
        }
    }

    class TestCollections
    {
        static List<Person> perlist = new List<Person>();
        static List<string> strlist = new List<string>();
        static Dictionary<Person, Lecturer> perlD = new Dictionary<Person, Lecturer>();
        static Dictionary<string, Lecturer> strlD = new Dictionary<string, Lecturer>();

        static Lecturer RefCreator(int i)
        {
            Lecturer l = new Lecturer();
            l.Surname += String.Format("l:{0}", i);
            return l;
        }

        public TestCollections(int count)
        {
            for (int i = 0; i <= count; i++)
            {
                Lecturer l = RefCreator(i);
                perlist.Add(l as Person);
                strlist.Add(l.ToString());
                perlD.Add(l as Person, l);
                strlD.Add(l.ToString(), l);
            }
        }

        public string SearchTime(int n)
        {
            string info = "";
            Lecturer l1 = RefCreator(n);
            Person p1 = l1 as Person;
            int t1 = Environment.TickCount;

            bool b = perlist.Contains(l1);
            int t2 = Environment.TickCount;
            info += string.Format("\n{0}, {1}\n", t2 - t1, b);

            t1 = Environment.TickCount;
            b = strlist.Contains(p1.ToString());
            t2 = Environment.TickCount;
            info += string.Format("\n{0}, {1}\n", t2 - t1, b);

            t1 = Environment.TickCount;
            b = perlD.ContainsKey(l1);
            t2 = Environment.TickCount;
            info += string.Format("\n{0}, {1}\n", t2 - t1, b);

            t1 = Environment.TickCount;
            b = strlD.ContainsKey(l1.ToString());
            t2 = Environment.TickCount;
            info += string.Format("\n{0}, {1}\n", t2 - t1, b);

            t1 = Environment.TickCount;
            b = strlD.ContainsValue(l1);
            t2 = Environment.TickCount;
            info += string.Format("\n{0}, {1}\n", t2 - t1, b);
            return info;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            int[] keys = { 100, 98, 46, 60, 90, 96 };
            
            //1
            LecturerCollection<string> c1 = new LecturerCollection<string>();
            LecturerCollection<string> c2 = new LecturerCollection<string>();
            c1.CollectionName = "collection1";
            c2.CollectionName = "collection2";

            //2
            Journal<string> journal = new Journal<string>();
            c1.LecturerChanged += journal.LecturerChanged;
            c2.LecturerChanged += journal.LecturerChanged;

            //3
            Lecturer lecturer1 = new Lecturer(new Person("Варя", "Ганьшина", new DateTime(1997, 10, 8)),
                                           "Кафедра математического обеспечения ЭВМ", Post.AssociateProfessor, keys[0]);
            Lecturer lecturer2 = new Lecturer(new Person("Саша", "Папирнык", new DateTime(2000, 10, 1)),
                                             "Кафедра вычислительной математики и математической кибернетики", Post.Professor, keys[1]);
            Lecturer lecturer3 = new Lecturer(new Person(),
                                          "Кафедра компьютерных технологий", Post.Professor, keys[2]);
            Lecturer lecturer4 = new Lecturer(new Person("Марк", "Мурашов", new DateTime(1999, 4, 30)),
                                          "Кафедра математического обеспечения ЭВМ", Post.Assistant, keys[3]);
            Lecturer lecturer5 = new Lecturer(new Person("Артем", "Косогов", new DateTime(2000, 5, 26)),
                                          "Кафедра компьютерных технологий", Post.Assistant, keys[4]);

            c1.AddLecturer(keys[0].ToString(), lecturer1);
            c1.AddLecturer(keys[1].ToString(), lecturer2);
            c1.AddLecturer(keys[2].ToString(), lecturer3);
            c1.AddLecturer(keys[3].ToString(), lecturer4);
            c1.AddLecturer(keys[4].ToString(), lecturer5);

            c2.AddLecturer("21", new Lecturer());

            c1.Remove(lecturer2);

            lecturer2.Department = "Kafedrarararara";
            lecturer1.Department = "Кафедраааааа";
            lecturer5.Lpost = Post.AssociateProfessor;

            //4
            Console.WriteLine(journal);

            List<Lecturer> lecturers = new List<Lecturer>();
            string res = null;

            //5
            lecturers = c1.SoringDictionary();

            for (int i = 0; i < lecturers.Count; i++)
            {
                res += lecturers[i].ToShortString() + $"\nKey:{lecturers[i].Rating}\n\n";
            }

            Console.WriteLine(res);
            Console.ReadLine();

        }
    }
}
