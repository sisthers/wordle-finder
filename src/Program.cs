using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordleFinder
{
    class Program
    {
        public static int lenght;

        public static List<wordWeight> wordList = new List<wordWeight>();
        public class anyLetter
        {
            public char letterName;
        }
        public class letterForMask : anyLetter
        {
            public int amount = -1;

            public bool ifFoundAll = false;
            public letterForMask(char let)
            {
                letterName = let;
            }
        }
        public class letterForSearch : anyLetter
        {
            public int index;
            public bool ifMissed;
            public letterForSearch(char let, int index, bool ifMis)
            {
                ifMissed = ifMis;
                letterName = let;
                this.index = index;
            }
        }

        public class word
        {
            protected List<char> lettersList = new List<char>();

            public string name;
            public word()
            {
                lettersList.Add('о');
                lettersList.Add('е');
                lettersList.Add('а');
                lettersList.Add('и');
                lettersList.Add('н');
                lettersList.Add('т');
                lettersList.Add('с');
                lettersList.Add('р');
                lettersList.Add('в');
                lettersList.Add('л');
                lettersList.Add('к');
                lettersList.Add('м');
                lettersList.Add('д');
                lettersList.Add('п');
                lettersList.Add('у');
                lettersList.Add('я');
                lettersList.Add('ы');
                lettersList.Add('ь');
                lettersList.Add('г');
                lettersList.Add('з');
                lettersList.Add('б');
                lettersList.Add('ч');
                lettersList.Add('й');
                lettersList.Add('х');
                lettersList.Add('ж');
                lettersList.Add('ш');
                lettersList.Add('ю');
                lettersList.Add('ц');
                lettersList.Add('щ');
                lettersList.Add('э');
                lettersList.Add('ф');
                lettersList.Add('ъ');

            }

        }
        public class wordWeight : word
        {
            public int weight;
            public wordWeight(string wordName)
            {
                name = wordName;
                char[] wC = wordName.ToCharArray();
                for (int i = 0; i < lenght; i++)
                    weight += lettersList.FindIndex(x => x == wC[i]);
            }

        }

        public class mask : word
        {
            private string indexesOfMissedLetters;

            private char[] newMaskForSearch = new char[lenght];

            public List<List<char>> lettersWhichCantBe = new List<List<char>>();

            public List<letterForMask> letters = new List<letterForMask>();
            public mask()
            {
                for (int i = 0; i < lenght; i++)
                {
                    lettersWhichCantBe.Add(new List<char>());
                    newMaskForSearch[i] = '*';
                }
                lettersList = lettersList.OrderBy(x => x).ToList();
                for (int i = 0; i < lettersList.Count(); i++)
                    letters.Add(new letterForMask(lettersList[i]));
            }

            public string findWord(string name, string indexesOf)
            {
                string ind = "";
                char[] c = indexesOf.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    int x = int.Parse(c[i].ToString()) - 1;
                    ind += x.ToString();
                }
                this.indexesOfMissedLetters = ind;

                this.name = name;
                List<List<letterForSearch>> listOfLettersLists = new List<List<letterForSearch>>();

                for (int i = 0; i < lettersList.Count(); i++)
                {
                    if (name.ToLower().Contains(lettersList[i]))
                    {
                        listOfLettersLists.Add(new List<letterForSearch>());
                        for (int x = 0; x < lenght; x++)
                        {
                            if (Char.ToLower(name[x]) == lettersList[i])
                            {
                                bool ifMis = false;
                                if (indexesOfMissedLetters.Contains(x.ToString()))
                                    ifMis = true;
                                listOfLettersLists.Last().Add(new letterForSearch(name[x], x, ifMis));
                            }
                        }
                    }
                }


                for (int i = 0; i < listOfLettersLists.Count(); i++)
                {

                    List<letterForSearch> letterForSearches = listOfLettersLists[i];


                    if (letterForSearches.Any(x => x.letterName == Char.ToUpper(letterForSearches[0].letterName)))
                    {
                        if (letterForSearches.All(x => x.letterName == Char.ToUpper(letterForSearches[0].letterName)))
                        {
                            letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count();
                            for (int x = 0; x < letterForSearches.Count(); x++)
                            {
                                newMaskForSearch[letterForSearches[x].index] = letterForSearches[x].letterName;
                            }
                        }
                        else if (letterForSearches.Any(x => x.ifMissed == true))
                        {
                            if (letterForSearches.Where(x => x.letterName != Char.ToUpper(letterForSearches[0].letterName)).All(x => x.ifMissed == true))
                            {
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).ifFoundAll = true;
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count(x => x.letterName == Char.ToUpper(letterForSearches[0].letterName));
                                for (int x = 0; x < letterForSearches.Count(); x++)
                                {
                                    if (letterForSearches[x].letterName == Char.ToUpper(letterForSearches[0].letterName))
                                        newMaskForSearch[letterForSearches[x].index] = letterForSearches[x].letterName;
                                }

                            }
                            else
                            {
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count(x => x.ifMissed == false);
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).ifFoundAll = true;
                                for (int x = 0; x < letterForSearches.Count(); x++)
                                {
                                    if (letterForSearches[x].letterName == Char.ToUpper(letterForSearches[0].letterName))
                                        newMaskForSearch[letterForSearches[x].index] = letterForSearches[x].letterName;
                                    else if (letterForSearches[x].ifMissed == false)
                                        lettersWhichCantBe[letterForSearches[x].index].Add(letterForSearches[x].letterName);
                                }

                            }
                        }
                        else
                        {
                            letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count();
                            for (int x = 0; x < letterForSearches.Count(); x++)
                            {
                                if (letterForSearches[x].letterName == Char.ToUpper(letterForSearches[0].letterName))
                                    newMaskForSearch[letterForSearches[x].index] = letterForSearches[x].letterName;
                                else
                                    lettersWhichCantBe[letterForSearches[x].index].Add(letterForSearches[x].letterName);
                            }

                        }
                    }
                    else
                    {
                        if (letterForSearches.Any(x => x.ifMissed == true))
                        {
                            if (letterForSearches.All(x => x.ifMissed == true))
                            {
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = 0;
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).ifFoundAll = true;
                            }
                            else
                            {
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count(x => x.letterName != Char.ToUpper(letterForSearches[0].letterName) && x.ifMissed == false);
                                letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).ifFoundAll = true;
                                for (int y = 0; y < letterForSearches.Count(); y++)
                                {
                                    if (letterForSearches[y].ifMissed == false)
                                        lettersWhichCantBe[letterForSearches[y].index].Add(letterForSearches[y].letterName);
                                }
                            }
                        }
                        else
                        {
                            letters.Find(x => x.letterName == Char.ToLower(letterForSearches[0].letterName)).amount = letterForSearches.Count();
                            for (int y = 0; y < letterForSearches.Count(); y++)
                            {
                                lettersWhichCantBe[letterForSearches[y].index].Add(letterForSearches[y].letterName);
                            }
                        }

                    }

                }


                for (int i = 0; i < wordList.Count(); i++)
                {


                    bool ok = true;
                    string wordFinding = wordList[i].name;

                    for (int x = 0; x < letters.Count(); x++)
                    {
                        letterForMask letter = letters[x];
                        if (letter.amount == -1)
                            continue;
                        else if (letter.ifFoundAll == true && wordFinding.Count(x => x == letter.letterName) == letter.amount)
                            continue;
                        else if (letter.ifFoundAll == false && wordFinding.Count(x => x == letter.letterName) >= letter.amount)
                            continue;
                        else
                        {
                            ok = false;
                            goto Found;
                        }

                    }
                    for (int x = 0; x < lenght; x++)
                    {
                        if (newMaskForSearch[x] == '*' && !lettersWhichCantBe[x].Contains(wordFinding[x]))
                            continue;
                        else if (Char.IsUpper(newMaskForSearch[x]) && Char.ToLower(newMaskForSearch[x]) == wordFinding[x])
                            continue;
                        else
                        {
                            ok = false;
                            goto Found;
                        }

                    }
                Found: if (ok)
                        return wordFinding;

                }

                return "Нужное слово не найдено";
            }
        }

        public static void Main()
        {
            Console.WriteLine("Введите длину слов");
            lenght = int.Parse(Console.ReadLine());
            using (StreamReader streamReader = new StreamReader("1.txt"))
            {
                do
                {
                    string str = streamReader.ReadLine().ToLower().Replace('ё', 'е');
                    if (str.Length == lenght)
                        wordList.Add(new wordWeight(str));
                } while (!streamReader.EndOfStream);
            }
            wordList = wordList.OrderBy(x => x.weight).ToList();
            while (true)
            {
                Console.WriteLine("Введите режим:\n1. Сначала\n2. С середины");
                int gameType = int.Parse(Console.ReadLine());
                if (gameType == 2)
                {
                    string ans = "";
                    mask newMask = new mask();
                    while (true)
                    {
                        Console.WriteLine("Введите полученную маску(буквы, стоящие на нужном месте, писать с заглавной буквы)");
                        string maskW = Console.ReadLine();
                        if (maskW.ToLower() == "ищи")
                            break;
                        Console.WriteLine("Введите цифры мест, где буквы не оказалось");
                        string missingLetters = Console.ReadLine();
                        ans = newMask.findWord(maskW, missingLetters);
                    }
                    while (true)
                    {
                        Console.WriteLine("Попробуйте ввести слово " + ans);
                        Console.WriteLine("Введите полученную маску(буквы, стоящие на нужном месте, писать с заглавной буквы)");
                        string word = Console.ReadLine();
                        if (word.ToLower() == "нашел")
                            break;
                        Console.WriteLine("Введите цифры мест, где буквы не оказалось");
                        string missingLetters = Console.ReadLine();
                        ans = newMask.findWord(word, missingLetters);

                    }

                }
                else if (gameType == 1)
                {
                    Console.WriteLine($"Введите в игре слово {wordList.First(x => x.name.Distinct().Count() == x.name.Count()).name}");
                    mask newMask = new mask();
                    while (true)
                    {
                        Console.WriteLine("Введите полученную маску(буквы, стоящие на нужном месте, писать с заглавной буквы)");
                        string word = Console.ReadLine();
                        if (word.ToLower() == "нашел")
                            break;
                        Console.WriteLine("Введите цифры мест, где буквы не оказалось");
                        string missingLetters = Console.ReadLine();
                        Console.Write("Попробуйте ввести слово ");
                        Console.WriteLine(newMask.findWord(word, missingLetters));
                    }
                }
            }
        }

    }

}