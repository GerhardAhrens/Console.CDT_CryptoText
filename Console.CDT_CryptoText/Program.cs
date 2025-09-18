//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="PTA GmbH">
//     Class: Program
//     Copyright © PTA GmbH 2025
// </copyright>
// <Template>
// 	Version 2, 18.7.2025
// </Template>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>18.09.2025 08:44:27</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

/* Imports from NET Framework */
using System;

/* Imports from ModernUI Framework */
using ModernConsole.Menu;

namespace Console.CDT_CryptoText
{
    public class Program
    {
        private static void Main(string[] args)
        {
            SmartMenu.Menu("Console Custom Data Typ - CryptoText")
                .Add("Einen String verschlüsseln", () => { MenuPoint1(); })

              .Add("Einen String entschlüsseln", () => { MenuPoint2(); }
                ).Show();

        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            MConsole.Clear();

            CryptoText txt1 = "Hallo GerhardA";
            CryptoText txt2 = "Hallo GerhardB";

            bool result = txt1 == txt2;

            string aa = txt1.ToString();

            CryptoText txtDe = aa;

            string bb = txtDe.ToString();

            CryptoText cc = txt1;

            MConsole.Alert("", "Ergebnis", ModernConsole.Message.ConsoleMessageType.Info);


            MConsole.Wait();
        }

        private static void MenuPoint2()
        {
            MConsole.Clear();

            MConsole.Alert("ebene", "Ergebnis", ModernConsole.Message.ConsoleMessageType.Info);

            MConsole.Wait();
        }
    }
}
