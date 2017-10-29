using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task3
{
    class Program
    {
        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
                word = word.Substring(0, apostropheLocation);
            return word;
        }

        static Dictionary<string, int> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b", RegexOptions.IgnoreCase);

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value).ToLower();
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (result.ContainsKey(word))
                    result[word]++;
                else
                    result.Add(word, 1);
            }
            return result;
        }
        
        static void Main(string[] args)
        {
            var words = GetWords("C#[b] (pronounced as see sharp) is a multi-paradigm programming language encompassing strong typing, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.");
            foreach(var item in words)
                Console.WriteLine("{0} {1}", item.Key, item.Value);
            Console.ReadKey();
        }
    }
}
