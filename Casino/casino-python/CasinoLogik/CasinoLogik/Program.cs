using System;

namespace CasinoLogik
{
    // Deine Würfel-Logik als Konsolen-Programm für die Webseite.
    // Aufruf:  CasinoLogik.exe <versuche>      (1, 2 oder 3)
    // Ausgabe: genau EINE Zeile, z. B.
    // {"ergebnis":[["*","~","#"],["x","x","x"]],"gewonnen":[false,true]}
    //
    // Wichtig: Hier darf es kein Console.ReadLine(), kein Console.ReadKey()
    // und keine anderen Console.WriteLine-Ausgaben geben. Python liest nur die letzte Zeile.
    internal class Program
    {
        static void Main(string[] args)
        {
            /*------------------------------------------------------------VARIABLEN---------------------------------------------------------*/

            Random rng = new Random();
            char a = '*', b = '~', c = '#', x = 'x';

            int versuche;                                   // kommt jetzt aus args[0] statt aus ReadLine
            int versuch1 = 0, versuch2 = 0, versuch3 = 0;   // 1 = gewonnen, 0 = verloren
            int zufall1, zufall2, zufall3;

            char[] ergebnis = new char[3], ergebnis2 = new char[3], ergebnis3 = new char[3];

            /*-------------------------------------------------------------EINGABE----------------------------------------------------------*/

            // Statt Console.ReadLine(): Versuchszahl aus dem Aufruf lesen und prüfen
            if (args.Length < 1 || !int.TryParse(args[0], out versuche) || versuche < 1 || versuche > 3)
            {
                Console.WriteLine("{\"fehler\":\"Ungueltige Versuchszahl\"}");
                return;
            }

            /*-----------------------------------------------------------WÜRFELN------------------------------------------------------------*/

            switch (versuche)
            {
                case 1: // ---------- Ablauf bei 1 Versuch ----------
                    for (int v1 = 0; v1 < 3; v1++)
                    {
                        zufall1 = rng.Next(0, 4);
                        if (zufall1 == 1)
                        {
                            ergebnis[v1] = a;
                        }
                        else
                        {
                            if (zufall1 == 2)
                            {
                                ergebnis[v1] = b;
                            }
                            else
                            {
                                if (zufall1 == 3)
                                {
                                    ergebnis[v1] = c;
                                }
                                else
                                {
                                    ergebnis[v1] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                    {
                        versuch1 = 1;
                    }
                    else
                    {
                        versuch1 = 0;
                    }
                    break;

                case 2: // ---------- Ablauf bei 2 Versuchen ----------
                    for (int v1 = 0; v1 < 3; v1++)
                    {
                        zufall1 = rng.Next(0, 4);
                        if (zufall1 == 1)
                        {
                            ergebnis[v1] = a;
                        }
                        else
                        {
                            if (zufall1 == 2)
                            {
                                ergebnis[v1] = b;
                            }
                            else
                            {
                                if (zufall1 == 3)
                                {
                                    ergebnis[v1] = c;
                                }
                                else
                                {
                                    ergebnis[v1] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                    {
                        versuch1 = 1;
                    }
                    else
                    {
                        versuch1 = 0;
                    }
                    for (int v2 = 0; v2 < 3; v2++)
                    {
                        zufall2 = rng.Next(0, 4);
                        if (zufall2 == 1)
                        {
                            ergebnis2[v2] = a;
                        }
                        else
                        {
                            if (zufall2 == 2)
                            {
                                ergebnis2[v2] = b;
                            }
                            else
                            {
                                if (zufall2 == 3)
                                {
                                    ergebnis2[v2] = c;
                                }
                                else
                                {
                                    ergebnis2[v2] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis2[0] == ergebnis2[1] && ergebnis2[1] == ergebnis2[2] && ergebnis2[0] != x)
                    {
                        versuch2 = 1;
                    }
                    else
                    {
                        versuch2 = 0;
                    }
                    break;

                case 3: // ---------- Ablauf bei 3 Versuchen ----------
                    for (int v1 = 0; v1 < 3; v1++)
                    {
                        zufall1 = rng.Next(0, 4);
                        if (zufall1 == 1)
                        {
                            ergebnis[v1] = a;
                        }
                        else
                        {
                            if (zufall1 == 2)
                            {
                                ergebnis[v1] = b;
                            }
                            else
                            {
                                if (zufall1 == 3)
                                {
                                    ergebnis[v1] = c;
                                }
                                else
                                {
                                    ergebnis[v1] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                    {
                        versuch1 = 1;
                    }
                    else
                    {
                        versuch1 = 0;
                    }
                    for (int v2 = 0; v2 < 3; v2++)
                    {
                        zufall2 = rng.Next(0, 4);
                        if (zufall2 == 1)
                        {
                            ergebnis2[v2] = a;
                        }
                        else
                        {
                            if (zufall2 == 2)
                            {
                                ergebnis2[v2] = b;
                            }
                            else
                            {
                                if (zufall2 == 3)
                                {
                                    ergebnis2[v2] = c;
                                }
                                else
                                {
                                    ergebnis2[v2] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis2[0] == ergebnis2[1] && ergebnis2[1] == ergebnis2[2] && ergebnis2[0] != x)
                    {
                        versuch2 = 1;
                    }
                    else
                    {
                        versuch2 = 0;
                    }
                    for (int v3 = 0; v3 < 3; v3++)
                    {
                        zufall3 = rng.Next(0, 4);
                        if (zufall3 == 1)
                        {
                            ergebnis3[v3] = a;
                        }
                        else
                        {
                            if (zufall3 == 2)
                            {
                                ergebnis3[v3] = b;
                            }
                            else
                            {
                                if (zufall3 == 3)
                                {
                                    ergebnis3[v3] = c;
                                }
                                else
                                {
                                    ergebnis3[v3] = x;
                                }
                            }
                        }
                    }
                    if (ergebnis3[0] == ergebnis3[1] && ergebnis3[1] == ergebnis3[2] && ergebnis3[0] != x)
                    {
                        versuch3 = 1;
                    }
                    else
                    {
                        versuch3 = 0;
                    }
                    break;

                default:
                    break;      // kann nicht vorkommen, die Versuchszahl wurde oben geprüft
            }

            /*-----------------------------------------------------------AUSGABE------------------------------------------------------------*/

            // Statt Tabelle und ASCII-Art: alles als eine JSON-Zeile für Python
            string json = "{\"ergebnis\":[[\"" + ergebnis[0] + "\",\"" + ergebnis[1] + "\",\"" + ergebnis[2] + "\"]";
            if (versuche >= 2)
            {
                json += ",[\"" + ergebnis2[0] + "\",\"" + ergebnis2[1] + "\",\"" + ergebnis2[2] + "\"]";
            }
            if (versuche >= 3)
            {
                json += ",[\"" + ergebnis3[0] + "\",\"" + ergebnis3[1] + "\",\"" + ergebnis3[2] + "\"]";
            }

            json += "],\"gewonnen\":[" + (versuch1 == 1 ? "true" : "false");
            if (versuche >= 2)
            {
                json += "," + (versuch2 == 1 ? "true" : "false");
            }
            if (versuche >= 3)
            {
                json += "," + (versuch3 == 1 ? "true" : "false");
            }
            json += "]}";

            Console.WriteLine(json);
        }
    }
}