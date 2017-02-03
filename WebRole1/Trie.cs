using System;
using System.Collections.Generic;
using System.Text;

namespace WebRole1
{
    /**
     * A space-saving data structure to store words and their counts.
     * Words that share prefixes will have common paths along a branch of the tree.
     */
    public class Trie
    {
        private TrieNode head { get; set; }
        private TrieNode curr { get; set; }
        private List<string> searchResults { set; get; }

        public Trie()
        {
            head = new TrieNode();
            curr = head;
            searchResults = new List<string>();
        }

        public List<string> Search(string input)
        {
            curr = head;
            string prefix = Prefix(input);
            searchResults.Clear();
            GetChildren(curr, prefix, input);
            return searchResults;
        }

        private string Prefix(string input)
        {
            StringBuilder temp = new StringBuilder();
            TrieNode child = null;
            int index = 0;
            foreach (var c in input)
            {
                if (index < input.Length - 1)
                    temp.Append(c);

                child = curr.GetChild(c);

                if (child == null)
                    break;

                curr = child;
                index++;
            }
            return temp.ToString();
        }

        private void GetChildren(TrieNode node, string subString, string input)
        {
            if (node == null || searchResults.Count > 10)
                return;

            subString = subString + node.Data;

            if (node.isLeaf() && subString.ToLower().StartsWith(input.ToLower())) 
                searchResults.Add(subString);

            foreach (var child in node.Children)
                GetChildren(child, subString, input);

        }

        /**
         * Add a word to the trie.
         * Adding is O (|A| * |W|), where A is the alphabet and W is the word being searched.
         */
        public void AddWord(string word)
        {
            TrieNode curr = head;
            curr = curr.GetChild(word[0], true);
            for (int i = 1; i < word.Length; i++)
            {
                curr = curr.GetChild(word[i], true);
            }
            curr.AddCount();
        }

        /**
         * Get the count of a partictlar word.
         * Retrieval is O (|A| * |W|), where A is the alphabet and W is the word being searched.
         */
        public int GetCount(string word)
        {
            TrieNode curr = head;
            foreach (char c in word)
            {
                curr = curr.GetChild(c);
                if (curr == null)
                {
                    return 0;
                }
            }
            return curr.Count;
        }

        internal class TrieNode
        {
            public LinkedList<TrieNode> Children { private set; get; }
            public int Count { private set; get; }
            public char Data { private set; get; }

            public TrieNode(char data = ' ')
            {
                this.Data = data;
                Count = 0;
                Children = new LinkedList<TrieNode>();
            }

            public bool isLeaf()
            {
                return Count > 0;
            }

            public TrieNode GetChild(char c, bool createIfNotExist = false)
            {
                foreach (var child in Children)
                    if (child.Data == c)
                        return child;
 
                if (createIfNotExist)
                    return CreateChild(c);

                return null;
            }

            public void AddCount()
            {
                Count++;
            }

            public TrieNode CreateChild(char c)
            {
                var child = new TrieNode(c);
                Children.AddLast(child);
                return child;
            }
        }
    }
}