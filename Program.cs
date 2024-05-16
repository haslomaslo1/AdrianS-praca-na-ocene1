using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AplikacjaKonsolowa
{
    public class Osoba
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
    }

    public class Menu
    {
        public virtual void Wyswietl()
        {
            Console.WriteLine("Menu główne:");
            Console.WriteLine("1. Dodaj osobę");
            Console.WriteLine("2. Wyświetl osoby");
            Console.WriteLine("3. Zapisz i wyjdź");
        }
    }

    public class MenuSpecjalne : Menu
    {
        public override void Wyswietl()
        {
            Console.WriteLine("Specjalne Menu:");
            Console.WriteLine("1. Dodaj osobę");
            Console.WriteLine("2. Wyświetl osoby");
            Console.WriteLine("3. Zapisz i wyjdź");
            Console.WriteLine("4. Wyświetl liczbę osób");
        }
    }

    public interface IOperacjeNaOsobach
    {
        void DodajOsobe(Osoba osoba);
        void WyswietlOsoby();
        int LiczbaOsob();
    }

    public class ObslugaOsob : IOperacjeNaOsobach
    {
        public List<Osoba> ListaOsob { get; set; } = new List<Osoba>();

        public void DodajOsobe(Osoba osoba)
        {
            ListaOsob.Add(osoba);
        }

        public void WyswietlOsoby()
        {
            foreach (var osoba in ListaOsob)
            {
                Console.WriteLine($"Imię: {osoba.Imie}, Nazwisko: {osoba.Nazwisko}");
            }
        }

        public int LiczbaOsob()
        {
            return ListaOsob.Count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var obslugaOsob = new ObslugaOsob();

            // Wczytywanie z osoby.json
            if (File.Exists("osoby.json"))
            {
                string json = File.ReadAllText("osoby.json");
                obslugaOsob.ListaOsob = JsonSerializer.Deserialize<List<Osoba>>(json);
            }

            var menu = new MenuSpecjalne();  // Użycie MenuSpecjalne zamiast Menu

            bool repeat = true;
            while (repeat)
            {
                menu.Wyswietl();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj imię: ");
                        string imie = Console.ReadLine();
                        Console.Write("Podaj nazwisko: ");
                        string nazwisko = Console.ReadLine();
                        obslugaOsob.DodajOsobe(new Osoba { Imie = imie, Nazwisko = nazwisko });
                        break;
                    case "2":
                        obslugaOsob.WyswietlOsoby();
                        break;
                    case "3":
                        repeat = false;
                        break;
                    case "4":
                        int liczbaOsob = obslugaOsob.LiczbaOsob();
                        Console.WriteLine($"Liczba osób w bazie: {liczbaOsob}");
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
            }

            // Zapisywanie do osoby.json
            string jsonOutput = JsonSerializer.Serialize(obslugaOsob.ListaOsob);
            File.WriteAllText("osoby.json", jsonOutput);

            Console.WriteLine("Dane zapisane do pliku osoby.json.");
        }
    }
}