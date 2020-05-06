using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RickRoller_2;

namespace RickRoller_2.Tests
{
    [TestFixture]
    public class Backend_tests
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        string path = "C:/cred.txt";
        public string getCredentials(int n)
        {
            string[] list = new string[2];
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                int i = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list[i] = line;
                    i++;
                }
            }
            return list[n];
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!                                                                                                                                                                                                 

        //Sprawdzić czy logowanie poprawnego użytkownika nie zwraca wyjątku
        [Test]
        public void DoesLoggingValidUserThrowsNoException()
        {
            // Arrange
            Backend rickRoller = new Backend();
            //Assert
            Assert.DoesNotThrow(() => rickRoller.login(getCredentials(0), getCredentials(1)));
            rickRoller.killBrowser();
        }

        //Sprawdić czy logowanie bzur zwraca wyjątek
        [Test]
        public void DoesLoggingInvalidUserThrowsException()
        {
            Backend rickRoller = new Backend();
            var ex = Assert.Throws<OpenQA.Selenium.NoSuchElementException> (() => rickRoller.login("Ham","Burger"));
            Assert.That(ex.GetType().Name, Is.EqualTo("NoSuchElementException"));
            rickRoller.killBrowser();
        }

        //Sprawdzić czy metoda GetFriendsList zwraca wyjątek bez ?logownia?

        //Sprawdzić czy metoda GetFriendsList zwraca tablicę stringów

        //Sprawdzić czy metoda RickRoll zwraca wyjątek bez ?logowania?

        //Sprawdzić czy metoda Rickroll nie zwraca wyjątku przy poprawnym logowaniu

        //Sprawdzić czy metoda Rickrol zwraca wyjątek przy podaniu "złego" znajomego

        //Sprawdzić czy metoda Rickroll nie zwraca wyjątku przy podaniu "dobrego" znajomego

        //Sprawdzić czy metoda songReader zwraca poprawną ArrayListę <--można testcase

        //Sprawdzić czy metoda SongReader zwraca wyjątek przy niepoprawnej ścieżce do pliku <--można testcase

        //Sprawdzić czy metoda sonngReader zwraca wyjątek gdy plik nie jest plikiem ".txt"  <--można testcase

        //Sprawdzić przy użyciu selenium czy logując się testowo i przez metodę zwracana jest ta sama strona

        //Michu wymyśl coś na mock'a bo ja tego nie ogarniam xd

        //Dodać przechwytywanie błędów do statusu w formatce
    }
}
