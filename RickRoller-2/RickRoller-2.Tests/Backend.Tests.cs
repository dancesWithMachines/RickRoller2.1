﻿using System;
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
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // PLEASE SELECT PATH TO YOUR FACEBOOK LOGIN AND PASSWORD, IT MUST BE .txt FILE WHERE FIRST LINE IS YOUR LOGIN AND SECOND IS PASSWORD!
        // THIS IS TO AVOID SHARING YOUR CREDENTIALS ON GITHUB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private string path = "D:/cred.txt";
        private string sampleText = "D:/sampleText.txt";
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Używaj tej funkcji do pobrania twojego loginu i hasła        
        private string getCredentials(int n)
        {
            string[] list = new string[2];
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                int i = 0;
                while ((line = streamReader.ReadLine()) != null  && i<2)
                {
                    list[i] = line;
                    i++;
                }
            }
            return list[n];

            
        }
        
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
        [Test]
        public void DoesGetFriendsListThrowsExceptionWithoutLogging()
        {
            Backend rickRoller = new Backend();
            var ex = Assert.Throws<OpenQA.Selenium.NoSuchElementException>(() => rickRoller.getFriendsList());
            Assert.That(ex.GetType().Name, Is.EqualTo("NoSuchElementException"));

        }
        //Sprawdzić czy metoda GetFriendsList zwraca cokolwiek
        [Test]
        public void DoesGetFriendsListReturnsNotEmpty()
        {
            Backend rickRoller = new Backend();
            rickRoller.login(getCredentials(0), getCredentials(1));
            var table = rickRoller.getFriendsList();
            Assert.NotNull(table);
            rickRoller.killBrowser();
        }
        //Sprawdzić czy metoda GetFriendsList zwraca tablicę stringów
        [Test]
        public void DoesGetFriendsListReturnsStringTable()
        {
            Backend rickRoller = new Backend();
            rickRoller.login(getCredentials(0), getCredentials(1));
            var table = rickRoller.getFriendsList();
            Assert.AreEqual(table.GetType().ToString(), "System.String[]");
            rickRoller.killBrowser();
        }
        //Sprawdzić czy metoda RickRoll zwraca wyjątek bez ?logowania?
        [Test]
        public void DoesRickRollFriendThrowsExceptionWithoutLogging()
        {
            Backend rickRoller = new Backend();
            var ex = Assert.Throws<OpenQA.Selenium.NoSuchElementException>(() => rickRoller.rickRoll("Ham Burger",sampleText));
            Assert.That(ex.GetType().Name, Is.EqualTo("NoSuchElementException"));
            rickRoller.killBrowser();

        }

        //Sprawdzić czy metoda Rickroll nie zwraca wyjątku przy poprawnym logowaniu

        [Test]
        public void DoesRickRollThrowsNoExceptionWhenValidLogin()
        {
            // Arrange
            Backend rickRoller = new Backend();
            //Assert
            rickRoller.login(getCredentials(0), getCredentials(1));
            //Należy wstawić imie i nazwisko prawdziwego znajomego!
            Assert.DoesNotThrow(() => rickRoller.rickRoll("Anastazja Rozenska", sampleText));
            rickRoller.killBrowser();
        }

        //Sprawdzić czy metoda Rickrol zwraca wyjątek przy podaniu "złego" znajomego
        [Test]
        public void DoesRickRollThrowsExceptionWhenInvalid()
        {
            // Arrange
            Backend rickRoller = new Backend();
            rickRoller.login(getCredentials(0), getCredentials(1));
            var ex = Assert.Throws<OpenQA.Selenium.NoSuchElementException>(() => rickRoller.rickRoll("Ham Burger", sampleText));
            Assert.That(ex.GetType().Name, Is.EqualTo("NoSuchElementException"));
            rickRoller.killBrowser();
        }

        //Sprawdzić czy metoda rickRoll nie zwraca wyjątku przy podaniu "dobrego" znajomego
        [Test]
        public void DoesRickRollThrowsExceptionWhenValid()
        {
            //Arange
            Backend rickRoller = new Backend();
            rickRoller.login(getCredentials(0), getCredentials(1));
            Assert.DoesNotThrow(() => rickRoller.rickRoll("Mateusz Kusiak", sampleText));
            rickRoller.killBrowser();
        }
        //Sprawdzić czy metoda songReader zwraca poprawną ArrayListę <--można testcase
        //Sprawdzić czy metoda SongReader zwraca wyjątek przy niepoprawnej ścieżce do pliku <--można testcase
        //Sprawdzić czy metoda sonngReader zwraca wyjątek gdy plik nie jest plikiem ".txt"  <--można testcase
        [TestCase("D:/sampleText.txt")]
        [TestCase("D:/TextSample.txt")]
        [TestCase("D:/sampleText.doc")]
        [TestCase("D:/sampleText.exe")]
        [TestCase("D:/sampleText.xml")]
        public void DoesSongReaderReturnValidArrayList(string songReaderText)
        {            
            Backend rickRoller = new Backend();
            rickRoller.login(getCredentials(0), getCredentials(1));
            var arrayList = rickRoller.songReader(songReaderText);
            Assert.AreEqual(arrayList.GetType(), arrayList.GetType());
            rickRoller.rickRoll("Mateusz Kusiak", "D:/sampleText.exe");
            rickRoller.killBrowser();
        }        
        //Sprawdzić przy użyciu selenium czy logując się testowo i przez metodę zwracana jest ta sama strona

        //sprawdzić czy metoda killbrowser zwraca wyjątek bez logowania;

        //sprawdzić czy metoda killbrowser nie zwraca błędu przy logowaniu;

        //Michu wymyśl coś na mock'a bo ja tego nie ogarniam xd

        //Dodać przechwytywanie błędów do statusu w formatce
    }
}
