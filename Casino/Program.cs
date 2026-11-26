using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*------------------------------------------------------------VARIABLEN---------------------------------------------------------*/


            Random rng = new Random();                      // Zufallsgenerator (nur EINMAL anlegen)

            // Die Symbole der Walzen. 'x' ist die "Niete", wenn die Zufallszahl keinem Symbol entspricht
            char a = '*', b = '~', c = '#', x = 'x';

            int versuche;                                   // Anzahl Versuche, die der Benutzer eingibt (1-3)
            int versuch1 = 0, versuch2 = 0, versuch3 = 0;   // Merker pro Versuch: 1 = gewonnen, 0 = verloren
            int zufall1, zufall2, zufall3;                  // Zufallszahl pro Versuch (wird in der Schleife gewürfelt)

            double einsatz;                                 // Einsatz, den der Benutzer eingibt
            double guthaben;                          // Startguthaben auf dem Konto

            // Pro Versuch ein eigenes Array mit 3 Plätzen (= 3 Zeilen/Symbole).
            // So überschreibt Versuch 2 nicht die Symbole von Versuch 1.
            char[] ergebnis = new char[3], ergebnis2 = new char[3], ergebnis3 = new char[3];

            // Große Schrift für Gewinn / Verlust (@ = mehrzeiliger Text)
            string jackpot = @"
   ██████  █████   ██████  ██   ██ ██████   █████  ████████ 
       ██ ██   ██ ██       ██  ██  ██   ██ ██   ██    ██    
       ██ ███████ ██       █████   ██████  ██   ██    ██    
  ██   ██ ██   ██ ██       ██  ██  ██      ██   ██    ██    
   █████  ██   ██  ██████  ██   ██ ██       █████     ██    
                                                            ";
            string loser = @"
 ██       ██████  ███████ ███████ ██████  
 ██      ██    ██ ██      ██      ██   ██ 
 ██      ██    ██ ███████ █████   ██████  
 ██      ██    ██      ██ ██      ██   ██ 
 ███████  ██████  ███████ ███████ ██   ██ 
                                            ";
            string nichtGueltig = @"
  ███    ██ ██  ██████ ██   ██ ████████      ██████  ██    ██ ███████ ██      ████████ ██  ██████  
  ████   ██ ██ ██      ██   ██    ██        ██       ██    ██ ██      ██         ██    ██ ██       
  ██ ██  ██ ██ ██      ███████    ██        ██   ███ ██    ██ █████   ██         ██    ██ ██   ███ 
  ██  ██ ██ ██ ██      ██   ██    ██        ██    ██ ██    ██ ██      ██         ██    ██ ██    ██ 
  ██   ████ ██  ██████ ██   ██    ██         ██████   ██████  ███████ ███████    ██    ██  ██████  
";
            string willkommenText = @"
  ██     ██ ██ ██      ██      ██   ██  ██████  ███    ███ ███    ███ ███████ ███    ██ 
  ██     ██ ██ ██      ██      ██  ██  ██    ██ ████  ████ ████  ████ ██      ████   ██ 
  ██  █  ██ ██ ██      ██      █████   ██    ██ ██ ████ ██ ██ ████ ██ █████   ██ ██  ██ 
  ██ ███ ██ ██ ██      ██      ██  ██  ██    ██ ██  ██  ██ ██  ██  ██ ██      ██  ██ ██ 
   ███ ███  ██ ███████ ███████ ██   ██  ██████  ██      ██ ██      ██ ███████ ██   ████ 

  ██ ███    ███      ██████  █████  ███████ ██ ███    ██  ██████  
  ██ ████  ████     ██      ██   ██ ██      ██ ████   ██ ██    ██ 
  ██ ██ ████ ██     ██      ███████ ███████ ██ ██ ██  ██ ██    ██ 
  ██ ██  ██  ██     ██      ██   ██      ██ ██ ██  ██ ██ ██    ██ 
  ██ ██      ██      ██████ ██   ██ ███████ ██ ██   ████  ██████  
