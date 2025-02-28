using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureProject.Classes
{
    public class GenircClass<t> : IList<t>, ICollection<t>, IEnumerable<t>
    {
        #region Fileds
        t[] Items;
        int CurrentIndex = -1;
        int Capacity = 4;
        int Count { get; set; }
        int ICollection<t>.Count => throw new NotImplementedException();
        public bool IsReadOnly => throw new NotImplementedException();
        IEqualityComparer<t> comarer;
        Stack<t> Stack;
        #endregion

        #region constructors and indexers
        public GenircClass()
        {
            Items = new t[Capacity];
            Count = 0;
            CurrentIndex = -1;
            comarer = EqualityComparer<t>.Default;
            Stack = new Stack<t>();
        }
        //constructor with custom comprer
        public GenircClass(IEqualityComparer<t> comperar)
        {
            Items = new t[Capacity];
            Count = 0;
            CurrentIndex = -1;
            comarer = comperar;
            Stack = new Stack<t>();
        }
        // constructor with non default capacity
        public GenircClass(int capacity)
        {
            Items = new t[capacity];
            Count = 0;
            CurrentIndex = -1;
            comarer = EqualityComparer<t>.Default;
            Stack = new Stack<t>();
        }
        public GenircClass(IEnumerable<t> collection)
        {
            int ncount = GetEnmurableCount(collection);
            Items = new t[ncount];
            foreach (t item in collection)
                Add(item);
            comarer = EqualityComparer<t>.Default;
            Stack = new Stack<t>();
        }
        //indexer return a value
        public t this[int Index]
        {
            get
            {
                if (Index < 0 || Index > Items.Length)
                    throw new IndexOutOfRangeException();
                return Items[Index];
            }
            set
            {
                if (Index < 0 || Index > Items.Length)
                    throw new IndexOutOfRangeException();
                Items[Index] = value;
            }
        }

        // indexer return a list of values
        public List<t> this[string m]
        {
            get
            {
                List<t> l = new List<t>();
                string[] indexes = m.Split(',');
                for (int i = 0; i < indexes.Length; i++)
                {
                    l.Add(Items[Convert.ToInt32(indexes[i])]);
                }
                return l;
            }
        }

        #endregion

        #region Add Methods
        //add element
        public void Add(t value)
        {
            //check if the list is full
            if (CurrentIndex == Items.Length - 1)
            {
                ReSize(Items.Length * 2);
            }

            Items[++CurrentIndex] = value;
            Count++;
        }
        //add range
        public void AddRange(IEnumerable<t> useritems)
        {
            //get length of the user items
            int length = GetEnmurableCount(useritems);
            //check if thar is space in list
            if ((Count + length) > Items.Length)
                ReSize(Count + length);
            foreach (t item in useritems)
            {
                Items[++CurrentIndex] = item;
                Count++;
            }
        }
        //insert element at index 
        public void Insert(int index, t useritem)
        {
            //validat the input
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();
            if (Count == Items.Length)
                ReSize(Items.Length * 2);
            //shift right
            ShiftRight(index);
            Items[index] = useritem;
            Count++;
            CurrentIndex++;
        }
        //insert range at
        public void InsertRange(int index, IEnumerable<t> useritems)
        {
            int Length= GetEnmurableCount(useritems);
            //validat input
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();
            // resize if needed
            if (Count + Length > Items.Length)
                ReSize(Count + Length);
            int countr = index;
            //Adding items
            foreach (t item in useritems)
            {
                //shift right
                ShiftRight(countr);
                //assign the knew values
                Items[countr++] = item;
                //increment count and current index
                CurrentIndex++;
                Count++;
            }

        }
        #endregion
       
        #region Remove Methods
        // remove by item method
        public bool Remove(t item)
        {
            ShiftLeft(IndexOf(item));
            return true;
        }
        // remove by index method
        public void RemoveAt(int index)
        {
            //validat input
            if (index < 0 || index > CurrentIndex)
                throw new IndexOutOfRangeException();
            //shift left the values
            ShiftLeft(index);

        }
        // remove range
        public void RemoveRange(int range, int index)
        {
            //validat input
            if (range <= 0 || index < 0 || index + range > Items.Length || index > Count)
                throw new IndexOutOfRangeException();
            for (int i = index + range; i >= index; i++)
            {
                ShiftLeft(i);
            }
            // optimize 
            if (Items.Length / 2 >= CurrentIndex)
                ReSize(Items.Length / 2);
        }
        // clear the array
        public void Clear()
        {
            Items = new t[4];
            Count = 0;
            CurrentIndex = -1;
        }
        //Remove all
        public void RemovAll()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = default(t);
            }
        }
        #endregion

        #region Other Methods
        //indexof method
        public int IndexOf(t item)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (comarer.Equals(Items[i], item))
                    return i;
            }
            return -1;
        }
        //revarse 
        public void Revers()
        {
            t[] Temp = new t[Count];
            for (int i = 0; i < Count; i++)
            {
                Temp[i] = Items[Count - i - 1];
            }
            Items = Temp;
        }
        //sort
        public void Sort()
        {
            Array.Sort(Items);
        }
        //enable foreach loop 
        public IEnumerator<t> GetEnumerator()
        {
            foreach (t item in Items)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public int GetEnmurableCount(IEnumerable collection)
        {
            int ncount = 0;
            foreach (t item in collection) ncount++;
            return ncount;
        }
        //check element in list
        public bool Contains(t item)
        {
            return IndexOf(item) >= 0;
        }
        public void CopyTo(t[] array, int arrayIndex)
        {
            Array.Copy(Items, 0, array, arrayIndex, Count);
        }
        public void UnDo()
        {
            t item = Stack.Pop();
            Add(item);
        }
        public void UnDoAll()
        {
            foreach (t item in Stack)
            {
                Add(item);
                Stack.Pop();
            }
        }
        #endregion
        #region HelperMethods
        // shift left methode
        void ShiftLeft(int index)
        {
            Stack.Push(Items[index]);
            for (int i = index; i < Items.Length - 1; i++)
            {
                if (Items[i] == null)
                    break;
                Items[i] = Items[i + 1];
            }
            if (Items[Items.Length - 1] != null)
                Items[index] = default(t);
            CurrentIndex--;
            Count--;
        }
        public void ShiftRight(int index)
        {
            for (int i = CurrentIndex; i >= index; i--)
            {
                Items[i + 1] = Items[i];
            }
        }
        //resize
        public void ReSize(int capacity)
        {
            t[] NewItems = new t[capacity];
            Array.Copy(Items, NewItems, Items.Length);
            Items = NewItems;
        } 
        #endregion
    }
}
