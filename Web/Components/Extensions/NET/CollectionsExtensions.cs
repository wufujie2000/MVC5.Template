using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Components.Extensions.Net
{
    public static class CollectionsExtensions
    {
        #region ICollection<T>

        public static Boolean IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }
        public static Boolean IsEquivalentTo<T>(this ICollection<T> collection, ICollection<T> equivalence)
        {
            return collection.Count == equivalence.Count && new HashSet<T>(collection).SetEquals(equivalence);
        }
        public static Boolean IsEquivalentTo<T>(this ICollection<List<T>> collection, ICollection<List<T>> equivalence)
        {
            if (collection.Count != equivalence.Count) return false;

            var tempEquivalence = equivalence.ToList();
            foreach (var list in collection)
            {
                var listIndex = tempEquivalence
                    .FindIndex(equivalanceList => list.IsEquivalentTo(equivalanceList));

                if (listIndex == -1) return false;
                tempEquivalence.RemoveAt(listIndex);
            }

            return true;
        }

        #endregion

        #region IList<T>

        public static IEnumerable<List<T>> PowerSet<T>(this IList<T> list)
        {
            Int32 sets = 1 << list.Count;
            for (Int32 set = 0; set < sets; ++set)
            {
                List<T> subSet = new List<T>();
                for (Int32 bits = set, item = 0; bits != 0; bits >>= 1, ++item)
                    if ((bits & 1) != 0)
                        subSet.Add(list[item]);

                yield return subSet;
            }
        }

        #endregion

        #region LinkedList<T>

        public static Int32 HeadLength<T>(this LinkedListNode<T> node)
        {
            Int32 headLength = 0;
            LinkedListNode<T> iterator = node;

            while (iterator.Previous != null)
            {
                iterator = iterator.Previous;
                headLength++;
            }

            return headLength;
        }
        public static Int32 TailLength<T>(this LinkedListNode<T> node)
        {
            Int32 tailLength = 0;
            LinkedListNode<T> iterator = node;

            while (iterator.Next != null)
            {
                iterator = iterator.Next;
                tailLength++;
            }

            return tailLength;
        }
        public static List<T> LeftSide<T>(this LinkedListNode<T> node)
        {
            List<T> leftSide = new List<T>();
            LinkedListNode<T> current = node.List.First;

            while (current != node)
            {
                leftSide.Add(current.Value);
                current = current.Next;
            }

            return leftSide;
        }
        public static List<T> RightSide<T>(this LinkedListNode<T> node)
        {
            List<T> rightSide = new List<T>();
            LinkedListNode<T> current = node;

            while (current.Next != null)
            {
                rightSide.Add(current.Next.Value);
                current = current.Next;
            }

            return rightSide;
        }

        #endregion
    }
}