";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            


            /*-------------------------------------------------------------EINGABE----------------------------------------------------------*/



            while (true)
            {



                guthaben = 500;
                Console.WriteLine(willkommenText +
                    "");
                do
                {
                    Console.WriteLine("\nKonto: {0} Euro", guthaben);

                    Console.WriteLine("Wie viele Versuche wollen sie nehmen");
                    versuche = Convert.ToInt32(Console.ReadLine());         // Text einlesen und in Zahl umwandeln

                    Console.WriteLine("Ihr Einsatz bitte ein geben:");
                    einsatz = Convert.ToDouble(Console.ReadLine());         // Einsatz einlesen
                    if (einsatz <= 0 || einsatz > guthaben)
                    {
                        Console.WriteLine("Ihr Einatz ({0} Euro) ist nicht gültig", einsatz);
                        Console.WriteLine("Bitte drücken sie eine Taste ...");
                        Console.ReadKey();
                    }
                    else
                    {
                        guthaben = guthaben - einsatz;                       // Einsatz wird sofort vom Konto abgezogen
                        versuch1 = 0;
                        versuch2 = 0;
                        versuch3 = 0;

                        Console.WriteLine("Konto: {0} Euro", guthaben);
                        Console.Write("\n\n");

                        /*-------------------------------------------------------------TABELLE----------------------------------------------------------*/

                        // Kopfzeile der Tabelle (Write = bleibt in derselben Zeile)
                        Console.Write("\tVersuch 1");   // Spalte 1
                        Console.Write("\tVersuch 2");   // Spalte 2
                        Console.Write("\tVersuch 3");   // Spalte 3
                        Console.WriteLine();            // Zeilenumbruch nach der Kopfzeile

                        /*-----------------------------------------------------------WÜRFELN------------------------------------------------------------*/

                        // Je nach Anzahl der Versuche wird ein anderer Fall ausgeführt.
                        // Es läuft immer nur EIN Fall, deshalb steht in Fall 2 und 3 der Code für Versuch 1 nochmal.
                        switch (versuche)
                        {
                            case 1: // ---------- Ablauf bei 1 Versuch ----------

                                // Schleife läuft 3x: pro Durchlauf wird ein Symbol gewürfelt (3 Zeilen)
                                for (int v1 = 0; v1 < 3; v1++)
                                {
                                    zufall1 = rng.Next(0, 4);

                                    // Zufallszahl in ein Symbol umwandeln und im Array speichern
                                    if (zufall1 == 1)
                                    {
                                        ergebnis[v1] = a;           // 1 -> *
                                    }
                                    else
                                    {
                                        if (zufall1 == 2)
                                        {
                                            ergebnis[v1] = b;       // 2 -> ~
                                        }
                                        else
                                        {
                                            if (zufall1 == 3)
                                            {
                                                ergebnis[v1] = c;   // 3 -> #
                                            }
                                            else
                                            {
                                                ergebnis[v1] = x;   // sonst (0) -> x
                                            }
                                        }
                                    }

                                }

                                // Auswertung Versuch 1: sind alle 3 Symbole gleich?
                                if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                                {
                                    versuch1 = 2;                   // gewonnen
                                }
                                else
                                {
                                    versuch1 = 1;                   // verloren
                                }

                                break; // Ende Ablauf bei 1 Versuch

                            case 2: // ---------- Ablauf bei 2 Versuchen ----------
                                {
                                    // --- Versuch 1 ---
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
                                    // Auswertung Versuch 1
                                    if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                                    {
                                        versuch1 = 2;
                                    }
                                    else
                                    {
                                        versuch1 = 1;
                                    }

                                    // --- Versuch 2 (eigenes Array: ergebnis2) ---
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
                                    // Auswertung Versuch 2
                                    if (ergebnis2[0] == ergebnis2[1] && ergebnis2[1] == ergebnis2[2] && ergebnis2[0] != x)
                                    {
                                        versuch2 = 1;
                                    }
                                    else
                                    {
                                        versuch2 = 0;
                                    }
                                }
                                break; // Ende Ablauf bei 2 Versuchen

                            case 3: // ---------- Ablauf bei 3 Versuchen ----------

                                // --- Versuch 1 (hier ist die Ausgabe schon entfernt) ---
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
                                // Auswertung Versuch 1
                                if (ergebnis[0] == ergebnis[1] && ergebnis[1] == ergebnis[2] && ergebnis[0] != x)
                                {
                                    versuch1 = 1;
                                }
                                else
                                {
                                    versuch1 = 0;
                                }

                                // --- Versuch 2 ---
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
                                // Auswertung Versuch 2
                                if (ergebnis2[0] == ergebnis2[1] && ergebnis2[1] == ergebnis2[2] && ergebnis2[0] != x)
                                {
                                    versuch2 = 2;
                                }
                                else
                                {
                                    versuch2 = 1;
                                }

                                // --- Versuch 3 ---
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
                                // Auswertung Versuch 3
                                if (ergebnis3[0] == ergebnis3[1] && ergebnis3[1] == ergebnis3[2] && ergebnis3[0] != x)
                                {
                                    versuch3 = 1;
                                }
                                else
                                {
                                    versuch3 = 0;
                                }
                                break; // Ende Ablauf bei 3 Versuchen

                            default:
                                // Alles andere (0, 4, 5, ...) ist ungültig
                                guthaben = guthaben + einsatz;
                                
                                break;
                        }

                        /*-----------------------------------------------------------AUSGABE------------------------------------------------------------*/

                        // Die Konsole schreibt nur von oben nach unten. Deshalb wird die Tabelle ZEILENWEISE ausgegeben:
                        // z = Zeile (0, 1, 2). In jeder Zeile stehen die Symbole der Versuche nebeneinander.
                        for (int z = 0; z < 3; z++)
                        {
                            Console.Write("\t\t" + ergebnis[z]);                       // Versuch 1, Zeile z
                            if (versuche >= 2) Console.Write("\t\t" + ergebnis2[z]);   // Versuch 2 nur bei mind. 2 Versuchen
                            if (versuche >= 3) Console.Write("\t\t" + ergebnis3[z]);   // Versuch 3 nur bei 3 Versuchen
                            Console.WriteLine();                                       // Zeilenumbruch
                        }

                        /*----------------------------------------------------------AUSWERTUNG----------------------------------------------------------*/

                        // Mindestens ein Versuch gewonnen -> Jackpot
                        if (versuch1 == 2 || versuch2 == 2 || versuch3 == 2)
                        {
                            guthaben = guthaben + einsatz * 3.5;                                  // Gewinn aufs Konto
                            Console.WriteLine(jackpot);
                            Console.WriteLine("Ihr neuer kontostand ist {0} Euro.", guthaben);
                        }
                        else
                        {
                            if (versuch1 == 0 || versuch2 == 0 || versuch3 == 0)
                            {
                                Console.WriteLine(loser);
                                Console.WriteLine("Ihr neuer kontostand ist {0} Euro.", guthaben);
                            }
                            else
                            {
                                Console.WriteLine(nichtGueltig);
                                Console.WriteLine("Ihr neuer kontostand ist {0} Euro.", guthaben);
                            }

                        }
                        Console.Write("\nDrücken sie eine beliebige Taste ...");
                        Console.ReadKey();  // wartet auf eine Taste, damit sich das Fenster nicht sofort schließt
                    }
                } while (guthaben > 0);
                Console.WriteLine("Leider reicht das Guthaben nicht mehr");
                Console.WriteLine("Bitte drücken sie eine Taste ...");
                Console.ReadKey();
                Console.Write("-------------------------------------\n");
            }
        }
    }
}